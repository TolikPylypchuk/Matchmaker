namespace Matchmaker.Linq;

public class CachedTests
{
    [Property]
    public Property CachedPatternShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (pattern.Cached() != null).ToProperty();

    [Property]
    public Property CachedPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get) != null).ToProperty();

    [Property]
    public Property CachedPatternShouldMatchSameAsPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Cached().Match(x) == pattern.Match(x)).ToProperty();

    [Property]
    public Property CachedPatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        string x,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Match(x) == pattern.Match(x)).ToProperty();

    [Property]
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

    [Property]
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

    [Property]
    public Property CachedPatternShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern) =>
        (pattern.Cached().Description == pattern.Description).ToProperty();

    [Property]
    public Property CachedPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void CachedPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IPattern<string, string>)null).Cached();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void CachedPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Cached(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void CachedPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Cached(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
