namespace Matchmaker.Linq;

public class AsyncBindTests
{
    [Property]
    public Property BindPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder) != null).ToProperty();

    [Property]
    public Property BindPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get) != null).ToProperty();

    [Property]
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

    [Property]
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

    [Property]
    public Property BindPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder) =>
        (pattern.Bind(binder).Description == pattern.Description).ToProperty();

    [Property]
    public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void BindPatternShouldThrowIfPatternIsNull(Func<string, IAsyncPattern<string, string>> binder)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Bind(binder);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, IAsyncPattern<string, string>> binder,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Bind(binder, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void BindPatternShouldThrowIfBinderIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Bind((Func<string, IAsyncPattern<string, int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, IAsyncPattern<string, string>> binder)
    {
        var action = () => pattern.Bind(binder, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
