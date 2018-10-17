# Introduction

## A simple example

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
`Option<TMatchResult> Match(Tinput input)`. The `Match` method
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

## Matchers

The second central idea is the _matcher_, which represents the match expression
itself. It is represented by two classes: `Matcher<TInput, TOutput>` and
`Matcher<TInput>`. The difference between them is that the former represents
a match expression, which yields a result, and the latter represents a match
expression, which doesn't yield a result (also known as match statement).

A match expression can be created using the `Create` methods of the static
class `Match`.

The `Matcher` classes include a `Case` method which is used to add a
pattern and a function, which is executed if the match is successful, to
the expression.

To execute a match expression, the `ExecuteOn` method is used. It takes the
input value to match. In the `Matcher<TInput, TOutput>` class this method
returns the result of the match, or throws a `MatchException` if no successful
match was found. In the `Matcher<TInput>` class this method returns a boolean
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

## Discriminated unions

While discriminated unions are not directly supported in C#, they can be modelled
using class hierarchies. But still, in order for them be user-friendly, a way to
process the values has to be implemented, usually in the form of the Visitor pattern.

Or, pattern matching can be used instead of visitors.

Let's define a very simple list, implemented as cons cells. This list is not generic
for simplicity.

```
public abstract class ConsList
{
    private protected ConsList() { }
    
    public static ConsList Cell(int head, ConsList tail)
        => new ConsCell(head, tail);
    
    public static ConsList Empty
        => new Empty();
}

public sealed class ConsCell : ConsList
{
    public int Head { get; }
    public ConsList Tail { get; }
    
    internal ConsCell(int head, ConsList tail)
    {
        this.Head = head;
        this.Tail = tail;
    }
}

public sealed class Empty : ConsList
{
    internal Empty() { }
}
```

Now let's look what pattern matching on the list whould look like. Let's create
a function which finds the sum of all items of the list.

```
Func<ConsList, int> sum = null;

sum = Match.Create<ConsList, int>()
	.Case<ConsCell>(cell => cell.Head + sum(cell.Tail))
	.Case<Empty>(_ => 0)
	.ToFunction();
```

Here is the equivalent function implemented using the `switch` statement:

```
public int Sum(ConsList list)
{
    switch (list)
    {
        case ConsCell cell:
            return cell.Head + Sum(cell.Tail);
        case Empty _:
            return 0;
    }

    throw new MatchException(
        "This will never happen, but C# can't know that.");
}
```

Note: The declaration of sum must be split from its initialization, because
C# doesn't permit initializing recursive lambda expressions in declaration.

The `Case<TType>(Func<TType, TOutput> func)` is simply shorthand for
`Case(Pattern.Type<TInput, TType> pattern, Func<TType, TOutput> func)`.

The `Type` pattern is matched successfully, if the input value is of the
specifified type.

As we can see, we have to throw an exception in the `switch` version, because
C# can't know that `ConsCell` and `Empty` are the only possible subclasses
of `ConsList`. And for that reason if we forget to define one of the cases
in `switch` or in a match, we'll get an exception. In F# a warning is issued,
when the match is incomplete, but then again, C# doesn't have the notion of
complete or incomplete matches.

## Performance

I didn't perform any benchmarks, but I can guess that pattern matching here is
much, _much_ slower than the traditional `switch` statements. This is because
the matchers use dynamic values internally.

The matchers contain a list of pairs of patterns and functions to execute.
This list has dynamic items in it because the matcher knows nothing about
transformations of the patterns. If it did, then the information about each type
of the pattern transformation would be required, and that would render the matcher
either unusable, because of the many types which will have to be specified, or
impossible, because there would always be a finite amount of matcher types
(each with information about one more match result type than the previous).

The type safety is not compromised this way, because the match result type is
needed only between the execution of the pattern match and the execution of
the function, and is not visible to the outside world.

This incurs a performance overhead, but it must be compromised in order
for this to work.
