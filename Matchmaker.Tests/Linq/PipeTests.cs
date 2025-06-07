namespace Matchmaker.Linq;

public class PipeTests
{
    [Property]
    public Property PipePatternShouldNeverReturnNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern) =>
        (firstPattern.Pipe(secondPattern) != null).ToProperty();

    [Property]
    public Property PipePatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get) != null).ToProperty();

    [Property]
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

    [Property]
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

    [Property]
    public Property PipePatternShouldHaveCorrectDescription(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern) =>
        (firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0).ImpliesThat(() =>
                firstPattern.Pipe(secondPattern).Description ==
                String.Format(
                    Pattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description))
            .ToProperty();

    [Property]
    public Property PipePatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty).Pipe(pattern).Description.Length == 0)
            .ToProperty();

    [Property]
    public Property PipePatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Pipe(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
            .ToProperty();

    [Property]
    public Property PipePatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, bool> predicate1,
        Func<string, bool> predicate2) =>
        (Pattern.CreatePattern(predicate1, String.Empty).Pipe(Pattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property]
    public Property PipePatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void PipePatternShouldThrowIfFirstPatternIsNull(IPattern<string, string> secondPattern)
    {
        var action = () => ((IPattern<string, string>)null).Pipe(secondPattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithDescriptionShouldThrowIfFirstPatternIsNull(
        IPattern<string, string> secondPattern,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Pipe(secondPattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternShouldThrowIfSecondPatternIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Pipe((IPattern<string, int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithDescriptionShouldThrowIfSecondPatternIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Pipe((IPattern<string, int>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> firstPattern,
        IPattern<string, string> secondPattern)
    {
        var action = () => firstPattern.Pipe(secondPattern, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property PipePatternWithFunctionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher) =>
        (pattern.Pipe(matcher) != null).ToProperty();

    [Property]
    public Property PipePatternWithFunctionAndDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get) != null).ToProperty();

    [Property]
    public Property PipePatternWithFunctionShouldMatchSameAsPatterns(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() => pattern.Pipe(matcher).Match(x) == matcher(result.Value))
            .ToProperty();
    }

    [Property]
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

    [Property]
    public Property PipePatternWithFunctionShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher) =>
        (pattern.Pipe(matcher).Description == pattern.Description).ToProperty();

    [Property]
    public Property PipePatternWithFunctionAndDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void PipePatternWithFunctionShouldThrowIfPatternIsNull(Func<string, MatchResult<string>> matcher)
    {
        var action = () => ((IPattern<string, string>)null).Pipe(matcher);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfPatternIsNull(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Pipe(matcher, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithFunctionShouldThrowIfMatcherIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Pipe((Func<string, MatchResult<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfMatcherIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Pipe((Func<string, MatchResult<int>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, MatchResult<string>> matcher)
    {
        var action = () => pattern.Pipe(matcher, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property CastPatternShouldNeverReturnNull(IPattern<string, object> pattern) =>
        (pattern.Cast<string, object, string>() != null).ToProperty();

    [Property]
    public Property CastPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get) != null).ToProperty();

    [Property]
    public Property CastPatternShouldMatchSameAsPatterns(IPattern<string, object> pattern, string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
                pattern.Cast<string, object, string>().Match(x) ==
                Pattern.Type<object, string>().Match(result.Value))
            .ToProperty();
    }

    [Property]
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

    [Property]
    public Property CastPatternShouldHaveCorrectDescription(IPattern<string, object> pattern) =>
        (pattern.Description.Length > 0).ImpliesThat(() =>
                pattern.Cast<string, object, string>().Description ==
                String.Format(
                    Pattern.DefaultPipeDescriptionFormat,
                    pattern.Description,
                    String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(string))))
            .ToProperty();

    [Property]
    public Property CastPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate, String.Empty)
                .Select(value => (object)value)
                .Cast<string, object, string>()
                .Description.Length == 0)
            .ToProperty();

    [Property]
    public Property CastPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void CastPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IPattern<string, object>)null).Cast<string, object, string>();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void CastPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IPattern<string, object>)null).Cast<string, object, string>(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void CastPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, object> pattern)
    {
        var action = () => pattern.Cast<string, object, string>(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
