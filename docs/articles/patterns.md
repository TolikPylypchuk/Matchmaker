# Patterns

One of two central ideas of this library is a _pattern_. Patterns are used to match the input value
against a certain 'shape'. For example, 'the input value is equal to some value' or 'the input value
has some type'.

## The `IPattern<TInput, TMatchResult>` interface

A pattern is an object which implements the `Matchmaker.Patterns.IPattern<TInput, TMatchResult>` interface.
This interface contains two members:

```
string Description { get; }

MatchResult<TMatchResult> Match(Tinput input);
```

The `Match` method actually does two things:

 - Matches the input value with the pattern and returns a successful result if the match is successful.
 - Transforms the input value. The returned result contains the transformed value if it's successful.

Since options are not supported natively in C#, a custom type - [`MatchResult<T>`](results.md) - is used.

The definition of patterns is similar to F#'s active patterns.

Descriptions for patterns are not terribly important but can be useful for debugging. As such, they are optional -
if you don't want a pattern to have a description, it should be empty. A pattern's description should _never_ be `null`.

## Null Values

The results of patterns' matching can be `null`. This is why the `MatchResult<T>` type can contain a `null` value.

## Using Predefined Patterns

There are several predefined patterns in the `Matchmaker.Patterns.Pattern` class:

 - `Any` is always matched successfully and returns its input.
 - `Return` is also always matched successfully but returns the specified value instead of its input.
 - `Null` is matched successfully if its input is `null`.
 - `ValueNull` is also matched successfully if its input is `null` but is used for nullable value types.
 - `EqualTo` is matched successfully if its input is equal to the specified value.
 - `LessThan`, `LessOrEqual`, `GreaterThan` and `GreaterOrEqual` are matched successfully if their input
is compared successfully to the specified value.
 - `Type` is matched successfully if its input has the specified type. It returns its input as a value of that type.
If the input is `null`, then the match will fail only if the destination type is a non-nullable value type.
 - `Not` is matched successfully if the specified pattern is not matched successfully. It ignores the specified
pattern's transformation and returns its input if matched successfully.

The patterns which compare their inputs can also take a custom comparer which will be used to compare values.

The patterns that take a value can also take a value provider (a function which returns the value) in order
to lazily match the input. The value provider will be called once upon the first call to the `Match` method
and then its result will be cached. Note that the caching process is not thread-safe.

All methods for getting predefined patterns are overloaded to take a custom description.

## LINQ to Patterns

The `Matchmaker.Linq` namespace provides several extension methods for patterns:

 - `Select` maps a pattern's result value if it's successful.
 - `Pipe` creates a pattern pipeline - the result of the first pattern is the input of the second pattern.
 - `Cast` casts a pattern's result to a specified type. It's the same as piping a pattern to the `Type` pattern.
If the input is `null`, then the match will fail only if the destination type is a non-nullable value type.
 - `Bind` flat-maps a pattern's result. If a pattern's result is successful, it calls the
specified function and passes the result to it. The function returns another pattern which is then used to match
the input. The second pattern's result is the final result.
 - `Where` filters the pattern's result if it's successful.
 - `And`, `Or` and `Xor` compose two patterns. The resulting pattern ignores the patterns' transformations and
returns the input if successful.
 - `Compose` is the same as the three methods above, but the composition operator is passed to it as well.
 - `Cached` returns a pattern which matches the same as the specified pattern but caches its results in a `null`-safe
hash table. Every input will be matched only once - if it's matched again, the result will be taken from the
cache. The caching process is not thread-safe.

All extension methods for patterns are overloaded to take a custom description.

Since there are the `Select` and `Where` extensions on patterns, you can write them using C#'s query syntax.

Patterns have the `Bind` and `Return` functions, so they are monads (I haven't tested the monad laws though).
To be more specific, patterns can be thought of as a combination of the Reader and Maybe monads.

## Immutability

Every predefined pattern as well as patterns returned by the `CreatePattern` and extension methods are immutable.
Calling an extension method on a pattern returns a new pattern - the old one is unchanged.

An exception is the pattern returned by the `Cached` method, which is not immutable - it holds a mutable cache.
But if a pattern is referentially transparent (its `Match` method always returns the same result for the same input and
doesn't have any side effects), then the caching pattern based on it can be thought of as immutable as well, because it
doesn't matter how many times the base pattern's `Match` method is called.

## Creating Custom Patterns

This library works with arbitrary patterns. There are several ways to create custom patterns.

### The Easy Way

The `Pattern` class contains the `CreatePattern` methods which create patterns from functions. There are two
variations:

 - Create a pattern from a predicate. This predicate will be used to test the value. If it returns `true`, then the
input value will be the result.
 - Create a pattern from a matcher function. This is a function which has the same signature as the `Match` method
of the `IPattern<TInput, TMatchResult>` interface. This function will be used to match inputs.

There are also overloads which take a description.

### The Hard Way

If you want something more complex than a single function, you can create a class which extends the
`Matchmaker.Patterns.Pattern<TInput, TMatchResult>` class. This is a base class for patterns, and it implements
the `Description` property which you don't have to use if you don't want – by default the description is empty, which
means that the pattern doesn't have a description.

You can also implement the `IPattern<TInput, TMatchResult>` interface directly. But there is no reason to do that
instead of extending the `Pattern<TInput, TMatchResult>` class unless your class already extends another class. But
in that case making your class a pattern will break the single responsibility principle. So, don't do that.

Next article: [Match expressions](expressions.md)
