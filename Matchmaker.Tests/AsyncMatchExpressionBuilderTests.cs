namespace Matchmaker;

public class AsyncMatchExpressionBuilderTests
{
    [Fact]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        AsyncMatch.CreateStatic<int, string>(match => { })
            .Should()
            .NotBeNull();

    [Fact]
    public void MatchCreateStaticShouldCreateExpressionOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            AsyncMatch.CreateStatic<int, string>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact]
    public void MatchClearCacheShouldForceStaticMatchCreation()
    {
        int counter = 0;

        void CreateMatchExression() =>
            AsyncMatch.CreateStatic<int, string>(match => { counter++; });

        CreateMatchExression();

        AsyncMatch.ClearCache<int, string>();

        CreateMatchExression();

        counter.Should().Be(2);
    }

    [Fact]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<int, string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<int, string>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => Task.FromResult(true))
                .Case(AsyncPattern.Any<string>(), _ => Task.FromResult(false)))
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
        Func<string, bool> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => Task.FromResult(true))
                .Case(Pattern.Any<string>(), _ => Task.FromResult(false)))
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
        Func<string, bool> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = Pattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false))
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteNonStrictAsync(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => Task.FromResult<bool?>(null)))
            .ExecuteNonStrictAsync(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteNonStrictAsync(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : null;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchShouldReturnNothingIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true))
                .ExecuteNonStrictAsync(value);

        return (result.IsSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ExecuteAsync(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property]
    public async Task NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ExecuteNonStrictAsync(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, int>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.CreateStatic<string, int>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => count++)
                .Case(AsyncPattern.Any<string>(), _ => count++))
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, int>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.CreateStatic<string, int>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++))
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ExecuteWithFallthroughAsync(value);

        return (result != null).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true))
            .ExecuteWithFallthroughAsync(value)
            .ToListAsync();

        return result.SequenceEqual([]).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matchSuccessful = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToNonStrictFunction()(value);

        bool matchSuccessful = result.IsSuccessful && result.Value;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => Task.FromResult<bool?>(null)))
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == true == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToNonStrictFunction()(value);

        var matchSuccessful = result.IsSuccessful ? result.Value : false;

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ToNonStrictFunction()(value);

        return (result.IsSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ToFunction()(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property]
    public async Task NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
                .Case(pattern, _ => true))
            .ToNonStrictFunction()(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, int>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        AsyncMatch.CreateStatic<string, int>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => count++)
                .Case(AsyncPattern.Any<string>(), _ => count++))
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool> { true, false };
        var failure = new List<bool> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool?>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = await AsyncMatch.CreateStatic<string, bool?>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        var success = new List<bool?> { true, false };
        var failure = new List<bool?> { false };

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? result.SequenceEqual(success).ToProperty()
            : result.SequenceEqual(failure).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string, int>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        AsyncMatch.CreateStatic<string, int>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++))
            .ToFunctionWithFallthrough()(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var result = AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true)
                .Case(AsyncPattern.Any<string>(), _ => false))
            .ToFunctionWithFallthrough()(value);

        return (result != null).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        AsyncMatch.ClearCache<string, bool>();

        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        var result = await AsyncMatch.CreateStatic<string, bool>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => true))
            .ToFunctionWithFallthrough()(value)
            .ToListAsync();

        return result.SequenceEqual([]).ToProperty();
    }

    [Fact]
    public void MatchShouldThrowIfAsyncPatternIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IAsyncPattern<string, string>)null, _ => true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IPattern<string, string>)null, _ => Task.FromResult(true)));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IAsyncPattern<string, string>)null, _ => Task.FromResult(true)));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IPattern<string, string>)null, _ => true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(pattern, (Func<string, Task<bool>>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((Func<string, Task<bool>>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((Func<string, bool>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.FromResult(true)));

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IPattern<string, string>)null, fallthrough, _ => Task.FromResult(true)));

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case((IPattern<string, string>)null, fallthrough, _ => true));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(pattern, fallthrough, (Func<string, Task<bool>>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(pattern, fallthrough, (Func<string, bool>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(pattern, fallthrough, (Func<string, Task<bool>>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(pattern, fallthrough, (Func<string, bool>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(fallthrough, (Func<string, Task<bool>>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string, bool>(match => match
            .Case(fallthrough, (Func<string, bool>)null));

        action.Should().Throw<ArgumentNullException>();
    }
}
