namespace Matchmaker.Linq;

public class AsyncCachedTests
{
    [Property(DisplayName = "Cached pattern should never return null")]
    public Property CachedPatternShouldNeverReturnNull(IAsyncPattern<string, string> pattern) =>
        (pattern.Cached() != null).ToProperty();

    [Property(DisplayName = "Cached pattern with description should never return null")]
    public Property CachedPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get) != null).ToProperty();

    [Property(DisplayName = "Cached pattern should match the same as pattern")]
    public async Task<Property> CachedPatternShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern, string x) =>
        (await pattern.Cached().MatchAsync(x) == await pattern.MatchAsync(x)).ToProperty();

    [Property(DisplayName = "Cached pattern with description should match the same as pattern")]
    public async Task<Property> CachedPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        string x,
        NonNull<string> description) =>
        (await pattern.Cached(description.Get).MatchAsync(x) == await pattern.MatchAsync(x)).ToProperty();

    [Property(DisplayName = "Cached pattern should be cached")]
    public async Task<Property> CachedPatternShouldBeCached(string x)
    {
        int count = 0;

        var pattern = AsyncPattern.CreatePattern<string>(v =>
        {
            count++;
            return Task.FromResult(true);
        }).Cached();

        await pattern.MatchAsync(x);
        await pattern.MatchAsync(x);

        return (count == 1).ToProperty();
    }

    [Property(DisplayName = "Cached pattern with description should be cached")]
    public async Task<Property> CachedPatternWithDescriptionShouldBeCached(string x, NonNull<string> description)
    {
        int count = 0;

        var pattern = AsyncPattern.CreatePattern<string>(v =>
        {
            count++;
            return Task.FromResult(true);
        }).Cached(description.Get);

        await pattern.MatchAsync(x);
        await pattern.MatchAsync(x);

        return (count == 1).ToProperty();
    }

    [Property(DisplayName = "Cached pattern should have the same description as pattern")]
    public Property CachedPatternShouldHaveSameDescriptionAsPattern(IAsyncPattern<string, string> pattern) =>
        (pattern.Cached().Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Cached pattern with description should have the specified description")]
    public Property CachedPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Cached pattern should throw if pattern is null")]
    public void CachedPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IAsyncPattern<string, string>)null).Cached();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cached pattern with description should throw if pattern is null")]
    public void CachedPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Cached(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cached pattern with description should throw if description is null")]
    public void CachedPatternWithDescriptionShouldThrowIfDescriptionIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Cached(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
