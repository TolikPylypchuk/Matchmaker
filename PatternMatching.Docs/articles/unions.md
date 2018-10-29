# Discriminated Unions

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

    throw new MatchException("This will never happen, but C# can't know that.");
}
```

*Note*: The declaration of sum must be split from its initialization, because
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
complete or incomplete matches. Of course, this match will fail if the
provided list is `null`, but this can be handled using the `Null` pattern.
