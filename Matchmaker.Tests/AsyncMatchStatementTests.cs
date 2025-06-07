namespace Matchmaker;

public class AsyncMatchStatementTests
{
    [Fact]
    public void MatchCreateShouldNeverReturnNull() =>
        AsyncMatch.Create<int>()
            .Should()
            .NotBeNull();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault) =>
        AsyncMatch.Create<int>(fallthroughByDefault)
            .Should()
            .NotBeNull();

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ =>
            {
                matchSuccessful = true;
                return Task.CompletedTask;
            })
            .Case(AsyncPattern.Any<string>(), _ =>
            {
                matchSuccessful = false;
                return Task.CompletedTask;
            })
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
            .ExecuteAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ =>
            {
                matchSuccessful = true;
                return Task.CompletedTask;
            })
            .Case(Pattern.Any<string>(), _ =>
            {
                matchSuccessful = false;
                return Task.CompletedTask;
            })
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
        Func<string, bool> predicate,
        string value)
    {
        var pattern = Pattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
            .ExecuteAsync(value);

        return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
            .ExecuteNonStrictAsync(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
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
    public async Task<Property> NonStrictMatchShouldReturnFalseIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matched = await AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
            .ExecuteNonStrictAsync(value);

        return (matched == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
            .ExecuteNonStrictAsync(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => count++)
            .Case(AsyncPattern.Any<string>(), _ => count++)
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        int count = 0;

        _ = AsyncMatch.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => count++)
            .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++)
            .ExecuteWithFallthroughAsync(value);

        return (count == 0).ToProperty();
    }

    [Property]
    public async Task<Property> MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ExecuteWithFallthroughAsync(value)
            .CountAsync();

        return (result == 0).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
            .ToFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task<Property> NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        bool matchSuccessful = false;

        await AsyncMatch.Create<string>()
            .Case(pattern, _ => matchSuccessful = true)
            .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
            .ToNonStrictFunction()(value);

        return (matchSuccessful == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
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
    public async Task<Property> NonStrictMatchToFunctionShouldReturnFalseIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        bool matched = await AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
            .ToNonStrictFunction()(value);

        return (matched == (await pattern.MatchAsync(value)).IsSuccessful).ToProperty();
    }

    [Property]
    public async Task NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, _ => { })
            .ToNonStrictFunction()(value);

        await action.Should().NotThrowAsync<MatchException>();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, fallthrough: false, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
        Func<string, Task<bool>> predicate,
        string value)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);
        int matchCount = 0;

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: false)
            .Case(pattern, fallthrough: true, _ => { matchCount++; })
            .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (await pattern.MatchAsync(value)).IsSuccessful
            ? (result == 2 && matchCount == 2).ToProperty()
            : (result == 1 && matchCount == 1).ToProperty();
    }

    [Property]
    public async Task<Property> MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
    {
        var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

        int result = await AsyncMatch.Create<string>(fallthroughByDefault: true)
            .Case(pattern, _ => { })
            .ToFunctionWithFallthrough()(value)
            .CountAsync();

        return (result == 0).ToProperty();
    }

    [Fact]
    public void MatchShouldThrowIfAsyncPatternIsNull()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IAsyncPattern<string, string>)null, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IPattern<string, string>)null, _ => Task.CompletedTask);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IAsyncPattern<string, string>)null, _ => Task.CompletedTask);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IPattern<string, string>)null, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((Func<string, Task>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((Action<string>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.CompletedTask);

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IAsyncPattern<string, string>)null, fallthrough, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IPattern<string, string>)null, fallthrough, _ => Task.CompletedTask);

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case((IPattern<string, string>)null, fallthrough, _ => { });

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, fallthrough, null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
        Func<string, Task<bool>> predicate,
        bool fallthrough)
    {
        var pattern = AsyncPattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, fallthrough, (Action<string>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, fallthrough, null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
        Func<string, bool> predicate,
        bool fallthrough)
    {
        var pattern = Pattern.CreatePattern(predicate);

        var action = () => AsyncMatch.Create<string>()
            .Case(pattern, fallthrough, (Action<string>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case(fallthrough, (Func<string, Task>)null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
    {
        var action = () => AsyncMatch.Create<string>()
            .Case(fallthrough, (Action<string>)null);

        action.Should().Throw<ArgumentNullException>();
    }
}
