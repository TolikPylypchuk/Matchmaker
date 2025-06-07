namespace Matchmaker.Linq;

public class AsyncBindTests
{
    [Property(DisplayName = "Bind pattern should never return null")]
    public Property BindPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder) != null).ToProperty();

    [Property(DisplayName = "Bind pattern with description should never return null")]
    public Property BindPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get) != null).ToProperty();

    [Property(DisplayName = "Bind pattern should match the same as binder result")]
    public async Task<Property> BindPatternShouldMatchSameAsBinderResult(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        string x)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Bind(binder).MatchAsync(x) == await binder(result.Value).MatchAsync(x)))
            .ToProperty();
    }

    [Property(DisplayName = "Bind pattern with description should match the same as binder result")]
    public async Task<Property> BindPatternWithDescriptionShouldMatchSameAsBinderResult(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        string x,
        NonNull<string> description)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Bind(binder, description.Get).MatchAsync(x) == await binder(result.Value).MatchAsync(x)))
            .ToProperty();
    }

    [Property(DisplayName = "Bind pattern should have the same description as pattern")]
    public Property BindPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Bind pattern with description should have the specified description")]
    public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Bind pattern should throw if pattern is null")]
    public void BindPatternShouldThrowIfPatternIsNull(Func<string, IAsyncPattern<string, string>> binder) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Bind(binder));

    [Property(DisplayName = "Bind pattern with description should throw if pattern is null")]
    public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Bind(binder, description.Get));

    [Property(DisplayName = "Bind pattern should throw if binder is null")]
    public void BindPatternShouldThrowIfBinderIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Bind((Func<string, IAsyncPattern<string, int>>)null));

    [Property(DisplayName = "Bind pattern with description should throw if description is null")]
    public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Bind(binder, null));
}
