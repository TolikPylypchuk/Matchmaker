namespace Matchmaker.Linq;

public class AsyncComposeOrTests
{
    [Property]
    public Property ComposeOrPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Or) != null).ToProperty();

    [Property]
    public Property ComposeOrPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> ComposeOrPatternShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Or).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> ComposeOrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.Or, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property ComposeOrPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.Or).Description ==
             String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.Or).Description.Length == 0)
            .ToProperty();

    [Property]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.Or)
            .Description.Length == 0)
            .ToProperty();

    [Property]
    public Property ComposeOrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.Or)
            .Description.Length == 0).ToProperty();

    [Property]
    public Property ComposeOrPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Or, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
    public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeOrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(
            pattern, PatternComposition.Or, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeOrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Compose(null, PatternComposition.Or);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeOrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Compose(null, PatternComposition.Or, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeOrPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.Compose(pattern2, PatternComposition.Or, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property OrPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Or(pattern2) != null).ToProperty();

    [Property]
    public Property OrPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> OrPatternShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Or(pattern2).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> OrPatternWithDescriptionShouldBeSameAsEitherPattern(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful || (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Or(pattern2, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property OrPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Or(pattern2).Description ==
                String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property]
    public Property OrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

    [Property]
    public Property OrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Or(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property]
    public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).Or(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property]
    public Property OrPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Or(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void OrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Or(pattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void OrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Or(pattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void OrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Or(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void OrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Or(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void OrPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.Or(pattern2, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
