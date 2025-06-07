namespace Matchmaker;

public class MatchStatementBuilderTests
{
    [Fact(DisplayName = "Match.CreateStatic should never return null")]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        Match.CreateStatic<int>(match => { })
            .Should()
            .NotBeNull();

    [Fact(DisplayName = "Match.CreateStatic should create expression once")]
    public void MatchCreateStaticShouldCreateStatementOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            Match.CreateStatic<int>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact(DisplayName = "Match.ClearCache should force static match creation")]
    public void MatchClearCacheShouldForceStaticMatchCreation()
    {
        int counter = 0;

        void CreateMatchExression() =>
            Match.CreateStatic<int>(match => { counter++; });

        CreateMatchExression();

        Match.ClearCache<int>();

        CreateMatchExression();

        counter.Should().Be(2);
    }

    [Fact(DisplayName = "Match.CreateStatic should throw if build action is null")]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => Match.CreateStatic<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match.CreateStatic should throw if file path is null")]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => Match.CreateStatic<int>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should match patterns correctly")]
    public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false))
            .ExecuteOn(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
    public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false))
            .ExecuteNonStrict(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
    public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ExecuteOn(value);

        if (pattern.Match(value).IsSuccessful)
        {
            action.Should().NotThrow<MatchException>();
        } else
        {
            action.Should().Throw<MatchException>();
        }
    }

    [Property(DisplayName = "Non-strict match should return false if no match found")]
    public Property NonStrictMatchShouldReturnFalseIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ExecuteNonStrict(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
    public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ExecuteNonStrict(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
    public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthrough(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should be lazy")]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => count++)
                .Case(Pattern.Any<string>(), _ => count++))
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly")]
    public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthrough(value)
            .Count();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly")]
    public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthrough(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should be lazy")]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(Pattern.Any<string>(), fallthrough: true, _ => count++))
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
    public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { }))
            .ExecuteWithFallthrough(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction should match patterns correctly")]
    public Property MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false))
            .ToFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false))
            .ToNonStrictFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should return nothing if no match found")]
    public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ToFunction()(value);

        if (pattern.Match(value).IsSuccessful)
        {
            action.Should().NotThrow<MatchException>();
        } else
        {
            action.Should().Throw<MatchException>();
        }
    }

    [Property(DisplayName = "ToFunction should return false if no match found")]
    public Property NonStrictMatchToFunctionShouldReturnFalseIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ToNonStrictFunction()(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
    public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ToNonStrictFunction()(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .Count();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
    public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { }))
            .ToFunctionWithFallthrough()(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Fact(DisplayName = "Match should throw if pattern is null")]
    public void MatchShouldThrowIfPatternIsNull()
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null, _ => { }));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function is null")]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string>(match => match.Case(pattern, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if case type function is null")]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull()
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if pattern with fall-through is null")]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null, fallthrough, _ => { }));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function with fall-through is null")]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string>(match => match.Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type sync function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(fallthrough, null));
        action.Should().Throw<ArgumentNullException>();
    }
}
