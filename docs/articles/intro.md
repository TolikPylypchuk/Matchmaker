# Introduction

## A Simple Example

This is what the simplest match expression looks like:

```
using static Matchmaker.Pattern;

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

While this example doesn't show the full power of pattern matching, there are a few things to note here:

 - The match expression yields a result. We don't have to assign the result explicitly in each case.
 - The input of the match expression is specified _after_ all the cases. This allows us to save the match expression
in an object and use it multiple times on different input values.
 - The default case is a pattern, just like any other. It's called `Any` and is always matched successfully.
 - Like in `switch` the patterns are tried out sequentially. This means that the `Any` pattern should always
come last.

The release C# 8.0 included a new way to write switch expressions which yield a value. While this drastically reduced
the need for external libraries like this one for pattern matching, the language itself includes only the simplest
patterns. This library lets the user define arbitrary patterns, which makes this library more powerful than the switch
expressions.

Here's what the equivalent switch expression looks like in C# 8.0:

```
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

OK, this is much shorter and cleaner than the previous two examples. But this library shines when the patterns are
more complex. C# allows only constant patterns, type patterns, and `when` expressions. This library allows anything
you can think about.

## Performance

Versions 1.x used the DLR to provide type-safe pattern matching on any types
without having to remember all those types. Thus the performance was _much_
worse than C#'s switch statements/expressions (even though I didn't perform
any benchmarks).

Versions 2+ of this library don't use the DLR anymore - I've found a better way
to do this, and frankly, I'm amazed I didn't think of this way before. So I'm
guessing the performance of the new versions must be much better than versions 1.x.
Maybe, I'll even implement some benchmarks in the future to compare the performance
of this library in comparison to the switch statements/expressions.

Next article: [Match results](results.md).
