namespace Matchmaker.Linq;

public class AsyncMatchResultExtensionsTests
{
    [Property(DisplayName = "GetValueOrDefault should return the value if result is successful")]
    public async Task<Property> GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrDefault() == value).ToProperty();

    [Fact(DisplayName = "GetValueOrDefault should return the default if result is unsuccessful")]
    public async Task GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful() =>
        Assert.Null((await Task.FromResult(MatchResult.Failure<string>())).GetValueOrDefault());

    [Property(DisplayName = "GetValueOrDefault with value should return the value if result is successful")]
    public async Task<Property> GetValueOrDefaultValueShouldReturnValueIfResultIsSuccessful(
        string value,
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrDefault(defaultValue) == value)
            .ToProperty();

    [Property(DisplayName = "GetValueOrDefault with value should return the specified value if result is unsuccessful")]
    public async Task<Property> GetValueOrDefaultValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(defaultValue) == defaultValue)
            .ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should return the value if result is successful")]
    public async Task<Property> GetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
        string value,
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrDefault(() => defaultValue) == value)
            .ToProperty();

    [Property(DisplayName = "Lazy async GetValueOrDefault should return the value if result is successful")]
    public async Task<Property> AsyncGetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
        string value,
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Success(value))
            .GetValueOrDefault(() => Task.FromResult(defaultValue)) == value)
            .ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should return the specified value if result is unsuccessful")]
    public async Task<Property> GetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => defaultValue) == defaultValue)
            .ToProperty();

    [Property(DisplayName = "Lazy async GetValueOrDefault should return the specified value if result is unsuccessful")]
    public async Task<Property> AsyncGetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(
        string defaultValue) =>
        (await Task.FromResult(MatchResult.Failure<string>())
            .GetValueOrDefault(() => Task.FromResult(defaultValue)) == defaultValue)
            .ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should be lazy")]
    public async Task GetValueOrDefaultLazyValueShouldBeLazy(string value)
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Success(value))
                .GetValueOrDefault((Func<string>)(() => throw new InvalidOperationException("not lazy"))));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy async GetValueOrDefault should be lazy")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldBeLazy(string value)
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Success(value))
                .GetValueOrDefault((Func<Task<string>>)(() => throw new InvalidOperationException("not lazy"))));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "GetValueOrDefault should throw if result task is null")]
    public async Task GetValueOrDefaultShouldThrowIfResultTaskIsNull() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null).GetValueOrDefault());

    [Property(DisplayName = "GetValueOrDefault with value should throw if result task is null")]
    public async Task GetValueOrDefaultValueShouldThrowIfResultTaskIsNull(string value) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((Task<MatchResult<string>>)null).GetValueOrDefault(value));

    [Property(DisplayName = "Lazy GetValueOrDefault should throw if result task is null")]
    public async Task GetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null)
            .GetValueOrDefault(() => value));

    [Property(DisplayName = "Lazy async GetValueOrDefault should throw if result task is null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((Task<MatchResult<string>>)null).GetValueOrDefault(() => Task.FromResult(value)));

    [Property(DisplayName = "Lazy GetValueOrDefault should throw if value provider is null")]
    public async Task GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.GetValueOrDefault((Func<string>)null));

    [Property(DisplayName = "Lazy async GetValueOrDefault should throw if value provider is null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(
        Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.GetValueOrDefault((Func<Task<string>>)null));

    [Fact(DisplayName = "Lazy async GetValueOrDefault should throw if value provider returns null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderReturnsNull() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => (Task<string>)null));

    [Property(DisplayName = "GetValueOrThrow should return value if result is successful")]
    public async Task<Property> GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrThrow(() => new MatchException()) == value)
            .ToProperty();

    [Fact(DisplayName = "GetValueOrThrow should throw if result is unsuccessful")]
    public async Task GetValueOrThrowShouldThrowIfResultIsUnsuccessful() =>
        await Assert.ThrowsAsync<MatchException>(() =>
            Task.FromResult(MatchResult.Failure<string>()).GetValueOrThrow(() => new MatchException()));

    [Fact(DisplayName = "GetValueOrThrow should throw if result task is null")]
    public async Task GetValueOrThrowShouldThrowIfResultTaskIsNull() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((Task<MatchResult<string>>)null).GetValueOrThrow(() => new MatchException()));

    [Property(DisplayName = "GetValueOrThrow should throw if exception provider is null")]
    public async Task GetValueOrThrowShouldThrowIfExceptionProviderIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.GetValueOrThrow(null));

    [Property(DisplayName = "Select should map value if result is successful")]
    public async Task<Property> SelectShouldMapValueIfResultIsSuccessful(string value, Func<string, int> mapper) =>
        (await Task.FromResult(MatchResult.Success(value)).Select(mapper) == MatchResult.Success(mapper(value)))
            .ToProperty();

    [Property(DisplayName = "Select should be unsuccessful if result is unsuccessful")]
    public async Task<Property> SelectShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, int> mapper) =>
        (!(await Task.FromResult(MatchResult.Failure<string>()).Select(mapper)).IsSuccessful).ToProperty();

    [Property(DisplayName = "Select should do nothing if result is unsuccessful")]
    public async Task SelectShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Failure<string>())
                .Select<string, int>(_ => throw new InvalidOperationException("select doesn't work")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Select should throw if task result is null")]
    public async Task SelectShouldThrowIfTaskResultIsNull(Func<string, int> mapper) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null).Select(mapper));

    [Property(DisplayName = "Select should throw if mapper is null")]
    public async Task SelectShouldThrowIfMapperIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.Select<string, int>(null));

    [Property(DisplayName = "Bind should flat-map value if result is successful")]
    public async Task<Property> BindShouldFlatMapValueIfResultIsSuccessful(
        string value,
        Func<string, Task<MatchResult<int>>> binder) =>
        (await Task.FromResult(MatchResult.Success(value)).Bind(binder) == await binder(value)).ToProperty();

    [Property(DisplayName = "Bind should be unsuccessful if result is unsuccessful")]
    public async Task<Property> BindShouldBeUnsuccessfulIfResultIsUnsuccessful(
        Func<string, Task<MatchResult<int>>> binder) =>
        (!(await Task.FromResult(MatchResult.Failure<string>()).Bind(binder)).IsSuccessful).ToProperty();

    [Property(DisplayName = "Bind should do nothing if result is unsuccessful")]
    public async Task BindShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Failure<string>())
                .Bind<string, int>(_ => throw new InvalidOperationException("bind doesn't work")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Bind should throw if task result is null")]
    public async Task BindShouldThrowIfTaskResultIsNull(Func<string, Task<MatchResult<int>>> binder) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null).Bind(binder));

    [Property(DisplayName = "Bind should throw if binder is null")]
    public async Task BindShouldThrowIfBinderIsNull(MatchResult<string> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(result).Bind<string, int>(null));

    [Property(DisplayName = "Bind should throw if binder returns null")]
    public async Task BindShouldThrowIfBinderReturnsNull(string value) =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.FromResult(MatchResult.Success(value)).Bind<string, int>(_ => null));

    [Property(DisplayName = "Where should filter value if result is successful")]
    public async Task<Property> WhereShouldFilterValueIfResultIsSuccessful(
        string value,
        Func<string, bool> predicate) =>
        ((await Task.FromResult(MatchResult.Success(value)).Where(predicate)).IsSuccessful == predicate(value))
            .ToProperty();

    [Property(DisplayName = "Async where should filter value if result is successful")]
    public async Task<Property> AsyncWhereShouldFilterValueIfResultIsSuccessful(
        string value,
        Func<string, Task<bool>> predicate) =>
        ((await Task.FromResult(MatchResult.Success(value)).Where(predicate)).IsSuccessful == (await predicate(value)))
            .ToProperty();

    [Property(DisplayName = "Where should have the same value if result is successful")]
    public async Task<Property> WhereShouldHaveSameValueIfResultIsSuccessful(string value) =>
        ((await Task.FromResult(MatchResult.Success(value)).Where(_ => true)).Value == value).ToProperty();

    [Property(DisplayName = "Async Where should have the same value if result is successful")]
    public async Task<Property> AsyncWhereShouldHaveSameValueIfResultIsSuccessful(string value) =>
        ((await Task.FromResult(MatchResult.Success(value)).Where(_ => Task.FromResult(true))).Value == value)
            .ToProperty();

    [Property(DisplayName = "Where should be unsuccessful if result is unsuccessful")]
    public async Task<Property> WhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, bool> predicate) =>
        (!(await Task.FromResult(MatchResult.Failure<string>())).Where(predicate).IsSuccessful).ToProperty();

    [Property(DisplayName = "Async Where should be unsuccessful if result is unsuccessful")]
    public async Task<Property> AsyncWhereShouldBeUnsuccessfulIfResultIsUnsuccessful(
        Func<string, Task<bool>> predicate) =>
        (!(await Task.FromResult(MatchResult.Failure<string>()).Where(predicate)).IsSuccessful).ToProperty();

    [Property(DisplayName = "Where should do nothing if result is unsuccessful")]
    public async Task WhereShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Failure<string>())
                .Where((Func<string, bool>)(_ => throw new InvalidOperationException("where doesn't work"))));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Async Where should do nothing if result is unsuccessful")]
    public async Task AsyncWhereShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = await Record.ExceptionAsync(() =>
            Task.FromResult(MatchResult.Failure<string>())
                .Where((Func<string, Task<bool>>)(_ => throw new InvalidOperationException("where doesn't work"))));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Where should throw if task result is null")]
    public async Task WhereShouldThrowIfTaskResultIsNull(Func<string, bool> predicate) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null).Where(predicate));

    [Property(DisplayName = "Async Where should throw if task result is null")]
    public async Task AsyncWhereShouldThrowIfTaskResultIsNull(Func<string, Task<bool>> predicate) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<string>>)null).Where(predicate));

    [Property(DisplayName = "Where should throw if predicate is null")]
    public async Task WhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.Where((Func<string, bool>)null));

    [Property(DisplayName = "Async Where should throw if predicate is null")]
    public async Task AsyncWhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.Where((Func<string, Task<bool>>)null));

    [Property(DisplayName = "Async Where should throw if predicate returns null")]
    public async Task AsyncWhereShouldThrowIfPredicateReturnsNull(string value) =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.FromResult(MatchResult.Success(value)).Where(_ => null));

    [Property(DisplayName = "Cast should cast value of correct type if result is successful")]
    public async Task<Property> CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value) =>
        (await Task.FromResult(MatchResult.Success<object>(value.Get)).Cast<object, string>() ==
            MatchResult.Success(value.Get))
            .ToProperty();

    [Fact(DisplayName = "Cast should succeed if result contains null")]
    public async Task CastShouldSucceedIfResultContainsNull() =>
        Assert.True((await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, string>()).IsSuccessful);

    [Fact(DisplayName = "Cast to nullable value should succeed if result contains null")]
    public async Task CastToNullableValueShouldSucceedIfResultContainsNull() =>
        Assert.True((await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int?>()).IsSuccessful);

    [Fact(DisplayName = "Cast to value should fail if result contains null")]
    public async Task CastToValueShouldFailIfResultContainsNull() =>
        Assert.False((await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int>()).IsSuccessful);

    [Property(DisplayName = "Cast should fail if value has incorrect type and result is successful")]
    public async Task<Property> CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value) =>
        (!(await Task.FromResult(MatchResult.Success<object>(value)).Cast<object, int>()).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "Cast should be unsuccessful if result is unsuccessful")]
    public async Task CastShouldBeUnsuccessfulIfResultIsUnsuccessful() =>
        Assert.False((await Task.FromResult(MatchResult.Failure<object>()).Cast<object, string>()).IsSuccessful);

    [Fact(DisplayName = "Cast should throw if task result is null")]
    public async Task CastShouldThrowIfTaskResultIsNull() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<object>>)null).Cast<object, string>());

    [Property(DisplayName = "Do should perform action if result is successful")]
    public async Task DoShouldPerformActionIfResultIsSuccessful(string value)
    {
        int count = 0;
        await Task.FromResult(MatchResult.Success(value)).Do(_ => count++);
        Assert.Equal(1, count);
    }

    [Property(DisplayName = "Async Do should perform action if result is successful")]
    public async Task<Property> AsyncDoShouldPerformActionIfResultIsSuccessful(string value)
    {
        int count = 0;
        await Task.FromResult(MatchResult.Success(value)).Do(_ => Task.FromResult(count++));
        return (count == 1).ToProperty();
    }

    [Fact(DisplayName = "Do should not perform action if result is unsuccessful")]
    public async Task DoShouldNotPerformActionIfResultIsUnsuccessful()
    {
        int count = 0;
        await Task.FromResult(MatchResult.Failure<string>()).Do(_ => count++);
        Assert.Equal(0, count);
    }

    [Fact(DisplayName = "Async Do should not perform action if result is unsuccessful")]
    public async Task AsyncDoShouldNotPerformActionIfResultIsUnsuccessful()
    {
        int count = 0;
        await Task.FromResult(MatchResult.Failure<string>()).Do(_ => Task.FromResult(count++));
        Assert.Equal(0, count);
    }

    [Property(DisplayName = "Do should return the same result")]
    public async Task<Property> DoShouldReturnSameResult(Task<MatchResult<string>> result) =>
        (await result == await result.Do(_ => { })).ToProperty();

    [Property(DisplayName = "Async Do should return the same result")]
    public async Task<Property> AsyncDoShouldReturnSameResult(Task<MatchResult<string>> result) =>
        (await result == await result.Do(_ => Task.CompletedTask)).ToProperty();

    [Property(DisplayName = "Do should pass value if result is successful")]
    public async Task<Property> DoShouldPassValueIfResultIsSuccessful(string value)
    {
        string actualValue = null;
        await Task.FromResult(MatchResult.Success(value)).Do(v => actualValue = v);
        return (actualValue == value).ToProperty();
    }

    [Property(DisplayName = "Async Do should pass value if result is successful")]
    public async Task<Property> AsyncDoShouldPassValueIfResultIsSuccessful(string value)
    {
        string actualValue = null;
        await Task.FromResult(MatchResult.Success(value)).Do(v => Task.FromResult(actualValue = v));
        return (actualValue == value).ToProperty();
    }

    [Fact(DisplayName = "Do should throw if task result is null")]
    public async Task DoShouldThrowIfTaskResultIsNull() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => ((Task<MatchResult<object>>)null).Do(_ => { }));

    [Fact(DisplayName = "Async Do should throw if task result is null")]
    public async Task AsyncDoShouldThrowIfTaskResultIsNull() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((Task<MatchResult<object>>)null).Do(_ => Task.CompletedTask));

    [Property(DisplayName = "Do should throw if action is null")]
    public async Task DoShouldThrowIfActionIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.Do((Action<string>)null));

    [Property(DisplayName = "Async Do should throw if action is null")]
    public async Task AsyncDoShouldThrowIfActionIsNull(Task<MatchResult<string>> result) =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.Do(null));

    [Property(DisplayName = "Async Do should throw if action returns null")]
    public async Task AsyncDoShouldThrowIfActionReturnsNull(string value) =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.FromResult(MatchResult.Success(value)).Do(_ => null));
}
