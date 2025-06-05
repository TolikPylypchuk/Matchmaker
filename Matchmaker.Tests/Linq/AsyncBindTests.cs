namespace Matchmaker.Linq;

public class AsyncBindTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldMatchSameAsBinderResult(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        string x)
    {
        var result = pattern.MatchAsync(x);
        return result.Result.IsSuccessful.ImpliesThat(() =>
                pattern.Bind(binder).MatchAsync(x).Result == binder(result.Result.Value).MatchAsync(x).Result)
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldMatchSameAsBinderResult(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        string x,
        NonNull<string> description)
    {
        var result = pattern.MatchAsync(x);
        return result.Result.IsSuccessful.ImpliesThat(() =>
            pattern.Bind(binder, description.Get).MatchAsync(x).Result ==
            binder(result.Result.Value).MatchAsync(x).Result)
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternShouldThrowIfPatternIsNull(Func<string, IAsyncPattern<string, string>> binder)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Bind(binder);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Bind(binder, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternShouldThrowIfBinderIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Bind((Func<string, IAsyncPattern<string, int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder)
    {
        var action = () => pattern.Bind(binder, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
