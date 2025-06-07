# Match Results

This library has to deal with values that sometimes might be present, and sometimes not. This can be represented
by the 'optional value' pattern.

Unfortunately, C# does not have a native optional type. The 1.x versions of this library used
[language-ext](https://github.com/louthy/language-ext) to include the optional type. There are several reasons
why this library was dropped in version 2:

- language-ext is a full-blown framework for functional programming. Users may not want that much functionality.
They may want just pattern matching and nothing else.
- Users may use a different library/framework for functional features. There are a lot of those for C#.

So, starting with version 2, instead of using the `OptionUnsafe<T>` type from language-ext, this library includes
a `MatchResult<T>` type. This type is much like optional types from other libraries in that it may contain a value,
or may not, except for a couple differences:

- `MatchResult<T>` is not a general-purpose optional type and shouldn't be used as such.
- `MatchResult<T>` can contain `null` values and this is important to remember. This type is not used to get rid
of `NullReferenceException`.

## Working with Match Results

`MatchResult<T>` is a struct with value-based equality. Values of this type can be created through methods in the
`MatchResult` class: `Success` and `Failure`.

The simplest way to work with match results is to use the `IsSuccessful` and `Value` properties. `Value` throws
an exception if the result doesn't contain a value.

If you are using a functional library/framework, you can write an extension method which transforms `MatchResult<T>`
into the library's optional type. But remember that `MatchResult<T>` can contain `null` values, and not all optional
implementations support that.

## LINQ to Results

The `Matchmaker.Linq` namespace contains several extension methods to make working with match results easier:

- `GetValueOrDefault` lets you safely get the value of the result.
- `GetValueOrThrow` lets you get the value of the result or explicitly throw an exception of your choice.
- `Select` maps the value of the result if it's present.
- `Bind` flat-maps the value of the result if it's present.
- `Where` filters the result's value if it's present.
- `Cast` casts the result's value to the specified type if it's present and can be cast to that type. Note that
`null` values cannot be cast to a non-nullable value type, and if a successful result which contains a `null` value
is cast to a non-nullable value type, you will get a failed result.
- `Do` performs an action on the result's value if it's present and returns the result itself.

Since there are the `Select` and `Where` extensions on `MatchResult<T>`, you can write them using C#'s query
syntax.
