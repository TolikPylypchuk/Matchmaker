namespace Matchmaker;

public class MatchExpressionBuilderTests
{
    [Fact]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        Match.CreateStatic<int, string>(match => { })
            .Should()
            .NotBeNull();

    [Fact]
    public void MatchCreateStaticShouldCreateExpressionOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            Match.CreateStatic<int, string>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact]
    public void MatchClearCacheShouldForceStaticMatchCreation()
    {
        int counter = 0;

        void CreateMatchExression()
            => Match.CreateStatic<int, string>(match => { counter++; });

        CreateMatchExression();

        Match.ClearCache<int, string>();

        CreateMatchExression();

        counter.Should().Be(2);
    }

    [Fact]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => Match.CreateStatic<int, string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => Match.CreateStatic<int, string>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteOn(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteNonStrict(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => null))
            .ExecuteNonStrict(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteNonStrict(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : null;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ExecuteNonStrict(value);

        return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true))
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
    public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true))
                .ExecuteNonStrict(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property]
    public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, int>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string, int>(match => match
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
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, int>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string, int>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(Pattern.Any<string>(), fallthrough: true, _ => count++))
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteWithFallthrough(value);

        return (result != null).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern<string>(_ => false);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true))
            .ExecuteWithFallthrough(value);

        return result.SequenceEqual([]).ToProperty();
    }

    [Property]
    public Property MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToNonStrictFunction()(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => null))
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public Property NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ToNonStrictFunction()(value);

        return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true))
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
    public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var action = () =>
            Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true))
                .ToNonStrictFunction()(value);

        action.Should().NotThrow<MatchException>();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, int>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string, int>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => count++)
                .Case(Pattern.Any<string>(), _ => count++))
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool?>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool?>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return pattern.Match(value).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, int>();

        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.CreateStatic<string, int>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(Pattern.Any<string>(), fallthrough: true, _ => count++))
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(
        Func<string, bool> predicate,
        string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        return (result != null).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern<string>(_ => false);

        var result = Match.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true))
            .ToFunctionWithFallthrough()(value);

        return result.SequenceEqual([]).ToProperty();
    }

    [Fact]
    public void MatchShouldThrowIfPatternIsNull()
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null, _ => true));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string, bool>(match => match.Case(pattern, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull()
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null));
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null, fallthrough, _ => true));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string, bool>(match => match.Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(fallthrough, null));
        action.Should().Throw<ArgumentNullException>();
    }
}
