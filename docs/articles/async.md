# Asynchronous Pattern Matching

Starting with version 3.0, asynchronous pattern matching is also available.

> [!IMPORTANT]
> On .NET Standard 2.0, Matchmaker uses
> [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces) as the polyfill for
> asynchronous interfaces.

## Async Patterns

Async patterns are objects which implement the `Matchmaker.Patterns.Async.IAsyncPattern<TInput, TMatchResult>`
interface. They are very similar to [normal patterns](patterns.md), except they are matched asynchronously. The
interface contains two members:

```c#
string Description { get; }

Task<MatchResult<TMatchResult>> MatchAsync(Tinput input);
```

The predefined patterns (contained in the `Matchmaker.Patterns.Async.AsyncPattern` class) and extension methods from
`MatchMaker.Linq` are basically the same. There are also extensions for `Task<MatchResult<TMatchResult>>`.

The only difference is that the `Cached` extension method returns a pattern which caches its inputs in a thread-safe
manner.

Creating custom async patterns is also basically the same as creating custom patterns: you can use the `CreatePattern`
methods from the `AsyncPattern` class, extend the `Matchmaker.Patterns.Async.AsyncPattern<TInput, TMatchResult>` class
to get the `Description` property for free, or implement the `IAsyncPattern<TInput, TMatchResult>` directly.

Normal patterns can be turned into async patterns by calling the `AsAsync()` extension (defined in the `Matchmaker.Linq`
namespace).

## Async Match Expressions

Async match expressions are also very similar to [normal match expressions](expressions.md). There are two types of
async match expressions: `AsyncMatch<TInput, TOutput>` which yields a result and `AsyncMatch<TInput>` which doesn't.
They can be created using methods in the `AsyncMatch` class.

The `Case` methods of async match expressions are overloaded to take either async patterns or normal patterns (which are
turned into async patterns using the `AsAsync()` extension) and to take either async or sync actions to executed when a
pattern is matched successfully (sync actions are turned into async actions).

Async match expressions can be executed using the `ExecuteAsync`, `ExecuteNonStrictAsync` and
`ExecuteWithFallthroughAsync` methods.  The `ToFunction` method and its variations are also available.
`ExecuteWithFallthroughAsync` returns an `IAsyncEnumerable` which enables lazy async execution of match expressions.

The `Matchmaker.Linq` namespace contains the `EnumerateAsync` extension method for `IAsyncEnumerable<T>` which
enumerates it and ignores the result. You can use it if you just want to execute the match statement with fall-through.

Static async match expressions are also available. Use the `AsyncMatch.CreateStatic` methods to create them, just like
normal match expressions. The methods accept an action on either `AsyncMatchBuilder<TInput, TOutput>` or
`AsyncMatchBuilder<TInput>`. Static match expressions are globally cached and the caching process is thread-safe. Caches
can be cleared with the `ClearCache` methods in `AsyncMatch`.
