namespace Matchmaker;

public class MatchStatementTests
{
    [Fact]
    public void MatchCreateShouldNeverReturnNull() =>
        Match.Create<int>()
            .Should()
            .NotBeNull();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault) =>
        Match.Create<int>(fallthroughByDefault)
            .Should()
            .NotBeNull();

    [Property]
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

    [Property]
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

    [Property]
    public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.Create<string>()
                .Case(pattern, _ => { })
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
        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.Create<string>()
            .Case(pattern, _ => { })
            .ExecuteNonStrict(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.Create<string>()
                .Case(pattern, _ => { })
                .ExecuteNonStrict(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ExecuteWithFallthrough(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Property]
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

    [Property]
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

    [Property]
    public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.Create<string>()
                .Case(pattern, _ => { })
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
        var pattern = Pattern.CreatePattern(predicate);

        bool matched = Match.Create<string>()
            .Case(pattern, _ => { })
            .ToNonStrictFunction()(value);

        return (matched == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.Create<string>()
                .Case(pattern, _ => { })
                .ToNonStrictFunction()(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        int result = Match.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ToFunctionWithFallthrough()(value)
            .Count();

        return (result == 0).ToProperty();
    }

    [Fact]
    public void MatchShouldThrowIfPatternIsNull()
    {
        var action = () => Match.Create<string>()
            .Case<string>(null, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.Create<string>()
            .Case(pattern, null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull()
    {
        var action = () => Match.Create<string>()
            .Case<string>(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.Create<string>()
            .Case<string>(null, fallthrough, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.Create<string>()
            .Case(pattern, fallthrough, null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.Create<string>()
            .Case<string>(fallthrough, null);

        action.Should().Throw<ArgumentNullException>();
    }
}
