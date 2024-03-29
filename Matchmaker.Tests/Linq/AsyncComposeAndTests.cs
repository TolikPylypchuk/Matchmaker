namespace Matchmaker.Linq;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

public class AsyncComposeAndTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.And) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        ((pattern1.MatchAsync(x).Result.IsSuccessful && pattern2.MatchAsync(x).Result.IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.And).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.MatchAsync(x).Result.IsSuccessful && pattern2.MatchAsync(x).Result.IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.And, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
            pattern1.Compose(pattern2, PatternComposition.And).Description ==
             String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
            .Compose(pattern, PatternComposition.And).Description.Length == 0)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.And)
            .Description.Length == 0)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.And)
            .Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeAndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.And, description.Get).Description == description.Get)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeAndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.And);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeAndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Compose(
            pattern, PatternComposition.And, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeAndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Compose(null, PatternComposition.And);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeAndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Compose(null, PatternComposition.And, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeAndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.Compose(pattern2, PatternComposition.And, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.And(pattern2) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x) =>
        ((pattern1.MatchAsync(x).Result.IsSuccessful && pattern2.MatchAsync(x).Result.IsSuccessful) ==
            pattern1.And(pattern2).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternWithDescriptionShouldBeSameAsBothPatterns(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.MatchAsync(x).Result.IsSuccessful && pattern2.MatchAsync(x).Result.IsSuccessful) ==
            pattern1.And(pattern2, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.And(pattern2).Description ==
                String.Format(AsyncPattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.And(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty).And(
                AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AndPatternShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.And(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AndPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).And(pattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).And(pattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AndPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.And(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.And(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AndPatternShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern1,
        IAsyncPattern<string, string> pattern2)
    {
        var action = () => pattern1.And(pattern2, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
