# Match Expressions

The second central idea is the match expression itself. It is represented by two classes: `Match<TInput, TOutput>` and
`Match<TInput>`. The difference between them is that the former represents a match expression which yields a result, and
the latter represents a match expression which doesn't yield a result (also known as a match statement).

## Using Match Expressions

### Creating Match Expressions

A match expression can be created using the `Create` methods of the static class `Match`.

### Adding Cases

The `Match` classes include `Case` methods which are used to add a pattern and a function which is executed if the match
is successful. Match expressions are immutable – `Case` methods return new match expressions; they do not affect the
ones on which they are called.

`Case` methods are generic – they also contain information about the pattern's transformation type. Match expressions
can contain patterns of arbitrary transformation types without knowing about these types.

### Executing Match Expressions

To execute a match expression, the `ExecuteOn` method is used. It takes the input value to match. There are two modes of
execution in match expressions: strict and non-strict. The strict mode throws an exception if no matches were found, and
the non-strict doesn't.

In the `Match<TInput, TOutput>` class, the `ExecuteOn` method returns the result of the match or throws a
`MatchException` if no successful match was found. `Match<TInput, TOutput>` also contains the `ExecuteNonStrict` method
which executes the match expression in the non-strict mode. It returns `MatchResult<TOutput>` because the result might
not be present.

In the `Match<TInput>` class, the `ExecuteOn` method doesn't return anything, and also throws a `MatchException` if the
match wasn't successful. This class also contains the `ExecuteNonStrict` method – it returns a boolean value which
indicates whether the match was successful and doesn't throw an exception if it wasn't.

The `ToFunction` method and its variations are also available. They return a function which, when called, will execute
the match expression.

## Matching with Fall-through

C, C++ and, Java support fall-through in `switch` statements. So does this library, although it works differently here.

Fall-through must be explicitly enabled for cases and then explicitly enabled during execution. Both `Match` classes
contain the `ExecuteWithFallthrough` method which takes fall-through behavior into account. `ExecuteOn` and
`ExecuteStrict` ignore the fall-through behavior.

If a case has fall-through enabled, then the expression falls to the next successful match, unlike `switch`, which
falls to the next case whether it's successful or not.

The `Case` methods are overloaded to accept a boolean value which indicates the fall-through behavior. If fall-through
is enabled for a pattern, then the expression will continue searching for the next successful pattern. If it isn't, then
the expression will stop at this pattern and not go any further.

`Match.Create` is also overloaded to take the default fall-through behavior.

Matching with fall-through is lazy i.e. it returns an `IEnumerable` and is only executed when this enumerable is
enumerated. Because matching will fall-through is lazy, it doesn't have any modes of execution – the user must decide
whether to throw an exception or not if there were no successful matches.

In the `Match<TInput, TOutput>` class, the `ExecuteWithFallthrough` method returns an `IEnumerable<TOutput>` which can
be used to get all successful match results.

In the `Match<TInput>` class there are no results, so the `ExecuteWithFallthrough` method returns an
`IEnumerable<object>` which should be used simply to enumerate the match process itself. This is implemented so that
matching with fall-through is also lazy in this class. The values of the resulting enumerable don't matter - in fact,
they are always `null`, because match statements don't produce any results. What matters is the process of enumeration.

You can use the LINQ's `Take` method to limit the number of executed matches, or the `Count` method to execute it and
get the number of successful matches.

The `Matchmaker.Linq` namespace contains the `Enumerate` extension method for `IEnumerable<T>` which enumerates it and
ignores the result. You can use it if you just want to execute the match statement with fall-through.

> [!IMPORTANT]
> Matching with fall-through is lazy and is actually executed when the result is enumerated.

Here's a (somewhat convoluted) implementation of the famous fizz-buzz program which uses matching with fall-through:

```c#
using System.Linq;

using Matchmaker;
using Matchmaker.Linq;

using static Matchmaker.Patterns.Pattern;

// ...

IPattern<int, int> DivisibleBy(int n) =>
    CreatePattern<int>(input => input % n == 0);

var result = Enumerable.Range(0, 15)
    .Select(Match.Create<int, string>(fallthroughByDefault: true)
        .Case(DivisibleBy(3), _ => "Fizz")
        .Case(DivisibleBy(5), _ => "Buzz")
        .Case(Not(DivisibleBy(3).Or(DivisibleBy(5))), n => n.ToString())
        .ToFunctionWithFallthrough())
    .Select(items => items.Aggregate(String.Concat))
    .ToList();

// The result is:
// "FizzBuzz", "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz"
```

## Static Match Expressions

### The Initialization Problem

One pain point of match expressions is that whenever a method which contains a match expression is executed, the match
expression is initialized from scratch. Take a look at this example:

```c#
void DoStuff(int i) =>
    Match.Create<int, string>()
        .Case(...)
        .Case(...)
        .Case(...)
        .Case(...)
        .ExecuteOn(i);
```

The problem here is that if we call `DoStuff` 10,000 times, we will initialize the match expression 10,000 times as
well, even though it's actually the same expression. Having just 4 cases may not seem like much, but the lag does
accumulate if we execute it thousands of times.

We can save the expression in a field and then call the `ExecuteOn` method on this field. But this makes the code much
less readable because the case definitions are in a different place from the actual execution point.

### The Solution

There is a way to create static match expressions – expressions which will be initialized only once.

The `Match` class contains the `CreateStatic` methods which allow the creation of static match expressions. Take a look
at the modified example:

```c#
void DoStuff(int i) =>
    Match.CreateStatic<int, string>(builder => builder
            .Case(...)
            .Case(...)
            .Case(...)
            .Case(...))
        .ExecuteOn(i);
```

It looks almost the same, except for one difference: the calls to `Case` methods are inside the lambda expression,
called the build action, which is passed to the `CreateStatic` method. Now this match expression will be initialized
only once, and its initialization code is in the same place as its execution point.

The parameter of the build action has the type `MatchBuilder<TInput, TOutput` or `MatchBuilder<TInput>`, depending on
which type of match expressions you are building. This type has the same methods for adding cases as the `Match` classes
and is mutable – the methods return the same builder instance.

`MatchBuilder` also has the `Fallthrough` method which specifies the default fall-through behavior. But this method
specifies fall-through behavior only for cases that are defined after it. For example:

```c#
builder
    .Fallthrough(true)
    .Case(...) // this case has fall-through behavior if not specified otherwise
    .Case(...) // this case also has fall-through behavior if not specified otherwise
    .Fallthrough(false)
    .Case(...) // this case doesn't have fall-through behavior if not specified otherwise
    .Case(...) // this case also doesn't have fall-through behavior if not specified otherwise
```

Every case can configure its fall-through behavior individually as well.

### Caching Match Expressions

The build action will be called only once, and its result will be cached. The cache is a static hash-table. The caching
process is not thread-safe.

The key of the cache is the place where the `CreateStatic` method is called. Apart from the build action, this method
also accepts two caller info arguments: the path to the source file and the line in the source file. Users don't need to
pass these arguments to the method as they have the `CallerFilePath` and `CallerLineNumber` attributes.

The `Match` class also contains the `ClearCache` methods which clear a global cache. Match expressions have a cache per
type (so `Match<int, int>` uses a different cache than `Match<int, string>`) so `ClearCache` only clears one cache.
Clearing the cache will force all static match expressions of that type to be reinitialized. This process is not
thread-safe as well.
