namespace Matchmaker.Linq;

public class AsyncMatchResultExtensionsTests
{
    [Property(DisplayName = "GetValueOrDefault should return the value if result is successful")]
    public async Task<Property> GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrDefault() == value).ToProperty();

    [Fact(DisplayName = "GetValueOrDefault should return the default if result is unsuccessful")]
    public async Task GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful() =>
        (await Task.FromResult(MatchResult.Failure<string>())).GetValueOrDefault().Should().BeNull();

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
        var action = () => Task.FromResult(MatchResult.Success(value))
            .GetValueOrDefault((Func<string>)(() => throw new AssertionFailedException("not lazy")));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy async GetValueOrDefault should be lazy")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldBeLazy(string value)
    {
        var action = () => Task.FromResult(MatchResult.Success(value))
            .GetValueOrDefault((Func<Task<string>>)(() => throw new AssertionFailedException("not lazy")));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Fact(DisplayName = "GetValueOrDefault should throw if result task is null")]
    public async Task GetValueOrDefaultShouldThrowIfResultTaskIsNull()
    {
        var action = () => ((Task<MatchResult<string>>)null).GetValueOrDefault();
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "GetValueOrDefault with value should throw if result task is null")]
    public async Task GetValueOrDefaultValueShouldThrowIfResultTaskIsNull(string value)
    {
        var action = () => ((Task<MatchResult<string>>)null).GetValueOrDefault(value);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GetValueOrDefault should throw if result task is null")]
    public async Task GetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value)
    {
        var action = () => ((Task<MatchResult<string>>)null).GetValueOrDefault(() => value);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy async GetValueOrDefault should throw if result task is null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value)
    {
        var action = () => ((Task<MatchResult<string>>)null).GetValueOrDefault(() => Task.FromResult(value));
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GetValueOrDefault should throw if value provider is null")]
    public async Task GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.GetValueOrDefault((Func<string>)null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy async GetValueOrDefault should throw if value provider is null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.GetValueOrDefault((Func<Task<string>>)null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy async GetValueOrDefault should throw if value provider returns null")]
    public async Task AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderReturnsNull()
    {
        var action = () => Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => (Task<string>)null);
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Property(DisplayName = "GetValueOrThrow should return value if result is successful")]
    public async Task<Property> GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value) =>
        (await Task.FromResult(MatchResult.Success(value)).GetValueOrThrow(() => new MatchException()) == value)
            .ToProperty();

    [Fact(DisplayName = "GetValueOrThrow should throw if result is unsuccessful")]
    public async Task GetValueOrThrowShouldThrowIfResultIsUnsuccessful()
    {
        var action = () => Task.FromResult(MatchResult.Failure<string>()).GetValueOrThrow(() => new MatchException());
        await action.Should().ThrowAsync<MatchException>();
    }

    [Fact(DisplayName = "GetValueOrThrow should throw if result task is null")]
    public async Task GetValueOrThrowShouldThrowIfResultTaskIsNull()
    {
        var action = () => ((Task<MatchResult<string>>)null).GetValueOrThrow(() => new MatchException());
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "GetValueOrThrow should throw if exception provider is null")]
    public async Task GetValueOrThrowShouldThrowIfExceptionProviderIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.GetValueOrThrow(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

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
        var action = () => Task.FromResult(MatchResult.Failure<string>())
            .Select<string, int>(_ => throw new AssertionFailedException("select doesn't work"));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Property(DisplayName = "Select should throw if task result is null")]
    public async Task SelectShouldThrowIfTaskResultIsNull(Func<string, int> mapper)
    {
        var action = () => ((Task<MatchResult<string>>)null).Select(mapper);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Select should throw if mapper is null")]
    public async Task SelectShouldThrowIfMapperIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.Select<string, int>(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

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
        var action = () => Task.FromResult(MatchResult.Failure<string>())
            .Bind<string, int>(_ => throw new AssertionFailedException("bind doesn't work"));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Property(DisplayName = "Bind should throw if task result is null")]
    public async Task BindShouldThrowIfTaskResultIsNull(Func<string, Task<MatchResult<int>>> binder)
    {
        var action = () => ((Task<MatchResult<string>>)null).Bind(binder);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Bind should throw if binder is null")]
    public async Task BindShouldThrowIfBinderIsNull(MatchResult<string> result)
    {
        var action = () => Task.FromResult(result).Bind<string, int>(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Bind should throw if binder returns null")]
    public async Task BindShouldThrowIfBinderReturnsNull(string value)
    {
        var action = () => Task.FromResult(MatchResult.Success(value)).Bind<string, int>(_ => null);
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

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
        var action = () => Task.FromResult(MatchResult.Failure<string>())
            .Where((Func<string, bool>)(_ => throw new AssertionFailedException("where doesn't work")));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Property(DisplayName = "Async Where should do nothing if result is unsuccessful")]
    public async Task AsyncWhereShouldDoNothingIfResultIsUnsuccessful()
    {
        var action = () => Task.FromResult(MatchResult.Failure<string>())
            .Where((Func<string, Task<bool>>)(_ => throw new AssertionFailedException("where doesn't work")));

        await action.Should().NotThrowAsync<AssertionFailedException>();
    }

    [Property(DisplayName = "Where should throw if task result is null")]
    public async Task WhereShouldThrowIfTaskResultIsNull(Func<string, bool> predicate)
    {
        var action = () => ((Task<MatchResult<string>>)null).Where(predicate);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Async Where should throw if task result is null")]
    public async Task AsyncWhereShouldThrowIfTaskResultIsNull(Func<string, Task<bool>> predicate)
    {
        var action = () => ((Task<MatchResult<string>>)null).Where(predicate);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Where should throw if predicate is null")]
    public async Task WhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.Where((Func<string, bool>)null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Async Where should throw if predicate is null")]
    public async Task AsyncWhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.Where((Func<string, Task<bool>>)null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Async Where should throw if predicate returns null")]
    public async Task AsyncWhereShouldThrowIfPredicateReturnsNull(string value)
    {
        var action = () => Task.FromResult(MatchResult.Success(value)).Where(_ => null);
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Property(DisplayName = "Cast should cast value of correct type if result is successful")]
    public async Task<Property> CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value) =>
        (await Task.FromResult(MatchResult.Success<object>(value.Get)).Cast<object, string>() ==
            MatchResult.Success(value.Get))
            .ToProperty();

    [Fact(DisplayName = "Cast should succeed if result contains null")]
    public async Task CastShouldSucceedIfResultContainsNull() =>
        (await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, string>())
            .IsSuccessful.Should().Be(true);

    [Fact(DisplayName = "Cast to nullable value should succeed if result contains null")]
    public async Task CastToNullableValueShouldSucceedIfResultContainsNull() =>
        (await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int?>()).IsSuccessful.Should().BeTrue();

    [Fact(DisplayName = "Cast to value should fail if result contains null")]
    public async Task CastToValueShouldFailIfResultContainsNull() =>
        (await Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int>()).IsSuccessful.Should().BeFalse();

    [Property(DisplayName = "Cast should fail if value has incorrect type and result is successful")]
    public async Task<Property> CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value) =>
        (!(await Task.FromResult(MatchResult.Success<object>(value)).Cast<object, int>()).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "Cast should be unsuccessful if result is unsuccessful")]
    public async Task CastShouldBeUnsuccessfulIfResultIsUnsuccessful() =>
        (await Task.FromResult(MatchResult.Failure<object>()).Cast<object, string>()).IsSuccessful.Should().BeFalse();

    [Fact(DisplayName = "Cast should throw if task result is null")]
    public async Task CastShouldThrowIfTaskResultIsNull()
    {
        var action = async () => await ((Task<MatchResult<object>>)null).Cast<object, string>();
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Do should perform action if result is successful")]
    public async Task DoShouldPerformActionIfResultIsSuccessful(string value)
    {
        int count = 0;
        await Task.FromResult(MatchResult.Success(value)).Do(_ => count++);
        count.Should().Be(1);
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
        count.Should().Be(0);
    }

    [Fact(DisplayName = "Async Do should not perform action if result is unsuccessful")]
    public async Task AsyncDoShouldNotPerformActionIfResultIsUnsuccessful()
    {
        int count = 0;
        await Task.FromResult(MatchResult.Failure<string>()).Do(_ => Task.FromResult(count++));
        count.Should().Be(0);
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
    public async Task DoShouldThrowIfTaskResultIsNull()
    {
        var action = () => ((Task<MatchResult<object>>)null).Do(_ => { });
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "Async Do should throw if task result is null")]
    public async Task AsyncDoShouldThrowIfTaskResultIsNull()
    {
        var action = () => ((Task<MatchResult<object>>)null).Do(_ => Task.CompletedTask);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Do should throw if action is null")]
    public async Task DoShouldThrowIfActionIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.Do((Action<string>)null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Async Do should throw if action is null")]
    public async Task AsyncDoShouldThrowIfActionIsNull(Task<MatchResult<string>> result)
    {
        var action = () => result.Do(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Property(DisplayName = "Async Do should throw if action returns null")]
    public async Task AsyncDoShouldThrowIfActionReturnsNull(string value)
    {
        var action = () => Task.FromResult(MatchResult.Success(value)).Do(_ => null);
        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
