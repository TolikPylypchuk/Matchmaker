namespace Matchmaker.Linq;

public class SelectTests
{
    [Property(DisplayName = "Select pattern should never return null")]
    public Property SelectPatternShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper) =>
        (pattern.Select(mapper) != null).ToProperty();

    [Property(DisplayName = "Select pattern with description should never return null")]
    public Property SelectPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get) != null).ToProperty();

    [Property(DisplayName = "Select pattern should match the same as pattern")]
    public Property SelectPatternShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        (pattern.Select(mapper).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful).ToProperty();

    [Property(DisplayName = "Select pattern should have mapped result when successful")]
    public Property SelectPatternShouldHaveMappedResultWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        pattern.Match(input).IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper).Match(input).Value == mapper(pattern.Match(input).Value))
            .ToProperty();

    [Property(DisplayName = "Select pattern with description should match the same as pattern")]
    public Property SelectPatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Select pattern with description should have mapped result when successful")]
    public Property SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        pattern.Match(input).IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper, description.Get).Match(input).Value == mapper(pattern.Match(input).Value))
            .ToProperty();

    [Property(DisplayName = "Select pattern should have the same description as pattern")]
    public Property SelectPatternShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, bool> mapper) =>
        (pattern.Select(mapper).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Select pattern with description should have the specified description")]
    public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Select pattern should throw if pattern is null")]
    public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Select(mapper));

    [Property(DisplayName = "Select pattern with description should throw if pattern is null")]
    public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, int> mapper,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Select(mapper, description.Get));

    [Property(DisplayName = "Select pattern should throw if mapper is null")]
    public void SelectPatternShouldThrowIfMapperIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select<string, string, int>(null));

    [Property(DisplayName = "Select pattern with description should throw if mapper is null")]
    public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select<string, string, int>(null, description.Get));

    [Property(DisplayName = "Select pattern with description should throw if description is null")]
    public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Select(mapper, null));
}
