namespace Matchmaker.Linq;

public class AsyncComposeAndTests
{
    [Property]
    public Property ComposeAndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.And) != null).ToProperty();

    [Property]
    public Property ComposeAndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> ComposeAndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.And).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> ComposeAndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.Compose(pattern2, PatternComposition.And, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property ComposeAndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.And).Description ==
             String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.And).Description.Length == 0)
            .ToProperty();

    [Property]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.And)
            .Description.Length == 0)
            .ToProperty();

    [Property]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.And)
            .Description.Length == 0).ToProperty();

    [Property]
    public Property ComposeAndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
    public void ComposeAndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.And);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeAndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(
            pattern, PatternComposition.And, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeAndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Compose(null, PatternComposition.And);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeAndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Compose(null, PatternComposition.And, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void ComposeAndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.Compose(pattern2, PatternComposition.And, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property AndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.And(pattern2) != null).ToProperty();

    [Property]
    public Property AndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> AndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.And(pattern2).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> AndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        (((await pattern1.MatchAsync(x)).IsSuccessful && (await pattern2.MatchAsync(x)).IsSuccessful) ==
            (await pattern1.And(pattern2, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property AndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.And(pattern2).Description ==
                String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property]
    public Property AndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

    [Property]
    public Property AndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.And(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property]
    public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).And(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property]
    public Property AndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void AndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).And(pattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).And(pattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.And(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.And(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void AndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.And(pattern2, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
