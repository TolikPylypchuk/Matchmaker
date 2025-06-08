# Nullable Reference Types

Starting with version 3.0, support for nullable reference types (NRTs) is enabled.

Support for NRTs is limited in this library because it uses generics extensively, and generics don't play nicely with
NRTs.

> [!NOTE]
> On .NET Standard 2.0, support for NRTs is even more limited, since attributes for null-state static analysis are not
> available, and Matchmaker simply elides them on this TFM instead of using a polyfill.

Since match expressions can work will `null`, their return types are marked as such. Match results may also contain
`null` values. This may be inconvenient if you know that `null` values are not used as match results.

In addition, extensions in `Matchmaker.Linq` also don't fully support NRTs. Look at the (simplified) implementation of
the `Select` extension for match results:

```c#
public static MatchResult<TResult> Select<T, TResult>(this MatchResult<T> result, Func<T, TResult> mapper) =>
    result.IsSuccessful ? MatchResult.Success(mapper(result.Value!)) : MatchResult.Failure<TResult>();
```

Here's the interesting part: `mapper(result.Value!)`. There is no way to tell C# that `mapper` can receive `null`
inputs, because `T` is generic and we cannot use `T?`.

Since extensions for patterns use extensions for match results, this also applies to extensions for patterns. There is
no way to tell C# that functions passed to those extensions can receive `null`, but they can, and this should be
accounted for.

When using this library with NRTs enabled, remember two things:

- `MatchResult<T>` may contain `null` and so patterns can return `null` as their results as well
- Match expressions' results may be `null`
