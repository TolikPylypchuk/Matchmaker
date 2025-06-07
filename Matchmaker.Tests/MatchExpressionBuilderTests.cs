namespace Matchmaker;

public class MatchExpressionBuilderTests
{
    [Fact(DisplayName = "Match.CreateStatic should never return null")]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        Match.CreateStatic<int, string>(match => { })
            .Should()
            .NotBeNull();

    [Fact(DisplayName = "Match.CreateStatic should create expression once")]
    public void MatchCreateStaticShouldCreateExpressionOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            Match.CreateStatic<int, string>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact(DisplayName = "Match.ClearCache should force static match creation")]
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

    [Fact(DisplayName = "Match.CreateStatic should throw if build action is null")]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => Match.CreateStatic<int, string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match.CreateStatic should throw if file path is null")]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => Match.CreateStatic<int, string>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should match patterns correctly")]
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

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
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

    [Property(DisplayName = "Non-strict match should match patterns correctly with null")]
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

    [Property(DisplayName = "Non-strict match should match patterns correctly with nullable")]
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

    [Property(DisplayName = "Non-strict match should return nothing if no match found")]
    public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        Match.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ExecuteNonStrict(value);

        return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
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

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
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

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
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

    [Property(DisplayName = "Match with fall-through should match patterns correctly with nullable")]
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

    [Property(DisplayName = "Match with fall-through should be lazy")]
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

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly")]
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

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly with nullable")]
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

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly")]
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

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly with nullable")]
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

    [Property(DisplayName = "Match with fall-through being true should be lazy")]
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

    [Property(DisplayName = "Match with fall-through should never return null")]
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

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
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

    [Property(DisplayName = "ToFunction should match patterns correctly")]
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

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
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

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with null")]
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

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with nullable")]
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

    [Property(DisplayName = "Non-strict ToFunction should return nothing if no match found")]
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

    [Property(DisplayName = "ToFunction should throw if no match found")]
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

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
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

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly")]
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

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly with nullable")]
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

    [Property(DisplayName = "ToFunction with fall-through should be lazy")]
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

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly")]
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

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly with nullable")]
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

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly")]
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

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly with nullable")]
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

    [Property(DisplayName = "ToFunction with fall-through being true should be lazy")]
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

    [Property(DisplayName = "ToFunction with fall-through should never return null")]
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

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
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

    [Fact(DisplayName = "Match should throw if pattern is null")]
    public void MatchShouldThrowIfPatternIsNull()
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null, _ => true));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function is null")]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string, bool>(match => match.Case(pattern, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match Should throw if case type function is null")]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull()
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if pattern with fall-through is null")]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(null, fallthrough, _ => true));
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function with fall-through is null")]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => Match.CreateStatic<string, bool>(match => match.Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => Match.CreateStatic<string, bool>(match => match.Case<string>(fallthrough, null));
        action.Should().Throw<ArgumentNullException>();
    }
}
