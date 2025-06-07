namespace Matchmaker.Linq;

public class CachedTests
{
    [Property(DisplayName = "Cached pattern should never return null")]
    public Property CachedPatternShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (pattern.Cached() != null).ToProperty();

    [Property(DisplayName = "Cached pattern with description should never return null")]
    public Property CachedPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get) != null).ToProperty();

    [Property(DisplayName = "Cached pattern should match the same as pattern")]
    public Property CachedPatternShouldMatchSameAsPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Cached().Match(x) == pattern.Match(x)).ToProperty();

    [Property(DisplayName = "Cached pattern with description should match the same as pattern")]
    public Property CachedPatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        string x,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Match(x) == pattern.Match(x)).ToProperty();

    [Property(DisplayName = "Cached pattern should be cached")]
    public Property CachedPatternShouldBeCached(string x)
    {
        int count = 0;

        var pattern = Pattern.CreatePattern<string>(v =>
        {
            count++;
            return true;
        }).Cached();

        pattern.Match(x);
        pattern.Match(x);

        return (count == 1).ToProperty();
    }

    [Property(DisplayName = "Cached pattern with description should be cached")]
    public Property CachedPatternWithDescriptionShouldBeCached(string x, NonNull<string> description)
    {
        int count = 0;

        var pattern = Pattern.CreatePattern<string>(v =>
        {
            count++;
            return true;
        }).Cached(description.Get);

        pattern.Match(x);
        pattern.Match(x);

        return (count == 1).ToProperty();
    }

    [Property(DisplayName = "Cached pattern should have the same description as pattern")]
    public Property CachedPatternShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern) =>
        (pattern.Cached().Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Cached pattern with description should have the specified description")]
    public Property CachedPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Cached pattern should throw if pattern is null")]
    public void CachedPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IPattern<string, string>)null).Cached();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cached pattern with description should throw if pattern is null")]
    public void CachedPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Cached(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cached pattern with description should throw if description is null")]
    public void CachedPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Cached(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
