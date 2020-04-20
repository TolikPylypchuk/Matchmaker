# Matchmaker

[![NuGet](https://img.shields.io/nuget/v/Matchmaker.svg)](https://www.nuget.org/packages/Matchmaker/)
[![Build status](https://ci.appveyor.com/api/projects/status/wptuo5d5mi4blss0?svg=true)](https://ci.appveyor.com/project/TolikPylypchuk/matchmaker)

A library which enables more powerful pattern matching than is currently available in the C#'s `switch`
statement/expression.

This library is a successor of [PatternMatching](https://github.com/TolikPylypchuk/PatternMatching).
Version 1.x can be found there. This repository contains version 2+.

## Installation

```
Install-Package Matchmaker -Version 2.0.0
```

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
in an object, and use it multiple times on different input values.

 - The default case is a pattern, just like any other. It's called `Any` and is always matched successfully.

 - Like in `switch` the patterns are tried out sequentially. This means that the `Any` pattern should always
come last.

The release C# 8.0 included a new way to write `switch` expressions which yield a value. While this drastically reduced
the need for external libraries like this one for pattern matching, the language itself includes only the simplest
patterns. This library lets the user define arbitrary patterns, which makes this library more powerful than the
`switch` expressions.

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

## More Info

If you want to learn how to use this library, you should read the
[documentation](https://tolikpylypchuk.github.io/Matchmaker/v2.0.0). The articles provide everything you need to know
to use this library.

If you need extensive information, go to the
[API reference](https://tolikpylypchuk.github.io/Matchmaker/v2.0.0/api/index.html).

If you need even more info about this library, you can go through the
[tests](https://github.com/TolikPylypchuk/Matchmaker/tree/v2.0.0/Matchmaker.Tests). They are property-based and as such
they describe every aspect of the classes and their members.

The documentation can be found here:

 - Version 2.0.0: https://tolikpylypchuk.github.io/Matchmaker/v2.0.0
 - Older versions: https://github.com/TolikPylypchuk/PatternMatching

## License

[MIT License](https://github.com/TolikPylypchuk/Matchmaker/blob/master/LICENSE)
