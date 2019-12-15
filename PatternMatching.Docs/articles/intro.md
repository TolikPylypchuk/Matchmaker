# Introduction

## A Simple Example

This is what the simplest match expression looks like:

```
using static PatternMatching.Pattern;

// ...

string result =
    Match.Create<int, string>()
        .Case(EqualTo(1), _ => "one")
        .Case(EqualTo(2), _ => "two")
        .Case(EqualTo(3), _ => "three")
        .Case(EqualTo(4), _ => "four")
        .Case(Any<int>(), i => i.ToString())
        .ExecuteOn(5);
```

`EqualTo` is a predefined pattern.

This is what an equivalent `switch` statement looks like:

```
string result;
int i = 5;

switch (i)
{
    case 1:
        result = "one";
        break;
    case 2:
        result = "two";
        break;
    case 3:
        result = "three";
        break;
    case 4:
        result = "four";
        break;
    default:
        result = i.ToString();
        break;
}
```

While this example doesn't show the full power of pattern matching, there are
a few things to note here:

 - The match expression yields a result. We don't have to assign the result
explicitly in each case.

 - The input of the match expression is specified _after_ all the cases. This
allows us to save the match expression in an object, and use it multiple times
on different input values.

 - The default case is a pattern, just like any other. It's called `Any` and
is always matched successfully.

 - Like in `switch` the patterns are tried out sequentially. This means that
the `Any` pattern should always come last.

## Patterns

One of two central ideas of this library is a _pattern_. A pattern is an
object which implements the `IPattern<TInput, TMatchResult>`
interface. This interface contains only one method -
`OptionUnsafe<TMatchResult> Match(Tinput input)`. The `Match` method
actually does two things:

 - Matches the input value with the pattern and returns a non-empty option
if the match is successful

 - Transforms the input value. The returned option contains the result of
the transformation.

Since options are not supported in C#,
[language-ext](https://github.com/louthy/language-ext) is used.

The definition of patterns is similar to F#'s active patterns.

Usually one does not have to implement the `IPattern` interface directly.
Some predefined patterns are available and can be used in most situations:

 - `ConditionalPattern<TInput, TOutput>` is an abstract class, which
enables adding additional conditions to the pattern.

 - `Pattern<TInput, TMatchResult>` is a pattern, which can be constructed
from a function with the same signature as `Match`. This class extends the
`ConditionalPattern<TInput, TOutput>`.

 - `SimplePattern<TInput>` is a non-transforming pattern, which can be
constructed from a predicate. This class extends the
`ConditionalPattern<TInput, TOutput>`.

 - Several objects of type `SimplePattern<TInput>`and
`Pattern<TInput, TMatchResult>` are defined in the static `Pattern` class.

`SimplePattern` can be easily combined with any other `SimplePattern`.
Methods, such as `And`, `Or` and others are defined (and operators,
such as `&` and `|`).

## Match Expressions

The second central idea is the match expression itself. It is represented
by two classes: `Match<TInput, TOutput>` and `Match<TInput>`. The difference
between them is that the former represents a match expression, which yields
a result, and the latter represents a match expression, which doesn't yield
a result (also known as match statement).

A match expression can be created using the `Create` methods of the static
class `Match`.

The `Match` classes include a `Case` method which is used to add a
pattern and a function, which is executed if the match is successful, to
the expression.

To execute a match expression, the `ExecuteOn` method is used. It takes the
input value to match. In the `Match<TInput, TOutput>` class this method
returns the result of the match, or throws a `MatchException` if no successful
match was found. In the `Match<TInput>` class this method returns a boolean
value, which signifies whether the match was successful. This class also contains
the `ExecuteOnStrict` method, which also throws the `MatchException` if
the match is not successful.

The `ToFunction` method is also available. It returns a function which, when
called, will execute the match expression.

## A more complex example

Let's consider another example of match expressions:

```
string result =
    Match.Create<int, string>()
        .Case(
            LessThan(1),
            _ => "x < 1")
        .Case(
            GreaterOrEqual(1) & LessThan(2),
            _ => "1 <= x < 2")
        .Case(
             GreaterOrEqual(2) & LessThan(3),
             _ => "2 <= x < 3")
        .Case(
            GreaterOrEqual(3) & LessThan(4),
            _ => "3 <= x < 4")
        .Case(
            GreaterOrEqual(4) & Not(GreaterThan(5)),
            _ => "4 <= x <= 5")
        .Case(
            Any<int>(),
            _ => "5 < x")
        .ExecuteOn(5);
```

This is what an equivalent `switch` statement looks like:

```
string result;
int i = 5;

switch (i)
{
    case var _ when i < 1:
        result = "x < 1";
        break;
    case var _ when 1 <= i && i < 2:
        result = "1 <= x < 2";
        break;
    case var _ when 2 <= i && i < 3:
        result = "2 <= x < 3";
        break;
    case var _ when 3 <= i && i < 4:
        result = "2 <= x < 3";
        break;
    case var _ when 4 <= i && !(i > 5):
        result = "4 <= x <= 5";
        break;
    default:
        result = "5 < x";
        break;
}
```

This is some very non-idiomatic usage of `switch` statements, but can easily
be rewritten using the `if-else-if` statement, which is still a statement,
and thus cannot yield a result.

There are also lazy equivalents of these patterns, which take value providers
instead of values, for example `EqualTo(() => 1)`. They are useful, when the
value to compare to is obtained with a long-running computation.

## Null Values

This library uses unsafe options, which can store `null` values, so these values
are fully supported. There are the `Null` and `ValueNull` patterns to match
a class or a nullable struct respectively.

## Strict Mode vs. Non-Strict Mode

Both match expressions and match statements have two modes: strict and non-strict.
When matching in strict mode, an exception is thrown if no successful patterns
are found in a match expression. On the other hand, when matching in non-strict
mode, no exceptions will be thrown. This means that a match expression has to
return an optional value.

*Note*: The default mode in a match expression is strict, because one would
usually want a result. The default mode in a match statement is non-strict,
because one would usually not care whether a successful match was actually found.
That's why a match expression contains the `ExecuteOn` and `ExecuteNonStrict` methods
and a match statement contains the `ExecuteOn` and `ExecuteStrict` methods.

## Performance

I didn't perform any benchmarks, but I can guess that pattern matching here is
much, _much_ slower than the traditional `switch` statements. This is because
the matches use dynamic values internally.

The matches contain a list of tuples of patterns and functions to execute.
This list has dynamic items in it because the match expression knows nothing about
transformations of the patterns. If it did, then the information about each type
of the pattern transformation would be required, and that would render the match
either unusable, because of the many types which will have to be specified, or
impossible, because there would always be a finite amount of match types
(each with information about one more match result type than the previous).

The type safety is not compromised this way, because the match result type is
needed only between the execution of the pattern match and the execution of
the function, and is not visible to the outside world.

This incurs a performance overhead, but it must be compromised in order
for this to work.
