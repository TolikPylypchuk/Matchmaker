namespace Matchmaker.Linq;

public class AsyncComposeAndTests
{
    [Property(DisplayName = "Compose And pattern should never return null")]
    public Property ComposeAndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.And) != null).ToProperty();

    [Property(DisplayName = "Compose And pattern with description should never return null")]
    public Property ComposeAndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose And pattern should be the same as both patterns")]
    public async Task<Property> ComposeAndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.And).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern with description should be the same as both patterns")]
    public async Task<Property> ComposeAndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.And, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have correct description")]
    public Property ComposeAndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.And).Description ==
             String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description if left pattern has empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.And).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description if right pattern has empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.And)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.And)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose And pattern should have the specified description")]
    public Property ComposeAndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose And pattern should throw if left pattern is null")]
    public void ComposeAndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.And));

    [Property(DisplayName = "Compose And pattern with description should throw if left pattern is null")]
    public void ComposeAndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.And, description.Get));

    [Property(DisplayName = "Compose And pattern should throw if right pattern is null")]
    public void ComposeAndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.And));

    [Property(DisplayName = "Compose And pattern with description should throw if right pattern is null")]
    public void ComposeAndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.And, description.Get));

    [Property(DisplayName = "Compose And pattern should throw if description is null")]
    public void ComposeAndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Compose(pattern2, PatternComposition.And, null));

    [Property(DisplayName = "And pattern should never return null")]
    public Property AndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.And(pattern2) != null).ToProperty();

    [Property(DisplayName = "And pattern with description should never return null")]
    public Property AndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "And pattern should be the same as both patterns")]
    public async Task<Property> AndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.And(pattern2).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "And pattern with description should be the same as both patterns")]
    public async Task<Property> AndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.And(pattern2, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "And pattern should have correct description")]
    public Property AndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.And(pattern2).Description ==
                String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "And pattern should have empty description if left pattern has empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have empty description if right pattern has empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.And(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have empty description if both patterns have empty description")]
    public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).And(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "And pattern should have the specified description")]
    public Property AndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "And pattern should throw if left pattern is null")]
    public void AndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).And(pattern));

    [Property(DisplayName = "And pattern with description should throw if left pattern is null")]
    public void AndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).And(pattern, description.Get));

    [Property(DisplayName = "And pattern should throw if right pattern is null")]
    public void AndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.And(null));

    [Property(DisplayName = "And pattern with description should throw if right pattern is null")]
    public void AndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.And(null, description.Get));

    [Property(DisplayName = "And pattern should throw if description is null")]
    public void AndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.And(pattern2, null));
}
