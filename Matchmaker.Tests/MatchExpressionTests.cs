namespace Matchmaker;

public class MatchExpressionTests
{
    [Fact(DisplayName = "Match.Create should never return null")]
    public void MatchCreateShouldNeverReturnNull() =>
        Assert.NotNull(Match.Create<int, string>());

    [Property(DisplayName = "Match.Create with fall-through should never return null")]
    public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault) =>
        Assert.NotNull(Match.Create<int, string>(fallthroughByDefault));

    [Property(DisplayName = "Match should match patterns correctly")]
    public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ExecuteOn(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
    public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ExecuteNonStrict(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly with null")]
    public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => null)
            .ExecuteNonStrict(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly with nullable")]
    public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ExecuteNonStrict(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : null;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should return nothing if no match found")]
    public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .ExecuteNonStrict(value);

        return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
    public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string, bool>()
                .Case(pattern, _ => true)
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

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
    public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .ExecuteNonStrict(value);

        var exception = Record.Exception(action);
        Assert.Null(exception);
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
    public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string, int>(fallthroughByDefault: true)
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

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string, int>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(Pattern.Any<string>(), fallthrough: true, _ => count++)
            .ExecuteWithFallthrough(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should never return null")]
    public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ExecuteWithFallthrough(value);

        return (result != null).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
    public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .ExecuteWithFallthrough(value);

        return result.SequenceEqual([]).ToProperty();
    }

    [Property(DisplayName = "ToFunction should match patterns correctly")]
    public Property MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ToFunction()(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ToNonStrictFunction()(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with null")]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => null)
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with nullable")]
    public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should return nothing if no match found")]
    public Property NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>()
            .Case(pattern, _ => true)
            .ToNonStrictFunction()(value);

        return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "ToFunction should throw if no match found")]
    public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string, bool>()
                .Case(pattern, _ => true)
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

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
    public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        void action() =>
            Match.Create<string, bool>()
                .Case(pattern, _ => true)
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

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string, int>(fallthroughByDefault: true)
            .Case(pattern, _ => count++)
            .Case(Pattern.Any<string>(), _ => count++)
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly")]
    public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool?>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
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
        var pattern = Pattern.CreatePattern(predicate);

        int count = 0;

        Match.Create<string, int>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(Pattern.Any<string>(), fallthrough: true, _ => count++)
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should never return null")]
    public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value);

        return (result != null).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
    public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = Pattern.CreatePattern<string>(_ => false);

        var result = Match.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .ToFunctionWithFallthrough()(value);

        return result.SequenceEqual([]).ToProperty();
    }

    [Fact(DisplayName = "Match should throw if pattern is null")]
    public void MatchShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string, bool>().Case<string>(null, _ => true));

    [Property(DisplayName = "Match should throw if case function is null")]
    public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() => Match.Create<string, bool>().Case(pattern, null));
    }

    [Fact(DisplayName = "Match Should throw if case type function is null")]
    public void MatchShouldThrowIfCaseTypeFunctionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string, bool>().Case<string>(null));

    [Property(DisplayName = "Match should throw if pattern with fall-through is null")]
    public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() =>
            Match.Create<string, bool>().Case<string>(null, fallthrough, _ => true));

    [Property(DisplayName = "Match should throw if case function with fall-through is null")]
    public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);
        Assert.Throws<ArgumentNullException>(() => Match.Create<string, bool>().Case(pattern, fallthrough, null));
    }

    [Property(DisplayName = "Match should throw if case type function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough) =>
        Assert.Throws<ArgumentNullException>(() => Match.Create<string, bool>().Case<string>(fallthrough, null));
}
