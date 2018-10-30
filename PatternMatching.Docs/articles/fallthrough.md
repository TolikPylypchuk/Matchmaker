# Matching with Fallthrough

C and C++ support fallthrough in `switch` expressions.
So does this library, although it works differently here.
If fallthrough is enabled, then after successfully matching a
pattern, a match expression will fall through to the
_next successful_ pattern.

## Match Statements with Fallthrough

Let's start with match statements. The `Case` methods are overloaded
to accept a boolean value which indicates the fallthrough behaviour.
If fallthrough is enabled for a pattern, then the statement will
continue searching for the next successful pattern.

Notice, that fallthrough is configured for each pattern individually.
This means that when a pattern with no fallthrough is matched successfully,
the statement stops searching.

The default fallthrough value can be specified in the `Match.Create` method.

You should bear in mind that just adding patterns with fallthrough will
not force the match statement to actually take them into account.
In order to match with fallthrough, you should use the `ExecuteWithFallthrough`
(or `ExecuteStrictWithFallthrough`). The reason the fallthrough behaviour
is implemented in different methods is that they return different results.
`ExecuteOn` returns a boolean value which denotes whether a match was successful.
`ExecuteStrict` doesn't return anything and throws an exception if the match wasn't
succcessful. But both `ExecuteWithFallthrough` and `ExecuteStrictWithFallthrough` return
an int which denotes how many patterns were matched successfully.

## Nondeterministic Matching

What about match expressions? If they can also be executed with fallthrough,
then what should be the return value? There are at least two options:

1. The result of the last successful pattern match is the result of the expression.
I've decided against it because some results will be ignored and the order of the
patterns will matter significantly. Although there would be one upside - it's
consistent with multicasting non-void delegates.

2. The result of every successful pattern is the result of the expression. This
means that the expression actually returns a _list_ of results. This is the
option I chose, because it would allow for nondeterministic matching and subsequent
use of the nondeterministic result. I have no idea whether it's actually useful,
but it's an interesting idea nonetheless.

Just like for match statemens, the `Case` methods and the `Match.Create` method are
overloaded to specify the fallthrough behaviour. It is also configured for each
pattern individually.

In order to match with fallthrough, you should use the `ExecuteWithFallthrough`
(or `ExecuteNonStrictWithFallthrough`). These methods return a `Lst` of results
(an immutable list type from [language-ext](https://github.com/louthy/language-ext)).
A non-strict version will return an empty list if no patterns were matched successfully.
A strict version will throw an exception in that case.

## An Example

Here's the implementation of the famous fizz-buzz program which uses nondeterministic matching:

```
SimplePattern<int> divisibleBy(int n) => new SimplePattern<int>(input => input % n == 0);

var result = Enumerable.Range(0, 15)
    .Select(Match.Create<int, string>(fallthroughByDefault: true)
        .Case(divisibleBy(3), _ => "Fizz")
        .Case(divisibleBy(5), _ => "Buzz")
        .Case(Not(divisibleBy(3) | divisibleBy(5)), n => n.ToString())
        .ToFunctionWithFallthrough())
    .Select(items => items.Aggregate(String.Concat));

// The result is List("FizzBuzz", "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz");
```
