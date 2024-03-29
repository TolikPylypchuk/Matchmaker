namespace Matchmaker.Linq;

using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

public class ComposeXorTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Xor).Match(x).IsSuccessful).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternWithDescriptionShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
                String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Compose(pattern, PatternComposition.Xor)
            .Description.Length == 0)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.Xor)
            .Description.Length == 0)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty)
            .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.Xor)
            .Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property ComposeXorPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Description == description.Get)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeXorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Compose(
            pattern, PatternComposition.Xor, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeXorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Compose(null, PatternComposition.Xor);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeXorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Compose(null, PatternComposition.Xor, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void ComposeXorPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2)
    {
        var action = () => pattern1.Compose(pattern2, PatternComposition.Xor, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Xor(pattern2) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Xor(pattern2).Match(x).IsSuccessful).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternWithDescriptionShouldBeSameAsExcusiveEitherPattern(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        string x,
        NonNull<string> description) =>
        ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
            pattern1.Xor(pattern2, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldHaveCorrectDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2) =>
        (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Xor(pattern2).Description ==
                String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Xor(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).Xor(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property XorPatternShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2,
        NonNull<string> description) =>
        (pattern1.Xor(pattern2, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void XorPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => ((IPattern<string, string>)null).Xor(pattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void XorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Xor(pattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void XorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Xor(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void XorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Xor(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void XorPatternShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern1,
        IPattern<string, string> pattern2)
    {
        var action = () => pattern1.Xor(pattern2, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
