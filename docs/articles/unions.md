# Discriminated Unions

While discriminated unions (or sum types) are not directly supported in C#, they can be modelled using class
hierarchies. But in order for them be user-friendly, a way to process the values has to be implemented,
e.g. in the form of the Visitor pattern.

Or, pattern matching can be used instead.

Let's define a simple list, implemented as [cons cells](https://en.wikipedia.org/wiki/Cons). This list is not
generic for simplicity.

```
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

Now let's look what pattern matching on the list would look like. Let's create
a function which finds the sum of all items of the list.

```
public int Sum(ConsList list) =>
    Match.Create<ConsList, int>()
        .Case<ConsCell>(cell => cell.Head + Sum(cell.Tail))
        .Case<Empty>(_ => 0)
        .ExecuteOn(list);
```

`Case<TType>(...)` is the same as `Case(Pattern.Type<TInput, TType>(), ...)`.

Here is the equivalent function implemented using the `switch` statement (pre-C# 8):

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

As you can see, we have to throw an exception in the `switch` version, because C# can't know that `ConsCell`
and `Empty` are the only possible subclasses of `ConsList`. And for that reason, if we forget to define one
of the cases in `switch` or in a match, we'll get an exception. In F# a warning is issued when the match is
incomplete, but C# doesn't have the notion of complete or incomplete matches. Of course, this match will fail if the
provided list is `null`, but this can be handled using the `Null` pattern.

With C# 8 there's a better way to match on discriminated unions, but we still have to explicitly throw an exception
in the default case (which we know won't happen):

```
public int Sum(ConsList list) =>
    list switch
    {
        ConsCell cell => cell.Head + Sum(cell.Tail),
        Empty _ => 0,
        _ => throw new MatchException("This will never happen, but C# can't know that.");
    };
}
```
