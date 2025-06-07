namespace Matchmaker.Linq;

public class ComposeXorTests
{
    [Property(DisplayName = "Compose Xor pattern should never return null")]
    public Property ComposeXorPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor) != null).ToProperty();

    [Property(DisplayName = "Compose Xor pattern with description should never return null")]
    public Property ComposeXorPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose Xor pattern should be the same as exclusive either pattern")]
    public Property ComposeXorPatternShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Xor).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Compose Xor pattern with description should be the same as exclusive either pattern")]
    public Property ComposeXorPatternWithDescriptionShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have correct description")]
    public Property ComposeXorPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
                String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description if left pattern has empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Compose(pattern, PatternComposition.Xor)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description if right pattern has empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.Xor)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty)
            .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.Xor)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have the specified description")]
    public Property ComposeXorPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should throw if left pattern is null")]
    public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Xor));

    [Property(DisplayName = "Compose Xor pattern with description should throw if left pattern is null")]
    public void ComposeXorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Xor, description.Get));

    [Property(DisplayName = "Compose Xor pattern should throw if right pattern is null")]
    public void ComposeXorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Xor));

    [Property(DisplayName = "Compose Xor pattern with description should throw if right pattern is null")]
    public void ComposeXorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Xor, description.Get));

    [Property(DisplayName = "Compose Xor pattern should throw if description is null")]
    public void ComposeXorPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Compose(pattern2, PatternComposition.Xor, null));

    [Property(DisplayName = "Xor pattern should never return null")]
    public Property XorPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Xor(pattern2) != null).ToProperty();

    [Property(DisplayName = "Xor pattern with description should never return null")]
    public Property XorPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "Xor pattern should be the same as exclusive eihter pattern")]
    public Property XorPatternShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Xor(pattern2).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Xor pattern with description should be the same as exclusive either pattern")]
    public Property XorPatternWithDescriptionShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Xor(pattern2, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Xor pattern should have correct description")]
    public Property XorPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Xor(pattern2).Description ==
                String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if left pattern has empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if right pattern has empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Xor(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if both patterns have empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).Xor(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have the specified description")]
    public Property XorPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Xor pattern should throw if left pattern is null")]
    public void XorPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Xor(pattern));

    [Property(DisplayName = "Xor pattern with description should throw if left pattern is null")]
    public void XorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Xor(pattern, description.Get));

    [Property(DisplayName = "Xor pattern should throw if right pattern is null")]
    public void XorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Xor(null));

    [Property(DisplayName = "Xor pattern with description should throw if right pattern is null")]
    public void XorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Xor(null, description.Get));

    [Property(DisplayName = "Xor pattern should throw if description is null")]
    public void XorPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Xor(pattern2, null));
}
