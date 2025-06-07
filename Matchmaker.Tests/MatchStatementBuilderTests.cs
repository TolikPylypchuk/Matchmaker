namespace Matchmaker;

public class MatchStatementBuilderTests
{
    [Fact(DisplayName = "Match.CreateStatic should never return null")]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        Assert.NotNull(Match.CreateStatic<int>(match => { }));

    [Fact(DisplayName = "Match.CreateStatic should create expression once")]
    public void MatchCreateStaticShouldCreateStatementOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            Match.CreateStatic<int>(match => { counter++; });
        }

        Assert.Equal(1, counter);
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

        Assert.Equal(2, counter);
    }

    [Fact(DisplayName = "Match.CreateStatic should throw if build action is null")]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.CreateStatic<int>(null));

    [Fact(DisplayName = "Match.CreateStatic should throw if file path is null")]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.CreateStatic<int>(match => { }, null));

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

        void action() =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ExecuteOn(value);

        if (pattern.Match(value).IsSuccessful)
        {
            var exception = Record.Exception(action);
            Assert.Null(exception);
        } else
        {
            Assert.Throws<MatchException>(action);
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

        void action() =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ExecuteNonStrict(value);

        var exception = Record.Exception(action);
        Assert.Null(exception);
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

        void action() =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ToFunction()(value);

        if (pattern.Match(value).IsSuccessful)
        {
            var exception = Record.Exception(action);
            Assert.Null(exception);
        } else
        {
            Assert.Throws<MatchException>(action);
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

        void action() =>
            Match.CreateStatic<string>(match => match
                    .Case(pattern, _ => { }))
                .ToNonStrictFunction()(value);

        var exception = Record.Exception(action);
        Assert.Null(exception);
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
    public void MatchShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() =>
            Match.CreateStatic<string>(match => match.Case<string>(null, _ => { })));

    [Property(DisplayName = "Match should throw if case function is null")]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() => Match.CreateStatic<string>(match => match.Case(pattern, null)));
    }

    [Fact(DisplayName = "Match should throw if case type function is null")]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.CreateStatic<string>(match => match.Case<string>(null)));

    [Property(DisplayName = "Match should throw if pattern with fall-through is null")]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() =>
            Match.CreateStatic<string>(match => match.Case<string>(null, fallthrough, _ => { })));

    [Property(DisplayName = "Match should throw if case function with fall-through is null")]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() =>
            Match.CreateStatic<string>(match => match.Case(pattern, fallthrough, null)));
    }

    [Property(DisplayName = "Match should throw if case type sync function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() =>
            Match.CreateStatic<string>(match => match.Case<string>(fallthrough, null)));
}
