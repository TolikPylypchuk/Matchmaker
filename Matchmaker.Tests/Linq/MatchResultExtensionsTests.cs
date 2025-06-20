namespace Matchmaker.Linq;

public class MatchResultExtensionsTests
{
    [Property(DisplayName = "GetValueOrDefault should return the value if result is successful")]
    public Property GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).GetValueOrDefault() == value).ToProperty();

    [Fact(DisplayName = "GetValueOrDefault should return the default if result is unsuccessful")]
    public void GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful() =>
        Assert.Null(MatchResult.Failure<string>().GetValueOrDefault());

    [Property(DisplayName = "GetValueOrDefault with value should return the value if result is successful")]
    public Property GetValueOrDefaultValueShouldReturnValueIfResultIsSuccessful(string value, string defaultValue) =>
        (MatchResult.Success(value).GetValueOrDefault(defaultValue) == value).ToProperty();

    [Property(DisplayName = "GetValueOrDefault with value should return the specified value if result is unsuccessful")]
    public Property GetValueOrDefaultValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue) =>
        (MatchResult.Failure<string>().GetValueOrDefault(defaultValue) == defaultValue).ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should return the value if result is successful")]
    public Property GetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
        string value,
        string defaultValue) =>
        (MatchResult.Success(value).GetValueOrDefault(() => defaultValue) == value).ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should return the specified value if result is unsuccessful")]
    public Property GetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue) =>
        (MatchResult.Failure<string>().GetValueOrDefault(() => defaultValue) == defaultValue).ToProperty();

    [Property(DisplayName = "Lazy GetValueOrDefault should be lazy")]
    public void GetValueOrDefaultLazyValueShouldBeLazy(string value)
    {
        var exception = Record.Exception(() =>
            MatchResult.Success(value).GetValueOrDefault(() => throw new InvalidOperationException("not lazy")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GetValueOrDefault should throw if value provider is null")]
    public void GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.GetValueOrDefault((Func<string>)null));

    [Property(DisplayName = "GetValueOrThrow should return value if result is successful")]
    public Property GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).GetValueOrThrow(() => new MatchException()) == value).ToProperty();

    [Fact(DisplayName = "GetValueOrThrow should throw if result is unsuccessful")]
    public void GetValueOrThrowShouldThrowIfResultIsUnsuccessful() =>
        Assert.Throws<MatchException>(() =>
            MatchResult.Failure<string>().GetValueOrThrow(() => new MatchException()));

    [Property(DisplayName = "GetValueOrThrow should throw if exception provider is null")]
    public void GetValueOrThrowShouldThrowIfExceptionProviderIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.GetValueOrThrow(null));

    [Property(DisplayName = "Select should map value if result is successful")]
    public Property SelectShouldMapValueIfResultIsSuccessful(string value, Func<string, int> mapper) =>
        (MatchResult.Success(value).Select(mapper) == MatchResult.Success(mapper(value))).ToProperty();

    [Property(DisplayName = "Select should be unsuccessful if result is unsuccessful")]
    public Property SelectShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, int> mapper) =>
        (!MatchResult.Failure<string>().Select(mapper).IsSuccessful).ToProperty();

    [Property(DisplayName = "Select should do nothing if result is unsuccessful")]
    public void SelectShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = Record.Exception(() =>
            MatchResult.Failure<string>()
                .Select<string, int>(_ => throw new InvalidOperationException("select doesn't work")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Select should throw if mapper is null")]
    public void SelectShouldThrowIfMapperIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.Select<string, int>(null));

    [Property(DisplayName = "Bind should flat-map value if result is successful")]
    public Property BindShouldFlatMapValueIfResultIsSuccessful(string value, Func<string, MatchResult<int>> binder) =>
        (MatchResult.Success(value).Bind(binder) == binder(value)).ToProperty();

    [Property(DisplayName = "Bind should be unsuccessful if result is unsuccessful")]
    public Property BindShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, MatchResult<int>> binder) =>
        (!MatchResult.Failure<string>().Bind(binder).IsSuccessful).ToProperty();

    [Property(DisplayName = "Bind should do nothing if result is unsuccessful")]
    public void BindShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = Record.Exception(() =>
            MatchResult.Failure<string>()
                .Bind<string, int>(_ => throw new InvalidOperationException("bind doesn't work")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Bind should throw if binder is null")]
    public void BindShouldThrowIfBinderIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.Bind<string, int>(null));

    [Property(DisplayName = "Where should filter value if result is successful")]
    public Property WhereShouldFilterValueIfResultIsSuccessful(string value, Func<string, bool> predicate) =>
        (MatchResult.Success(value).Where(predicate).IsSuccessful == predicate(value)).ToProperty();

    [Property(DisplayName = "Where should have the same value if result is successful")]
    public Property WhereShouldHaveSameValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).Where(_ => true).Value == value).ToProperty();

    [Property(DisplayName = "Where should be unsuccessful if result is unsuccessful")]
    public Property WhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, bool> predicate) =>
        (!MatchResult.Failure<string>().Where(predicate).IsSuccessful).ToProperty();

    [Property(DisplayName = "Where should do nothing if result is unsuccessful")]
    public void WhereShouldDoNothingIfResultIsUnsuccessful()
    {
        var exception = Record.Exception(() =>
            MatchResult.Failure<string>()
                .Where(_ => throw new InvalidOperationException("where doesn't work")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Where should throw if predicate is null")]
    public void WhereShouldThrowIfPredicateIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.Where(null));

    [Property(DisplayName = "Cast should cast value of correct type if result is successful")]
    public Property CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value) =>
        (MatchResult.Success<object>(value.Get).Cast<object, string>() == MatchResult.Success(value.Get))
            .ToProperty();

    [Fact(DisplayName = "Cast should succeed if result contains null")]
    public void CastShouldSucceedIfResultContainsNull() =>
        Assert.True(MatchResult.Success<object>(null).Cast<object, string>().IsSuccessful);

    [Fact(DisplayName = "Cast to nullable value should succeed if result contains null")]
    public void CastToNullableValueShouldSucceedfResultContainsNull() =>
        Assert.True(MatchResult.Success<object>(null).Cast<object, int?>().IsSuccessful);

    [Fact(DisplayName = "Cast to value should fail if result contains null")]
    public void CastToValueShouldFailIfResultContainsNull() =>
        Assert.False(MatchResult.Success<object>(null).Cast<object, int>().IsSuccessful);

    [Property(DisplayName = "Cast should fail if value has incorrect type and result is successful")]
    public Property CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value) =>
        (!MatchResult.Success<object>(value).Cast<object, int>().IsSuccessful).ToProperty();

    [Fact(DisplayName = "Cast should be unsuccessful if result is unsuccessful")]
    public void CastShouldBeUnsuccessfulIfResultIsUnsuccessful() =>
        Assert.False(MatchResult.Failure<object>().Cast<object, string>().IsSuccessful);

    [Property(DisplayName = "Do should perform action if result is successful")]
    public void DoShouldPerformActionIfResultIsSuccessful(string value)
    {
        int count = 0;
        MatchResult.Success(value).Do(_ => count++);
        Assert.Equal(1, count);
    }

    [Fact(DisplayName = "Do should not perform action if result is unsuccessful")]
    public void DoShouldNotPerformActionIfResultIsUnsuccessful()
    {
        int count = 0;
        MatchResult.Failure<string>().Do(_ => count++);
        Assert.Equal(0, count);
    }

    [Property(DisplayName = "Do should return the same result")]
    public Property DoShouldReturnSameResult(MatchResult<string> result) =>
        (result == result.Do(_ => { })).ToProperty();

    [Property(DisplayName = "Do should pass value if result is successful")]
    public void DoShouldPassValueIfResultIsSuccessful(string value)
    {
        string actualValue = null;
        MatchResult.Success(value).Do(v => actualValue = v);
        Assert.Equal(value, actualValue);
    }

    [Property(DisplayName = "Do should throw if action is null")]
    public void DoShouldThrowIfActionIsNull(MatchResult<string> result) =>
        Assert.Throws<ArgumentNullException>(() => result.Do(null));
}
