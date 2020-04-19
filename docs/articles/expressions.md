## Match Expressions

The second central idea is the match expression itself. It is represented by two classes: `Match<TInput, TOutput>`
and `Match<TInput>`. The difference between them is that the former represents a match expression which yields
a result, and the latter represents a match expression which doesn't yield a result (also known as match statement).

## Creating Match Expressions

A match expression can be created using the `Create` methods of the static class `Match`.

## Using Match Expressions

The `Match` classes include `Case` methods which are used to add a pattern and a function which is executed if
the match is successful. Match expressions are immutable - `Case` methods return new match expressions, they
do not affect the ones on which they are called.

To execute a match expression, the `ExecuteOn` method is used. It takes the input value to match. There are two modes
of execution in match expressions: strict and non-strict. The strict mode throws an exception and the non-strict doesn't.

In the `Match<TInput, TOutput>` class the `ExecuteOn` method returns the result of the match, or throws a
`MatchException` if no successful match was found. `Match<TInput, TOutput>` also contains the `ExecuteNonStrict`
method which executes the match expression in the non-strict mode. It returns `MatchResult<TOutput>`, because
the result might not be present.

In the `Match<TInput>` class the `ExecuteOn` method doesn't return anyhting, and also throws the `MatchException`
if the match wasn't successful. This class also contains the `ExecuteNonStrict` method - it returns a boolean value
which indicates whether the match was successful, and doesn't throw an exception if it wasn't.

The `ToFunction` methods and their variations are also available. They return a function which, when called,
will execute the match expression.
