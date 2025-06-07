namespace Matchmaker.Linq;

public class AsyncSelectTests
{
    [Property(DisplayName = "Select pattern should never return null")]
    public Property SelectPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper) =>
        (pattern.Select(mapper) != null).ToProperty();

    [Property(DisplayName = "Select pattern with description should never return null")]
    public Property SelectPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get) != null).ToProperty();

    [Property(DisplayName = "Select pattern should match the same as pattern")]
    public async Task<Property> SelectPatternShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        ((await pattern.Select(mapper).MatchAsync(input)).IsSuccessful ==
            (await pattern.MatchAsync(input)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Select pattern should have mapped result when successful")]
    public async Task<Property> SelectPatternShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        (await (await pattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                (await pattern.Select(mapper).MatchAsync(input)).Value ==
                mapper((await pattern.MatchAsync(input)).Value)))
            .ToProperty();

    [Property(DisplayName = "Select pattern with description should match the same as pattern")]
    public async Task<Property> SelectPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        ((await pattern.Select(mapper, description.Get).MatchAsync(input)).IsSuccessful ==
            (await pattern.MatchAsync(input)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Select pattern with description should have mapped result when successful")]
    public async Task<Property> SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        (await (await pattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                (await pattern.Select(mapper, description.Get).MatchAsync(input)).Value ==
                mapper((await pattern.MatchAsync(input)).Value)))
            .ToProperty();

    [Property(DisplayName = "Select pattern should have the same description as pattern")]
    public Property SelectPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> mapper) =>
        (pattern.Select(mapper).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Select pattern with description should have the specified description")]
    public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Select pattern should throw if pattern is null")]
    public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Select(mapper));

    [Property(DisplayName = "Select pattern with description should throw if pattern is null")]
    public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, int> mapper,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Select(mapper, description.Get));

    [Property(DisplayName = "Select pattern should throw if mapper is null")]
    public void SelectPatternShouldThrowIfMapperIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select<string, string, int>(null));

    [Property(DisplayName = "Select pattern with description should throw if mapper is null")]
    public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select<string, string, int>(null, description.Get));

    [Property(DisplayName = "Select pattern with description should throw if description is null")]
    public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select(mapper, null));
}
