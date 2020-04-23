# Migration Guide

This article describes how to migrate to version 2.0.0 from
[PatternMatching v1.2.0](https://github.com/TolikPylypchuk/PatternMatching).

If you need to migrate from older versions, first migrate to version 1.2.0. You can do that just by reading the
changelog, because previous versions contained much fewer changes and it's much simpler to migrate.

In order to migrate from version 2.0.0 to version 2.1.0 action is required only if you implemented the
`IPattern<TInput, TMatchResult>` interface (which wasn't recommended). The less generic `IPattern<TInput>`
interface was removed, so you should remove its `Match` method. That's it.

## General

Since this library has a new name, the namespace is different as well. Instead of the `PatternMatching` namespace
there are three namespaces: `Matchmaker`, `Matchmaker.Patterns` and `Matchmaker.Linq`.

The dependency on [language-ext](https://github.com/louthy/language-ext) was dropped. Instead of using `OptionUnsafe`
this library now uses its own `MatchResult`.

## Patterns

The predefined patterns haven't changed much, but the way to create custom patterns changed drastically.

The `Match` method in `IPattern<TInput, TMatchResult>` now returns `MatchResult<TMatchResult>` instead of
`OptionUnsafe<TMatchResult>`.

The `IPattern<TInput, TMatchResult>` now contains the `Description` property. If you implemented this interface,
you can add it and make it return an empty string if you don't want a description.

`IPattern<TInput, TMatchResult>` now extends `IPattern<TInput>`. If you implemented this interface, it's better
to extend the new `Pattern<TInput, TMatchResult>` class instead, as it provides both an implementation of the less
generic interface, and the `Description` property.

The `ConditionalPattern<TInput, TMatchResult, TPattern>` is gone. It's too much work for the users to implement
filtering of patterns if they want a highly customized pattern. If you filtered pattern results with the `When` method
before, now  you can do that using the `Where` extension method from the `Matchmaker.Linq` namespace, which works on
all patterns, and doesn't need a special base class. I renamed this method because previously I wanted it to look like
the `when` clause of the `switch` expressions, but now this method is part of LINQ to Patterns, so `Where` is more
appropriate.

`SimplePattern<TInput>` is gone and `Pattern<TInput, TMatchResult>` now serves as a base class for patterns.
If you need to create a pattern from a function or a predicate, use the `CreatePattern` methods in the `Pattern`
class.

Since `SimplePattern<TInput>` is gone, you cannot use operators to compose patterns (`&`, `|` and `^`). Also,
the `And`, `Or` and `Xor` methods are not part of pattern definition - they are extension methods from the
`Matchmaker.Linq` namespace. The `~` operator is gone as well. Use `Not` instead.

The `StructNull` pattern is gone. It was deprecated in version 1.2.0. The `ValueNull` pattern provides the same
functionality.

## Match Expressions

The default mode of execution for match statements is now strict. So `ExecuteOn` is strict. The `ExecuteStrict`
method is gone and `ExecuteNonStrict` is added. This was changed because it was weird to have different default modes
in match expressions and match statements.

Matching with fall-through became lazy. This is major change. The result in match expressions is now not the `Lst`
from language-ext, but a lazy `IEnumerable` which the user has to enumerate for the match to actually be executed.
Match statements also return an `IEnumerable` instead of a number of successful cases. If you want to know the number
of successful cases, you can call `Count()` on the result of the match. Or you can call `Enumerate` from
`Matchmaker.Linq` if you just want to execute it. Since matching with fall-though is lazy, it doesn't have modes
of execution - it's just non-strict. The library cannot decide to throw an exception if there are no successful cases
because the library doesn't decide when to execute the expression.

Next article: [Why this library was written](why.md)
