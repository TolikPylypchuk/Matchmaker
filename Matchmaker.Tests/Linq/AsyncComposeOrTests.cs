namespace Matchmaker.Linq;

public class AsyncComposeOrTests
{
    [Property(DisplayName = "Compose Or pattern should never return null")]
    public Property ComposeOrPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Or) != null).ToProperty();

    [Property(DisplayName = "Compose Or pattern with description should never return null")]
    public Property ComposeOrPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose Or pattern should be the same as either pattern")]
    public async Task<Property> ComposeOrPatternShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Or).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern with description should be the same as either pattern")]
    public async Task<Property> ComposeOrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Or, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have correct description")]
    public Property ComposeOrPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.Or).Description ==
             String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description if left pattern has empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.Or).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description if right pattern has empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.Or)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.Or)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose Or pattern should have the specified description")]
    public Property ComposeOrPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should throw if left pattern is null")]
    public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Or));

    [Property(DisplayName = "Compose Or pattern with description should throw if left pattern is null")]
    public void ComposeOrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Or, description.Get));

    [Property(DisplayName = "Compose Or pattern should throw if right pattern is null")]
    public void ComposeOrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Or));

    [Property(DisplayName = "Compose Or pattern with description should throw if right pattern is null")]
    public void ComposeOrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Or, description.Get));

    [Property(DisplayName = "Compose Or pattern should throw if description is null")]
    public void ComposeOrPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Compose(pattern2, PatternComposition.Or, null));

    [Property(DisplayName = "Or pattern should never return null")]
    public Property OrPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Or(pattern2) != null).ToProperty();

    [Property(DisplayName = "Or pattern with description should never return null")]
    public Property OrPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "Or pattern should be the same as eihter pattern")]
    public async Task<Property> OrPatternShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Or(pattern2).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Or pattern with description should be the same as either pattern")]
    public async Task<Property> OrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Or(pattern2, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Or pattern should have correct description")]
    public Property OrPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Or(pattern2).Description ==
                String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if left pattern has empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if right pattern has empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Or(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if both patterns have empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).Or(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have the specified description")]
    public Property OrPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Or pattern should throw if left pattern is null")]
    public void OrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Or(pattern));

    [Property(DisplayName = "Or pattern with description should throw if left pattern is null")]
    public void OrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Or(pattern, description.Get));

    [Property(DisplayName = "Or pattern should throw if right pattern is null")]
    public void OrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Or(null));

    [Property(DisplayName = "Or pattern with description should throw if right pattern is null")]
    public void OrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Or(null, description.Get));

    [Property(DisplayName = "Or pattern should throw if description is null")]
    public void OrPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Or(pattern2, null));
}
