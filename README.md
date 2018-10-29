# PatternMatching

[![NuGet](https://img.shields.io/nuget/v/CSharpStuff.PatternMatching.svg)](https://www.nuget.org/packages/CSharpStuff.PatternMatching/)

A library which enables more powerful pattern matching
than is currently available in the `switch` statement.

## Installation

```
Install-Package CSharpStuff.PatternMatching -Version 1.1.0
```

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

## More Info

The documentation can be found here:

 - Version 1.1.0: https://tolikpylypchuk.github.io/PatternMatching/v1.1.0
 - Version 1.0.0: https://tolikpylypchuk.github.io/PatternMatching/v1.0.0

## License

[MIT License](https://github.com/TolikPylypchuk/PatternMatching/blob/master/LICENSE)
