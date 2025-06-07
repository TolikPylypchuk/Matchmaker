namespace Matchmaker;

public class MatchStatementBuilderTests
{
    [Fact]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        Match.CreateStatic<int>(match => { })
            .Should()
            .NotBeNull();

    [Fact]
    public void MatchCreateStaticShouldCreateStatementOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            Match.CreateStatic<int>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact]
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

    [Fact]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => Match.CreateStatic<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => Match.CreateStatic<int>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public Property NonStrictMatchShouldReturnFalseIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ExecuteNonStrict(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Fact]
    public void MatchShouldThrowIfPatternIsNull()
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null, _ => { }));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string>(match => match.Case(pattern, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull()
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null));
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(null, fallthrough, _ => { }));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string>(match => match.Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string>(match => match.Case<string>(fallthrough, null));
        action.Should().Throw<ArgumentNullException>();
    }
}
