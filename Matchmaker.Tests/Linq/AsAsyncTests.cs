namespace Matchmaker.Linq;

public class AsAsyncTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsAsyncShouldMatchSameAsPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Match(x) == pattern.AsAsync().MatchAsync(x).Result).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsAsyncWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        NonNull<string> description,
        string x) =>
        (pattern.Match(x) == pattern.AsAsync(description.Get).MatchAsync(x).Result).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsAsyncShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (pattern.AsAsync() != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsAsyncWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.AsAsync(description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsAsyncShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern) =>
        (pattern.Description == pattern.AsAsync().Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
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

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsAsyncWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.AsAsync(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
