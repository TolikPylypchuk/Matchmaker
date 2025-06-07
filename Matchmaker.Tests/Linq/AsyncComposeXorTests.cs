namespace Matchmaker.Linq;

public class AsyncComposeXorTests
{
    [Property(DisplayName = "Compose Xor pattern should never return null")]
    public Property ComposeXorPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor) != null).ToProperty();

    [Property(DisplayName = "Compose Xor pattern with description should never return null")]
    public Property ComposeXorPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose Xor pattern should be the same as exclusive either pattern")]
    public async Task<Property> ComposeXorPatternShouldBeSameAsExclusiveEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful ^ (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Xor).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern with description should be the same as exclusive either pattern")]
    public async Task<Property> ComposeXorPatternWithDescriptionShouldBeSameAsExclusiveEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful ^ (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have correct description")]
    public Property ComposeXorPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
             String.Format(AsyncPattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description if left pattern has empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.Xor).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description if right pattern has empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfRightdPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.Xor)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.Xor)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose Xor pattern should have the specified description")]
    public Property ComposeXorPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose Xor pattern should throw if left pattern is null")]
    public void ComposeXorPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Xor));

    [Property(DisplayName = "Compose Xor pattern with description should throw if left pattern is null")]
    public void ComposeXorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Xor, description.Get));

    [Property(DisplayName = "Compose Xor pattern should throw if right pattern is null")]
    public void ComposeXorPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Xor));

    [Property(DisplayName = "Compose Xor pattern with description should throw if right pattern is null")]
    public void ComposeXorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Compose(null, PatternComposition.Xor, description.Get));

    [Property(DisplayName = "Compose Xor pattern should throw if description is null")]
    public void ComposeXorPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Compose(pattern2, PatternComposition.Xor, null));

    [Property(DisplayName = "Xor pattern should never return null")]
    public Property XorPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Xor(pattern2) != null).ToProperty();

    [Property(DisplayName = "Xor pattern with description should never return null")]
    public Property XorPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "Xor pattern should be the same as exclusive eihter pattern")]
    public async Task<Property> XorPatternShouldBeSameAsExclusiveEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful ^ (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Xor(pattern2).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Xor pattern with description should be the same as exclusive either pattern")]
    public async Task<Property> XorPatternWithDescriptionShouldBeSameAsExclusiveEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful ^ (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Xor(pattern2, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Xor pattern should have correct description")]
    public Property XorPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Xor(pattern2).Description ==
                String.Format(AsyncPattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if left pattern has empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if right pattern has empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Xor(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have empty description if both patterns have empty description")]
    public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).Xor(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Xor pattern should have the specified description")]
    public Property XorPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Xor pattern should throw if left pattern is null")]
    public void XorPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Xor(pattern));

    [Property(DisplayName = "Xor pattern with description should throw if left pattern is null")]
    public void XorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IAsyncPattern<string, string>)null).Xor(pattern, description.Get));

    [Property(DisplayName = "Xor pattern should throw if right pattern is null")]
    public void XorPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Xor(null));

    [Property(DisplayName = "Xor pattern with description should throw if right pattern is null")]
    public void XorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Xor(null, description.Get));

    [Property(DisplayName = "Xor pattern should throw if description is null")]
    public void XorPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        Assert.Throws<ArgumentNullException>(() => pattern1.Xor(pattern2, null));
}
