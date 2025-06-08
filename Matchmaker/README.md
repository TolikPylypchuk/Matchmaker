# Matchmaker

A library which enables more powerful pattern matching than is currently available in the C#'s `switch`
statement/expression.

This library is a successor of [PatternMatching](https://github.com/TolikPylypchuk/PatternMatching). Version 1.x can be
found there. This package contains version 2+.

## A Simple Example

This is what the simplest match expression looks like:

```c#
using static Matchmaker.Patterns.Pattern;

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

This is what an equivalent `switch` statement looks like (pre-C# 8):

```c#
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

While this example doesn't show the full power of pattern matching, there are a few things to note here:

- The match expression yields a result. We don't have to assign the result explicitly in each case.

- The input of the match expression is specified _after_ all the cases. This allows us to save the match expression in
an object, and use it multiple times on different input values.

- The default case is a pattern, just like any other. It's called `Any` and is always matched successfully.

- Like in `switch` the patterns are tried out sequentially. This means that the `Any` pattern should always come last.

C# 8 included a new way to write `switch` expressions which yield a value, and further versions extended it quite a bit.
This drastically reduced the need for external libraries like this one for pattern matching. However, this library lets
the user define arbitrary patterns, which makes this library more powerful than the `switch` expressions.

Here's what the equivalent switch expression looks like in C# 8 or later:

```c#
int i = 5;

string result = i switch
{
    1 => "one",
    2 => "two",
    3 => "three",
    4 => "four",
    _ => i.ToString()
};
```

OK, this is much shorter and cleaner than the previous two examples. But this library shines when the patterns are more
complex. While C# allows various kinds of patterns, this library allows anything you can think of.

## Another Example

Let's define a simple list, implemented as [cons cells](https://en.wikipedia.org/wiki/Cons). This list is not generic
for simplicity.

```c#
public abstract class ConsList
{
    private protected ConsList()
    { }

    public static ConsList Cell(int head, ConsList tail) =>
        new ConsCell(head, tail);

    public static ConsList Empty =>
        new Empty();
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
    internal Empty()
    { }
}
```

Now let's look what pattern matching on the list would look like. Let's create a function which finds the sum of all
items of the list.

```c#
public int Sum(ConsList list) =>
    Match.Create<ConsList, int>()
        .Case<ConsCell>(cell => cell.Head + Sum(cell.Tail))
        .Case<Empty>(_ => 0)
        .ExecuteOn(list);
```

`Case<TType>(...)` is the same as `Case(Pattern.Type<TInput, TType>(), ...)`.

Here is the equivalent function implemented using the `switch` statement (pre-C# 8):

```c#
public int Sum(ConsList list)
{
    switch (list)
    {
        case ConsCell cell:
            return cell.Head + Sum(cell.Tail);
        case Empty _:
            return 0;
    }

    throw new MatchException("This will never happen, but C# can't know that.");
}
```

As you can see, we have to throw an exception in the `switch` version, because C# can't know that `ConsCell` and `Empty`
are the only possible subclasses of `ConsList`. And for that reason, if we forget to define one of the cases in `switch`
or in a match, we'll get an exception. In F#, a warning is issued when the match is incomplete, but C# doesn't have the
notion of complete or incomplete matches.

With C# 8 there's a better way to do this, but we still have to explicitly throw an exception in the default case (which
we know won't happen):

```c#
public int Sum(ConsList list) =>
    list switch
    {
        ConsCell cell => cell.Head + Sum(cell.Tail),
        Empty _ => 0,
        _ => throw new MatchException("This will never happen, but C# can't know that.");
    };
}
```

## Matching with Fall-through

C, C++ and, Java support fall-through in `switch` statements. So does this library, although it works differently here.
You can read more [here](https://matchmaker.tolik.io/articles/expressions.html#matching-with-fall-through).

Here's an implementation of the famous fizz-buzz program which uses matching with fall-through:

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

One pain point of match expressions is that whenever a method which contains a match expression is executed, the match
expression is initialized from scratch every time. This can be solved with static match expressions. Take a look at the
revised simple example:

```c#
string result = Match.CreateStatic<int, string>(match => match
        .Case(EqualTo(1), _ => "one")
        .Case(EqualTo(2), _ => "two")
        .Case(EqualTo(3), _ => "three")
        .Case(EqualTo(4), _ => "four")
        .Case(Any<int>(), i => i.ToString()))
    .ExecuteOn(5);
```

Now this match expression will be initialized only once even if its containing method is executed multiple times. You
can read more [here](https://matchmaker.tolik.io/articles/expressions.html#static-match-expressions).

## More Info

If you want to learn how to use this library, you should read the [documentation](https://matchmaker.tolik.io). The
articles provide everything you need to know to use this library.

If you need extensive information, go to the [API reference](https://matchmaker.tolik.io/api/index.html).

If you need even more info about this library, you can go through the
[tests](https://github.com/TolikPylypchuk/Matchmaker/Matchmaker.Tests). They are property-based and as such, they
describe every aspect of the classes and their members.

## Is This Library Still Maintained?

I'm not planning on writing new versions beyond 3.1. To be fair, I thought the same thing after releasing version 1.1
and yet here we are. This time I do believe that this library has enough features (probably more than enough). Maybe one
day I'll revisit this decision, but for now (June 2025) this is it; this is as good as it gets.

That said, if you report a bug or request a new feature, I'll definitely look into it. I'm not giving up on this library
any time soon.
