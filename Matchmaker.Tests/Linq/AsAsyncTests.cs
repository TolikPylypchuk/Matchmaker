namespace Matchmaker.Linq;

public class AsAsyncTests
{
    [Property(DisplayName = "AsAsync should match the same as pattern")]
    public async Task<Property> AsAsyncShouldMatchSameAsPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Match(x) == await pattern.AsAsync().MatchAsync(x)).ToProperty();

    [Property]
    public async Task<Property> AsAsyncWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        NonNull<string> description,
        string x) =>
        (pattern.Match(x) == await pattern.AsAsync(description.Get).MatchAsync(x)).ToProperty();

    [Property(DisplayName = "AsAsync should never return null")]
    public Property AsAsyncShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (pattern.AsAsync() != null).ToProperty();

    [Property(DisplayName = "AsAsync with description should never return null")]
    public Property AsAsyncWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.AsAsync(description.Get) != null).ToProperty();

    [Property(DisplayName = "AsAsync should have the same description as pattern")]
    public Property AsAsyncShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern) =>
        (pattern.Description == pattern.AsAsync().Description).ToProperty();

    [Property(DisplayName = "AsAsync with description should have the specified description")]
    public Property AsAsyncWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.AsAsync(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "AsAsync should throw if pattern is null")]
    public void AsAsyncShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).AsAsync());

    [Property(DisplayName = "AsAsync with description should throw if pattern is null")]
    public void AsAsyncWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).AsAsync(description.Get));

    [Property(DisplayName = "AsAsync with description should throw if description is null")]
    public void AsAsyncWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.AsAsync(null));
}
