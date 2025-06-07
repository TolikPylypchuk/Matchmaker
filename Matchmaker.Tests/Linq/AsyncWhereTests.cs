namespace Matchmaker.Linq;

public class AsyncWhereTests
{
    [Property(DisplayName = "Where pattern should never return null")]
    public Property WherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property(DisplayName = "Where pattern with description should never return null")]
    public Property WherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property(DisplayName = "Where pattern should match the same as pattern and predicate")]
    public async Task<Property> WherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input) =>
        ((await pattern.Where(predicate).MatchAsync(input)).IsSuccessful ==
                   ((await pattern.MatchAsync(input)).IsSuccessful && predicate(input)))
               .ToProperty();

    [Property(DisplayName = "Where pattern should have same result as pattern when successful")]
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

    [Property(DisplayName = "Where pattern with description should match the same as pattern and predicate")]
    public async Task<Property> WherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description) =>
        ((await pattern.Where(predicate, description.Get).MatchAsync(input)).IsSuccessful ==
            ((await pattern.MatchAsync(input)).IsSuccessful && predicate(input)))
            .ToProperty();

    [Property(DisplayName = "Where pattern with description should have the same result as pattern when successful")]
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

    [Property(DisplayName = "Where pattern should have the same description as pattern")]
    public Property WherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Where pattern with description should have the specified description")]
    public Property WherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Where pattern should throw if pattern is null")]
    public void WherePatternShouldThrowIfPatternIsNull(Func<string, bool> predicate) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Where(predicate));

    [Property(DisplayName = "Where pattern with description should throw if pattern is null")]
    public void WherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Where(predicate, description.Get));

    [Property(DisplayName = "Where pattern should throw if predicate is null")]
    public void WherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where((Func<string, bool>)null));

    [Property(DisplayName = "Where pattern with description should throw if predicate is null")]
    public void WherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where((Func<string, bool>)null, description.Get));

    [Property(DisplayName = "Where pattern with description should throw if description is null")]
    public void WherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where(predicate, null));

    [Property(DisplayName = "Async Where pattern should never return null")]
    public Property AsyncWherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property(DisplayName = "Async Where pattern with description should never return null")]
    public Property AsyncWherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property(DisplayName = "Async Where pattern should match the same as pattern and predicate")]
    public async Task<Property> AsyncWherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input) =>
        ((await pattern.Where(predicate).MatchAsync(input)).IsSuccessful ==
                ((await pattern.MatchAsync(input)).IsSuccessful && await predicate(input)))
            .ToProperty();

    [Property(DisplayName = "Async Where pattern should have same result as pattern when successful")]
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

    [Property(DisplayName = "Async Where pattern with description should match the same as pattern and predicate")]
    public async Task<Property> AsyncWherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input,
        NonNull<string> description) =>
        ((await pattern.Where(predicate, description.Get).MatchAsync(input)).IsSuccessful ==
                ((await pattern.MatchAsync(input)).IsSuccessful && await predicate(input)))
            .ToProperty();

    [Property(DisplayName = "Async Where pattern with description should have the same result as pattern when successful")]
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

    [Property(DisplayName = "Async Where pattern should have the same description as pattern")]
    public Property AsyncWherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Async Where pattern with description should have the specified description")]
    public Property AsyncWherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Async Where pattern should throw if pattern is null")]
    public void AsyncWherePatternShouldThrowIfPatternIsNull(Func<string, Task<bool>> predicate) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Where(predicate));

    [Property(DisplayName = "Async Where pattern with description should throw if pattern is null")]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Where(predicate, description.Get));

    [Property(DisplayName = "Async Where pattern should throw if predicate is null")]
    public void AsyncWherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where((Func<string, Task<bool>>)null));

    [Property(DisplayName = "Async Where pattern with description should throw if predicate is null")]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where((Func<string, Task<bool>>)null, description.Get));

    [Property(DisplayName = "Async Where pattern with description should throw if description is null")]
    public void AsyncWherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Where(predicate, null));
}
