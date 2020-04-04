# Matchmaker

[![Build status](https://ci.appveyor.com/api/projects/status/wptuo5d5mi4blss0?svg=true)](https://ci.appveyor.com/project/TolikPylypchuk/matchmaker)

A library which enables more powerful pattern matching
than is currently available in the C#'s `switch` statement/expression.

This library is a successor of
[PatternMatching](https://github.com/TolikPylypchuk/PatternMatching).
Version 1.x can be found there. This repository contains version 2+.

## Installation

This library cannot be installed through NuGet until version 2.0 is released.
For now you can install the [older version](https://www.nuget.org/packages/CSharpStuff.PatternMatching/).

## A simple example

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

The release C# 8.0 included a new way to write switch expressions which yield a value.
While this drastically reduced the need for external libraries like this one
for pattern matching, the language itself includes only the simplest patterns.
This library lets the user define arbitrary patterns, which makes this library
more powerful than the switch expressions.

## More Info

The documentation can be found here:

 - Version 2.0.0: https://tolikpylypchuk.github.io/Matchmaker/v2.0.0

## License

[MIT License](https://github.com/TolikPylypchuk/Matchmaker/blob/master/LICENSE)
