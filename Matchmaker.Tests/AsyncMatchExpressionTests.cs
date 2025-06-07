namespace Matchmaker;

public class AsyncMatchExpressionTests
{
    [Fact(DisplayName = "AsyncMatch.Create should never return null")]
    public void MatchCreateShouldNeverReturnNull() =>
        AsyncMatch.Create<int, string>()
            .Should()
            .NotBeNull();

    [Property(DisplayName = "AsyncMatch.Create with fall-through should never return null")]
    public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault) =>
        AsyncMatch.Create<int, string>(fallthroughByDefault)
            .Should()
            .NotBeNull();

    [Property(DisplayName = "Match should match patterns correctly with async pattern and async action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => Task.FromResult(true))
            .Case(AsyncPattern.Any<string>(), _ => Task.FromResult(false))
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with async pattern and sync action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with sync pattern and async action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => Task.FromResult(true))
            .Case(Pattern.Any<string>(), _ => Task.FromResult(false))
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with sync pattern and sync action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(Pattern.Any<string>(), _ => false)
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteNonStrictAsync(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly with null")]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => (bool?)null)
            .ExecuteNonStrictAsync(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly with nullable")]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteNonStrictAsync(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : null;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should return nothing if no match found")]
    public async Task<Property> NonStrictMatchShouldReturnNothingIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ExecuteNonStrictAsync(value);

        return (result.IsSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
    public async Task MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ExecuteAsync(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
    public async Task NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ExecuteNonStrictAsync(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly with nullable")]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should be lazy")]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.Create<string, int>(fallthroughByDefault: true)
            .Case(pattern, _ => count++)
            .Case(AsyncPattern.Any<string>(), _ => count++)
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly with nullable")]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly with nullable")]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should be lazy")]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.Create<string, int>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++)
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should never return null")]
    public async Task<Property> MatchWithFallthroughShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        return (result != null).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
    public async Task<Property> MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        return result.SequenceEqual([]).ToProperty();
    }

    [Property(DisplayName = "ToFunction should match patterns correctly")]
    public async Task<Property> MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToNonStrictFunction()(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with null")]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => (bool?)null)
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly with nullable")]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>()
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should return nothing if no match found")]
    public async Task<Property> NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ToNonStrictFunction()(value);

        return (result.IsSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "ToFunction should throw if no match found")]
    public async Task MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ToFunction()(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
    public async Task NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, _ => true)
            .ToNonStrictFunction()(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly with nullable")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should be lazy")]
    public Property MatchToFunctionWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        AsyncMatch.Create<string, int>(fallthroughByDefault: true)
            .Case(pattern, _ => count++)
            .Case(AsyncPattern.Any<string>(), _ => count++)
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly with nullable")]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly with nullable")]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool?>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should be lazy")]
    public Property MatchToFunctionWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        AsyncMatch.Create<string, int>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++)
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should never return null")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .Case(AsyncPattern.Any<string>(), _ => false)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        return (result != null).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        var result = await AsyncMatch.Create<string, bool>(fallthroughByDefault: true)
            .Case(pattern, _ => true)
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        return result.SequenceEqual([]).ToProperty();
    }

    [Fact(DisplayName = "Match should throw if async pattern is null")]
    public void MatchShouldThrowIfAsyncPatternIsNull()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IAsyncPattern<string, string>)null, _ => true);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if sync pattern is null with async action")]
    public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IPattern<string, string>)null, _ => Task.FromResult(true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if async pattern is null with async action")]
    public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IAsyncPattern<string, string>)null, _ => Task.FromResult(true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if sync pattern is null with sync action")]
    public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IPattern<string, string>)null, _ => true);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function is null with sync action")]
    public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, (Func<string, Task<bool>>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match Should throw if case type async function is null")]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((Func<string, Task<bool>>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if case type sync function is null")]
    public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((Func<string, bool>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if async pattern with fall-through is null with async action")]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.FromResult(true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if async pattern with fall-through is null with sync action")]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => true);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if sync pattern with fall-through is null with async action")]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IPattern<string, string>)null, fallthrough, _ => Task.FromResult(true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if sync pattern with fall-through is null with sync action")]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case((IPattern<string, string>)null, fallthrough, _ => true);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case async function with fall-through is null with async pattern")]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, fallthrough, (Func<string, Task<bool>>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case sync function with fall-through is null with async pattern")]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, fallthrough, (Func<string, bool>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case async function with fallthrough is null with sync pattern")]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, fallthrough, (Func<string, Task<bool>>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case sync function with fallthrough is null with sync pattern")]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string, bool>()
            .Case(pattern, fallthrough, (Func<string, bool>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type async function with fall-through is null")]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case(fallthrough, (Func<string, Task<bool>>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type sync function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string, bool>()
            .Case(fallthrough, (Func<string, bool>)null);

        action.Should().Throw<ArgumentNullException>();
    }
}
