namespace Matchmaker.Linq;

public class PipeTests
{
    [Property(DisplayName = "Pipe pattern should never return null")]
    public Property PipePatternShouldNeverReturnNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern) =>
        (firstPattern.Pipe(secondPattern) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with description should never return null")]
    public Property PipePatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern should match the same as patterns")]
    public Property PipePatternShouldMatchSameAsPatterns(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        string x)
    {
        var result = firstPattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
                firstPattern.Pipe(secondPattern).Match(x) == secondPattern.Match(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with description should match the same as pattern")]
    public Property PipePatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        string x,
        NonNull<string> description)
    {
        var result = firstPattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
            firstPattern.Pipe(secondPattern, description.Get).Match(x) == secondPattern.Match(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern should have correct description")]
    public Property PipePatternShouldHaveCorrectDescription(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern) =>
        (firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0).ImpliesThat(() =>
                firstPattern.Pipe(secondPattern).Description ==
                String.Format(
                    Pattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description))
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the first pattern has empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Pipe(pattern).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the second pattern has empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Pipe(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the both patterns have empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).Pipe(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pipe pattern with description should have the specified description")]
    public Property PipePatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pipe pattern should throw if the first pattern is null")]
    public void PipePatternShouldThrowIfFirstPatternIsNull(IPattern<string, string> secondPattern) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Pipe(secondPattern));

    [Property(DisplayName = "Pipe pattern with description should throw if the first pattern is null")]
    public void PipePatternWithDescriptionShouldThrowIfFirstPatternIsNull(
        IPattern<string, string> secondPattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, string>)null).Pipe(secondPattern, description.Get));

    [Property(DisplayName = "Pipe pattern should throw if the second pattern is null")]
    public void PipePatternShouldThrowIfSecondPatternIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Pipe((IPattern<string, int>)null));

    [Property(DisplayName = "Pipe pattern with description should throw if the second pattern is null")]
    public void PipePatternWithDescriptionShouldThrowIfSecondPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Pipe((IPattern<string, int>)null, description.Get));

    [Property(DisplayName = "Pipe pattern with description should throw if description is null")]
    public void PipePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern) =>
        Assert.Throws<ArgumentNullException>(() => firstPattern.Pipe(secondPattern, null));

    [Property(DisplayName = "Pipe pattern with function should never return null")]
    public Property PipePatternWithFunctionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher) =>
        (pattern.Pipe(matcher) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with function and description should never return null")]
    public Property PipePatternWithFunctionAndDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with function should match the same as patterns")]
    public Property PipePatternWithFunctionShouldMatchSameAsPatterns(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() => pattern.Pipe(matcher).Match(x) == matcher(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with function and description should match the same as pattern")]
    public Property PipePatternWithFunctionAndDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        string x,
        NonNull<string> description)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
            pattern.Pipe(matcher, description.Get).Match(x) == matcher(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with function should have the same description as pattern")]
    public Property PipePatternWithFunctionShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher) =>
        (pattern.Pipe(matcher).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Pipe pattern with function and description should have the specified description")]
    public Property PipePatternWithFunctionAndDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pipe pattern with function should throw if pattern is null")]
    public void PipePatternWithFunctionShouldThrowIfPatternIsNull(Func<string, MatchResult<string>> matcher) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Pipe(matcher));

    [Property(DisplayName = "Pipe pattern with function and description should throw if pattern is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfPatternIsNull(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, string>)null).Pipe(matcher, description.Get));

    [Property(DisplayName = "Pipe pattern with function should throw if matcher is null")]
    public void PipePatternWithFunctionShouldThrowIfMatcherIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Pipe((Func<string, MatchResult<int>>)null));

    [Property(DisplayName = "Pipe pattern with function and description should throw if matcher is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfMatcherIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Pipe((Func<string, MatchResult<int>>)null, description.Get));

    [Property(DisplayName = "Pipe pattern with function and description should throw if description is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Pipe(matcher, null));

    [Property(DisplayName = "Cast pattern should never return null")]
    public Property CastPatternShouldNeverReturnNull(IPattern<string, object> pattern) =>
        (pattern.Cast<string, object, string>() != null).ToProperty();

    [Property(DisplayName = "Cast pattern with description should never return null")]
    public Property CastPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Cast pattern should match the same as pattern")]
    public Property CastPatternShouldMatchSameAsPattern(IPattern<string, object> pattern, string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
                pattern.Cast<string, object, string>().Match(x) ==
                Pattern.Type<object, string>().Match(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Cast pattern with description should match the same as pattern")]
    public Property CastPatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, object> pattern,
        string x,
        NonNull<string> description)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
            pattern.Cast<string, object, string>(description.Get).Match(x) ==
            Pattern.Type<object, string>().Match(result.Value))
            .ToProperty();
    }

    [Property(DisplayName = "Cast pattern should have correct description")]
    public Property CastPatternShouldHaveCorrectDescription(IPattern<string, object> pattern) =>
        (pattern.Description.Length > 0).ImpliesThat(() =>
                pattern.Cast<string, object, string>().Description ==
                String.Format(
                    Pattern.DefaultPipeDescriptionFormat,
                    pattern.Description,
                    String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(string))))
            .ToProperty();

    [Property(DisplayName = "Cast pattern should have empty description if pattern has empty description")]
    public Property CastPatternShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty)
                .Select(value => (object)value)
                .Cast<string, object, string>()
                .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Cast pattern with description should have the specified description")]
    public Property CastPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Cast pattern should throw if pattern is null")]
    public void CastPatternShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() => ((IPattern<string, object>)null).Cast<string, object, string>());

    [Property(DisplayName = "Cast pattern with description should throw if pattern is null")]
    public void CastPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            ((IPattern<string, object>)null).Cast<string, object, string>(description.Get));

    [Property(DisplayName = "Cast pattern with description should throw if description is null")]
    public void CastPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, object> pattern) =>
        Assert.Throws<ArgumentNullException>(() => pattern.Cast<string, object, string>(null));
}
