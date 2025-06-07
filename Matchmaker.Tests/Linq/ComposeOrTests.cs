namespace Matchmaker.Linq;

public class ComposeOrTests
{
    [Property(DisplayName = "Compose Or pattern should never return null")]
    public Property ComposeOrPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Or) != null).ToProperty();

    [Property(DisplayName = "Compose Or pattern with description should never return null")]
    public Property ComposeOrPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get) != null).ToProperty();

    [Property(DisplayName = "Compose Or pattern should be the same as either pattern")]
    public Property ComposeOrPatternShouldBeSameAsEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Or).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern with description should be the same as either pattern")]
    public Property ComposeOrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Or, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have correct description")]
    public Property ComposeOrPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.Or).Description ==
                String.Format(Pattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description if left pattern has empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Compose(pattern, PatternComposition.Or)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description if right pattern has empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.Or)
            .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should have empty description " +
        "if both patterns have empty description")]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty)
            .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.Or)
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Compose Or pattern should have the specified description")]
    public Property ComposeOrPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Compose Or pattern should throw if left pattern is null")]
    public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Compose Or pattern with description should throw if left pattern is null")]
    public void ComposeOrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Compose(
            pattern, PatternComposition.Or, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Compose Or pattern should throw if right pattern is null")]
    public void ComposeOrPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Compose(null, PatternComposition.Or);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Compose Or pattern with description should throw if right pattern is null")]
    public void ComposeOrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Compose(null, PatternComposition.Or, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Compose Or pattern should throw if description is null")]
    public void ComposeOrPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2)
    {
        var action = () => pattern1.Compose(pattern2, PatternComposition.Or, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Or pattern should never return null")]
    public Property OrPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Or(pattern2) != null).ToProperty();

    [Property(DisplayName = "Or pattern with description should never return null")]
    public Property OrPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get) != null).ToProperty();

    [Property(DisplayName = "Or pattern should be the same as eihter pattern")]
    public Property OrPatternShouldBeSameAsEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
            pattern1.Or(pattern2).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Or pattern with description should be the same as either pattern")]
    public Property OrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
            pattern1.Or(pattern2, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Or pattern should have correct description")]
    public Property OrPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Or(pattern2).Description ==
                String.Format(Pattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if left pattern has empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfLeftPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if right pattern has empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfRightPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Or(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have empty description if both patterns have empty description")]
    public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).Or(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Or pattern should have the specified description")]
    public Property OrPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Or pattern should throw if left pattern is null")]
    public void OrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => ((IPattern<string, string>)null).Or(pattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Or pattern with description should throw if left pattern is null")]
    public void OrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Or(pattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Or pattern should throw if right pattern is null")]
    public void OrPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Or(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Or pattern with description should throw if right pattern is null")]
    public void OrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Or(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Or pattern should throw if description is null")]
    public void OrPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2)
    {
        var action = () => pattern1.Or(pattern2, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
