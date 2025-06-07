namespace Matchmaker;

public class AsyncMatchStatementBuilderTests
{
    [Fact(DisplayName = "AsyncMatch.CreateStatic should never return null")]
    public void MatchCreateStaticShouldNeverReturnNull() =>
        AsyncMatch.CreateStatic<int>(match => { })
            .Should()
            .NotBeNull();

    [Fact(DisplayName = "AsyncMatch.CreateStatic should create expression once")]
    public void MatchCreateStaticShouldCreateStatementOnce()
    {
        int counter = 0;

        for (int i = 0; i < 5; i++)
        {
            AsyncMatch.CreateStatic<int>(match => { counter++; });
        }

        counter.Should().Be(1);
    }

    [Fact(DisplayName = "AsyncMatch.ClearCache should force static match creation")]
    public void MatchClearCacheShouldForceStaticMatchCreation()
    {
        int counter = 0;

        void CreateMatchExression() =>
            AsyncMatch.CreateStatic<int>(match => { counter++; });

        CreateMatchExression();

        AsyncMatch.ClearCache<int>();

        CreateMatchExression();

        counter.Should().Be(2);
    }

    [Fact(DisplayName = "AsyncMatch.CreateStatic should throw if build action is null")]
    public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "AsyncMatch.CreateStatic should throw if file path is null")]
    public void MatchCreateStaticShouldThrowIfFilePathIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<int>(match => { }, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should match patterns correctly with async pattern and async action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ =>
                {
                    matchSuccessful = true;
                    return Task.CompletedTask;
                })
                .Case(AsyncPattern.Any<string>(), _ =>
                {
                    matchSuccessful = false;
                    return Task.CompletedTask;
                }))
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with async pattern and sync action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false))
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with sync pattern and async action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
        Func<string, bool> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ =>
                {
                    matchSuccessful = true;
                    return Task.CompletedTask;
                })
                .Case(Pattern.Any<string>(), _ =>
                {
                    matchSuccessful = false;
                    return Task.CompletedTask;
                }))
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should match patterns correctly with sync pattern and sync action")]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
        Func<string, bool> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false))
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should match patterns correctly")]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false))
            .ExecuteNonStrictAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Match should throw if no match found")]
    public async Task MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ExecuteAsync(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property(DisplayName = "Non-strict match should return false if no match found")]
    public async Task<Property> NonStrictMatchShouldReturnFalseIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matched = await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ExecuteNonStrictAsync(value);

        return (matched == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict match should not throw if no match found")]
    public async Task NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ExecuteNonStrictAsync(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property(DisplayName = "Match with fall-through should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should be lazy")]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => count++)
                .Case(AsyncPattern.Any<string>(), _ => count++))
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being false should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should match patterns correctly")]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through being true should be lazy")]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++))
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property(DisplayName = "Match with fall-through should return empty enumerable if no match found")]
    public async Task<Property> MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { }))
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (result == 0).ToProperty();
    }

    [Property(DisplayName = "ToFunction should match patterns correctly")]
    public async Task<Property> MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false))
            .ToFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should match patterns correctly")]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false))
            .ToNonStrictFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should return nothing if no match found")]
    public async Task MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ToFunction()(value);

        if ((await pattern.MatchAsync(value)).IsSuccessful)
        {
            await action.Should().NotThrowAsync<MatchException>();
        } else
        {
            await action.Should().ThrowAsync<MatchException>();
        }
    }

    [Property(DisplayName = "ToFunction should return false if no match found")]
    public async Task<Property> NonStrictMatchToFunctionShouldReturnFalseIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matched = await AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ToNonStrictFunction()(value);

        return (matched == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property(DisplayName = "Non-strict ToFunction should not throw if no match found")]
    public async Task NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
                .Case(pattern, _ => { }))
            .ToNonStrictFunction()(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property(DisplayName = "ToFunction with fall-through should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being false should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through being true should match patterns correctly")]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; }))
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property(DisplayName = "ToFunction with fall-through should return empty enumerable if no match found")]
    public async Task<Property> MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        AsyncMatch.ClearCache<string>();

        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        int result = await AsyncMatch.CreateStatic<string>(match => match
                .Fallthrough(true)
                .Case(pattern, _ => { }))
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (result == 0).ToProperty();
    }

    [Fact(DisplayName = "Match should throw if async pattern is null")]
    public void MatchShouldThrowIfAsyncPatternIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IAsyncPattern<string, string>)null, _ => { }));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if sync pattern is null with async action")]
    public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IPattern<string, string>)null, _ => Task.CompletedTask));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if async pattern is null with async action")]
    public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IAsyncPattern<string, string>)null, _ => Task.CompletedTask));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if sync pattern is null with sync action")]
    public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
    {
        Action action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IPattern<string, string>)null, _ => { }));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case function is null with sync action")]
    public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(pattern, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match Should throw if case type async function is null")]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((Func<string, Task>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Match should throw if case type sync function is null")]
    public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((Action<string>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if async pattern with fall-through is null with async action")]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.CompletedTask));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if async pattern with fall-through is null with sync action")]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => { }));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if sync pattern with fall-through is null with async action")]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IPattern<string, string>)null, fallthrough, _ => Task.CompletedTask));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if sync pattern with fall-through is null with sync action")]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case((IPattern<string, string>)null, fallthrough, _ => { }));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case async function with fall-through is null with async pattern")]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case sync function with fall-through is null with async pattern")]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(pattern, fallthrough, (Action<string>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case async function with fallthrough is null with sync pattern")]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(pattern, fallthrough, null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case sync function with fallthrough is null with sync pattern")]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(pattern, fallthrough, (Action<string>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type async function with fall-through is null")]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(fallthrough, (Func<string, Task>)null));

        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Match should throw if case type sync function with fallthrough is null")]
    public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.CreateStatic<string>(match => match
            .Case(fallthrough, (Action<string>)null));

        action.Should().Throw<ArgumentNullException>();
    }
}
