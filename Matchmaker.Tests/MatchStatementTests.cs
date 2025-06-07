namespace Matchmaker;

public class MatchStatementTests
{
    [Fact(DisplayName = "Match.Create should never return null")]
    public void MatchCreateShouldNeverReturnNull() =>
        Assert.NotNull(Match.Create<int>());

    [Property(DisplayName = "Match.Create with fall-through should never return null")]
    public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault) =>
        Assert.NotNull(Match.Create<int>(fallthroughByDefault));

    [Property(DisplayName = "Match should match patterns correctly")]
    public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
            .ExecuteOn(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
    public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
            .ExecuteNonStrict(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
    public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string>()
                .Case(pattern, _ => { })
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
        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.Create<string>()
            .Case(pattern, _ => { })
            .ExecuteNonStrict(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
    public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string>()
                .Case(pattern, _ => { })
                .ExecuteNonStrict(value);

        var exception = Record.Exception(action);
        Assert.Null(exception);
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
    public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthrough(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should be lazy")]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => count++)
            .Case(Pattern.Any<string>(), _ => count++)
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly")]
    public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthrough(value)
            .Count();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly")]
    public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthrough(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should be lazy")]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(Pattern.Any<string>(), fallthrough: true, _ => count++)
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
    public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ExecuteWithFallthrough(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction should match patterns correctly")]
    public Property MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
            .ToFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        Match.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
            .ToNonStrictFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should throw if no match found")]
    public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string>()
                .Case(pattern, _ => { })
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

    [Property(DisplayName = "Non-strict ToFunction should return false if no match found")]
    public Property NonStrictMatchToFunctionShouldReturnFalseIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.Create<string>()
            .Case(pattern, _ => { })
            .ToNonStrictFunction()(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
    public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string>()
                .Case(pattern, _ => { })
                .ToNonStrictFunction()(value);

        var exception = Record.Exception(action);
        Assert.Null(exception);
    }

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
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
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
            .ToFunctionWithFallthrough()(value)
            .Count();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = Match.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => { matchCount++; })
            .Case(Pattern.Any<string>(), _ => { matchCount++; })
            .ToFunctionWithFallthrough()(value)
            .Count();

        return pattern.Match(value).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
    public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ToFunctionWithFallthrough()(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Fact(DisplayName = "Match should throw if pattern is null")]
    public void MatchShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case<string>(null, _ => { }));

    [Property(DisplayName = "Match should throw if case function is null")]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case(pattern, null));
    }

    [Fact(DisplayName = "Match should throw if case type function is null")]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case<string>(null));

    [Property(DisplayName = "Match should throw if pattern with fall-through is null")]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case<string>(null, fallthrough, _ => { }));

    [Property(DisplayName = "Match should throw if case function with fall-through is null")]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case(pattern, fallthrough, null));
    }

    [Property(DisplayName = "Match should throw if case type function with fall-through is null")]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string>().Case<string>(fallthrough, null));
}
