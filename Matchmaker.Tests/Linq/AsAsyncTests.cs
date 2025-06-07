namespace Matchmaker.Linq;

public class AsAsyncTests
{
    [Property]
    public async Task<Property> AsAsyncShouldMatchSameAsPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Match(x) == await pattern.AsAsync().MatchAsync(x)).ToProperty();

    [Property]
    public async Task<Property> AsAsyncWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        NonNull<string> description,
        string x) =>
        (pattern.Match(x) == await pattern.AsAsync(description.Get).MatchAsync(x)).ToProperty();

    [Property]
    public Property AsAsyncShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (pattern.AsAsync() != null).ToProperty();

    [Property]
    public Property AsAsyncWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.AsAsync(description.Get) != null).ToProperty();

    [Property]
    public Property AsAsyncShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern) =>
        (pattern.Description == pattern.AsAsync().Description).ToProperty();

    [Property]
    public Property AsAsyncWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.AsAsync(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void AsAsyncShouldThrowIfPatternIsNull()
    {
        var action = () => ((IPattern<string, string>)null).AsAsync();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsAsyncWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).AsAsync(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AsAsyncWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.AsAsync(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
