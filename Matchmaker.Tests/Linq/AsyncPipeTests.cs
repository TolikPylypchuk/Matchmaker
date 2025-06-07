namespace Matchmaker.Linq;

public class AsyncPipeTests
{
    [Property(DisplayName = "Pipe pattern should never return null")]
    public Property PipePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern) =>
        (firstPattern.Pipe(secondPattern) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with description should never return null")]
    public Property PipePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern should match the same as patterns")]
    public async Task<Property> PipePatternShouldMatchSameAsPatterns(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern,
        string x)
    {
        var result = await firstPattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await firstPattern.Pipe(secondPattern).MatchAsync(x) == await secondPattern.MatchAsync(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with description should match the same as pattern")]
    public async Task<Property> PipePatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern,
        string x,
        NonNull<string> description)
    {
        var result = await firstPattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await firstPattern.Pipe(secondPattern, description.Get).MatchAsync(x) ==
                await secondPattern.MatchAsync(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern should have correct description")]
    public Property PipePatternShouldHaveCorrectDescription(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern) =>
        (firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0).ImpliesThat(() =>
                firstPattern.Pipe(secondPattern).Description ==
                String.Format(
                    AsyncPattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description))
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the first pattern has empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty).Pipe(pattern).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the second pattern has empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Pipe(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Pipe pattern should havee empty description if the both patterns have empty description")]
    public Property PipePatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
        Func<string, Task<bool>> predicate1,
        Func<string, Task<bool>> predicate2) =>
        (AsyncPattern.CreatePattern(predicate1, String.Empty)
            .Pipe(AsyncPattern.CreatePattern(predicate2, String.Empty))
            .Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pipe pattern with description should have the specified description")]
    public Property PipePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern,
        NonNull<string> description) =>
        (firstPattern.Pipe(secondPattern, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pipe pattern should throw if the first pattern is null")]
    public void PipePatternShouldThrowIfFirstPatternIsNull(IAsyncPattern<string, string> secondPattern)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Pipe(secondPattern);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with description should throw if the first pattern is null")]
    public void PipePatternWithDescriptionShouldThrowIfFirstPatternIsNull(
        IAsyncPattern<string, string> secondPattern,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Pipe(secondPattern, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern should throw if the second pattern is null")]
    public void PipePatternShouldThrowIfSecondPatternIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Pipe((IAsyncPattern<string, int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with description should throw if the second pattern is null")]
    public void PipePatternWithDescriptionShouldThrowIfSecondPatternIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Pipe((IAsyncPattern<string, int>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with description should throw if description is null")]
    public void PipePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> firstPattern,
        IAsyncPattern<string, string> secondPattern)
    {
        var action = () => firstPattern.Pipe(secondPattern, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with function should never return null")]
    public Property PipePatternWithFunctionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher) =>
        (pattern.Pipe(matcher) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with function and description should never return null")]
    public Property PipePatternWithFunctionAndDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get) != null).ToProperty();

    [Property(DisplayName = "Pipe pattern with function should match the same as patterns")]
    public async Task<Property> PipePatternWithFunctionShouldMatchSameAsPatterns(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher,
        string x)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Pipe(matcher).MatchAsync(x) == await matcher(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with function and description should match the same as pattern")]
    public async Task<Property> PipePatternWithFunctionAndDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher,
        string x,
        NonNull<string> description)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Pipe(matcher, description.Get).MatchAsync(x) == await matcher(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Pipe pattern with function should have the same description as pattern")]
    public Property PipePatternWithFunctionShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher) =>
        (pattern.Pipe(matcher).Description == pattern.Description).ToProperty();

    [Property(DisplayName = "Pipe pattern with function and description should have the specified description")]
    public Property PipePatternWithFunctionAndDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (pattern.Pipe(matcher, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pipe pattern with function should throw if pattern is null")]
    public void PipePatternWithFunctionShouldThrowIfPatternIsNull(Func<string, Task<MatchResult<string>>> matcher)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Pipe(matcher);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with function and description should throw if pattern is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfPatternIsNull(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Pipe(matcher, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with function should throw if matcher is null")]
    public void PipePatternWithFunctionShouldThrowIfMatcherIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Pipe((Func<string, Task<MatchResult<int>>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with function and description should throw if matcher is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfMatcherIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Pipe((Func<string, Task<MatchResult<int>>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Pipe pattern with function and description should throw if description is null")]
    public void PipePatternWithFunctionAndDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<MatchResult<string>>> matcher)
    {
        var action = () => pattern.Pipe(matcher, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cast pattern should never return null")]
    public Property CastPatternShouldNeverReturnNull(IAsyncPattern<string, object> pattern) =>
        (pattern.Cast<string, object, string>() != null).ToProperty();

    [Property(DisplayName = "Cast pattern with description should never return null")]
    public Property CastPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Cast pattern should match the same as pattern")]
    public async Task<Property> CastPatternShouldMatchSameAsPattern(IAsyncPattern<string, object> pattern, string x)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Cast<string, object, string>().MatchAsync(x) ==
                await AsyncPattern.Type<object, string>().MatchAsync(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Cast pattern with description should match the same as pattern")]
    public async Task<Property> CastPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, object> pattern,
        string x,
        NonNull<string> description)
    {
        var result = await pattern.MatchAsync(x);
        return (await result.IsSuccessful.ImpliesThatAsync(async () =>
                await pattern.Cast<string, object, string>(description.Get).MatchAsync(x) ==
                await AsyncPattern.Type<object, string>().MatchAsync(result.Value)))
            .ToProperty();
    }

    [Property(DisplayName = "Cast pattern should have correct description")]
    public Property CastPatternShouldHaveCorrectDescription(IAsyncPattern<string, object> pattern) =>
        (pattern.Description.Length > 0).ImpliesThat(() =>
                pattern.Cast<string, object, string>().Description ==
                String.Format(
                    AsyncPattern.DefaultPipeDescriptionFormat,
                    pattern.Description,
                    String.Format(AsyncPattern.DefaultTypeDescriptionFormat, typeof(string))))
            .ToProperty();

    [Property(DisplayName = "Cast pattern should have empty description if pattern has empty description")]
    public Property CastPatternShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(
        Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate, String.Empty)
                .Select(value => (object)value)
                .Cast<string, object, string>()
                .Description.Length == 0)
            .ToProperty();

    [Property(DisplayName = "Cast pattern with description should have the specified description")]
    public Property CastPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, object> pattern,
        NonNull<string> description) =>
        (pattern.Cast<string, object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Cast pattern should throw if pattern is null")]
    public void CastPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IAsyncPattern<string, object>)null).Cast<string, object, string>();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cast pattern with description should throw if pattern is null")]
    public void CastPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, object>)null).Cast<string, object, string>(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Cast pattern with description should throw if description is null")]
    public void CastPatternWithDescriptionShouldThrowIfDescriptionIsNull(IAsyncPattern<string, object> pattern)
    {
        var action = () => pattern.Cast<string, object, string>(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
