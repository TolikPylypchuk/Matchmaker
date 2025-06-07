namespace Matchmaker.Linq;

public class AsyncWhereTests
{
    [Property]
    public Property WherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property]
    public Property WherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> WherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input) =>
        ((await pattern.Where(predicate).MatchAsync(input)).IsSuccessful ==
                   ((await pattern.MatchAsync(input)).IsSuccessful && predicate(input)))
               .ToProperty();

    [Property]
    public async Task<Property> WherePatternShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input)
    {
        var wherePattern = pattern.Where(predicate);
        return (await (await wherePattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                await wherePattern.MatchAsync(input) == await pattern.MatchAsync(input)))
            .ToProperty();
    }

    [Property]
    public async Task<Property> WherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description) =>
        ((await pattern.Where(predicate, description.Get).MatchAsync(input)).IsSuccessful ==
            ((await pattern.MatchAsync(input)).IsSuccessful && predicate(input)))
            .ToProperty();

    [Property]
    public async Task<Property> WherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description)
    {
        var wherePattern = pattern.Where(predicate, description.Get);
        return (await (await wherePattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                await wherePattern.MatchAsync(input) == await pattern.MatchAsync(input)))
            .ToProperty();
    }

    [Property]
    public Property WherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property]
    public Property WherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void WherePatternShouldThrowIfPatternIsNull(Func<string, bool> predicate)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void WherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, bool> predicate,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void WherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Where((Func<string, bool>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void WherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Where((Func<string, bool>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void WherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate)
    {
        var action = () => pattern.Where(predicate, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property AsyncWherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property]
    public Property AsyncWherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> AsyncWherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input) =>
        ((await pattern.Where(predicate).MatchAsync(input)).IsSuccessful ==
                ((await pattern.MatchAsync(input)).IsSuccessful && await predicate(input)))
            .ToProperty();

    [Property]
    public async Task<Property> AsyncWherePatternShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input)
    {
        var wherePattern = pattern.Where(predicate);
        return (await (await wherePattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                await wherePattern.MatchAsync(input) == await pattern.MatchAsync(input)))
            .ToProperty();
    }

    [Property]
    public async Task<Property> AsyncWherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input,
        NonNull<string> description) =>
        ((await pattern.Where(predicate, description.Get).MatchAsync(input)).IsSuccessful ==
                ((await pattern.MatchAsync(input)).IsSuccessful && await predicate(input)))
            .ToProperty();

    [Property]
    public async Task<Property> AsyncWherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input,
        NonNull<string> description)
    {
        var wherePattern = pattern.Where(predicate, description.Get);
        return (await (await wherePattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                await wherePattern.MatchAsync(input) == await pattern.MatchAsync(input)))
            .ToProperty();
    }

    [Property]
    public Property AsyncWherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property]
    public Property AsyncWherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void AsyncWherePatternShouldThrowIfPatternIsNull(Func<string, Task<bool>> predicate)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsyncWherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Where((Func<string, Task<bool>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Where((Func<string, Task<bool>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsyncWherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate)
    {
        var action = () => pattern.Where(predicate, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
