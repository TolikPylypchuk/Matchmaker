namespace Matchmaker.Linq;

public class ComposeAndTests
{
    [Property(DisplayName = "Compose And pattern should never return null")]
    public Property ComposeAndPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.And) != null).ToProperty();

    [Property(DisplayName = "Compose And pattern with description should never return null")]
    public Property ComposeAndPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose And pattern should be the same as both patterns")]
    public Property ComposeAndPatternShouldBeSameAsBothPatterns(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.And).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern with description should be the same as both patterns")]
    public Property ComposeAndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.And, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have correct description")]
    public Property ComposeAndPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.And).Description ==
             String.Format(Pattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description if left pattern has empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.And).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description if right pattern has empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.And)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty)
            .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.And)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose And pattern should have the specified description")]
    public Property ComposeAndPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should throw if left pattern is null")]
    public void ComposeAndPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, string>)null).Compose(pattern, PatternComposition.And));

    [Property(DisplayName = "Compose And pattern with description should throw if left pattern is null")]
    public void ComposeAndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, string>)null).Compose(pattern, PatternComposition.And, description.Get));

    [Property(DisplayName = "Compose And pattern should throw if right pattern is null")]
    public void ComposeAndPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.And));

    [Property(DisplayName = "Compose And pattern with description should throw if right pattern is null")]
    public void ComposeAndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.And, description.Get));

    [Property(DisplayName = "Compose And pattern should throw if description is null")]
    public void ComposeAndPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Compose(pattern2, PatternComposition.And, null));

    [Property(DisplayName = "And pattern should never return null")]
    public Property AndPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.And(pattern2) != null).ToProperty();

    [Property(DisplayName = "And pattern with description should never return null")]
    public Property AndPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "And pattern should be the same as both patterns")]
    public Property AndPatternShouldBeSameAsBothPatterns(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
            pattern1.And(pattern2).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "And pattern with description should be the same as both patterns")]
    public Property AndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
            pattern1.And(pattern2, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "And pattern should have correct description")]
    public Property AndPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.And(pattern2).Description ==
                String.Format(Pattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "And pattern should have empty description if left pattern has empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have empty description if right pattern has empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.And(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have empty description if both patterns have empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).And(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have the specified description")]
    public Property AndPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "And pattern should throw if left pattern is null")]
    public void AndPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).And(pattern));

    [Property(DisplayName = "And pattern with description should throw if left pattern is null")]
    public void AndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).And(pattern, description.Get));

    [Property(DisplayName = "And pattern should throw if right pattern is null")]
    public void AndPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.And(null));

    [Property(DisplayName = "And pattern with description should throw if right pattern is null")]
    public void AndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.And(null, description.Get));

    [Property(DisplayName = "And pattern should throw if description is null")]
    public void AndPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.And(pattern2, null));
}
