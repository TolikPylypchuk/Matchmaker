namespace Matchmaker.Linq;

public class BindTests
{
    [Property(DisplayName = "Bind pattern should never return null")]
    public Property BindPatternShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder) =>
        (pattern.Bind(binder) != null).ToProperty();

    [Property(DisplayName = "Bind pattern with description should never return null")]
    public Property BindPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get) != null).ToProperty();

    [Property(DisplayName = "Bind pattern should match the same as binder result")]
    public Property BindPatternShouldMatchSameAsBinderResult(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
                pattern.Bind(binder).Match(x) == binder(result.Value).Match(x))
            .ToProperty();
    }

    [Property(DisplayName = "Bind pattern with description should match the same as binder result")]
    public Property BindPatternWithDescriptionShouldMatchSameAsBinderResult(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        string x,
        NonNull<string> description)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
            pattern.Bind(binder, description.Get).Match(x) == binder(result.Value).Match(x))
            .ToProperty();
    }

    [Property(DisplayName = "Bind pattern should have the same description as pattern")]
    public Property BindPatternShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder) =>
        (pattern.Bind(binder).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Bind pattern with description should have the specified description")]
    public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Bind pattern should throw if pattern is null")]
    public void BindPatternShouldThrowIfPatternIsNull(Func<string, IPattern<string, string>> binder) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Bind(binder));

    [Property(DisplayName = "Bind pattern with description should throw if pattern is null")]
    public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Bind(binder, description.Get));

    [Property(DisplayName = "Bind pattern should throw if binder is null")]
    public void BindPatternShouldThrowIfBinderIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Bind((Func<string, IPattern<string, int>>)null));

    [Property(DisplayName = "Bind pattern with description should throw if description is null")]
    public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Bind(binder, null));
}
