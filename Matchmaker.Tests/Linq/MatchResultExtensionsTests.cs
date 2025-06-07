namespace Matchmaker.Linq;

public class MatchResultExtensionsTests
{
    [Property]
    public Property GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).GetValueOrDefault() == value).ToProperty();

    [Fact]
    public void GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful() =>
        MatchResult.Failure<string>().GetValueOrDefault().Should().BeNull();

    [Property]
    public Property GetValueOrDefaultValueShouldReturnValueIfResultIsSuccessful(string value, string defaultValue) =>
        (MatchResult.Success(value).GetValueOrDefault(defaultValue) == value).ToProperty();

    [Property]
    public Property GetValueOrDefaultValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue) =>
        (MatchResult.Failure<string>().GetValueOrDefault(defaultValue) == defaultValue).ToProperty();

    [Property]
    public Property GetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
        string value,
        string defaultValue) =>
        (MatchResult.Success(value).GetValueOrDefault(() => defaultValue) == value).ToProperty();

    [Property]
    public Property GetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue) =>
        (MatchResult.Failure<string>().GetValueOrDefault(() => defaultValue) == defaultValue).ToProperty();

    [Property]
    public void GetValueOrDefaultLazyValueShouldBeLazy(string value)
    {
        var action = () => MatchResult.Success(value).GetValueOrDefault(
            () => throw new AssertionFailedException("not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(MatchResult<string> result)
    {
        var action = () => result.GetValueOrDefault((Func<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).GetValueOrThrow(() => new MatchException()) == value).ToProperty();

    [Fact]
    public void GetValueOrThrowShouldThrowIfResultIsUnsuccessful()
    {
        var action = () => MatchResult.Failure<string>().GetValueOrThrow(() => new MatchException());
        action.Should().Throw<MatchException>();
    }

    [Property]
    public void GetValueOrThrowShouldThrowIfExceptionProviderIsNull(MatchResult<string> result)
    {
        var action = () => result.GetValueOrThrow(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property SelectShouldMapValueIfResultIsSuccessful(string value, Func<string, int> mapper) =>
        (MatchResult.Success(value).Select(mapper) == MatchResult.Success(mapper(value))).ToProperty();

    [Property]
    public Property SelectShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, int> mapper) =>
        (!MatchResult.Failure<string>().Select(mapper).IsSuccessful).ToProperty();

    [Property]
    public void SelectShouldDoNothingIfResultIsUnsuccessful()
    {
        var action = () => MatchResult.Failure<string>().Select<string, int>(
            _ => throw new AssertionFailedException("select doesn't work"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void SelectShouldThrowIfMapperIsNull(MatchResult<string> result)
    {
        var action = () => result.Select<string, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property BindShouldFlatMapValueIfResultIsSuccessful(string value, Func<string, MatchResult<int>> binder) =>
        (MatchResult.Success(value).Bind(binder) == binder(value)).ToProperty();

    [Property]
    public Property BindShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, MatchResult<int>> binder) =>
        (!MatchResult.Failure<string>().Bind(binder).IsSuccessful).ToProperty();

    [Property]
    public void BindShouldDoNothingIfResultIsUnsuccessful()
    {
        var action = () => MatchResult.Failure<string>().Bind<string, int>(
            _ => throw new AssertionFailedException("bind doesn't work"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void BindShouldThrowIfBinderIsNull(MatchResult<string> result)
    {
        var action = () => result.Bind<string, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property WhereShouldFilterValueIfResultIsSuccessful(string value, Func<string, bool> predicate) =>
        (MatchResult.Success(value).Where(predicate).IsSuccessful == predicate(value)).ToProperty();

    [Property]
    public Property WhereShouldHaveSameValueIfResultIsSuccessful(string value) =>
        (MatchResult.Success(value).Where(_ => true).Value == value).ToProperty();

    [Property]
    public Property WhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, bool> predicate) =>
        (!MatchResult.Failure<string>().Where(predicate).IsSuccessful).ToProperty();

    [Property]
    public void WhereShouldDoNothingIfResultIsUnsuccessful()
    {
        var action = () => MatchResult.Failure<string>().Where(
            _ => throw new AssertionFailedException("where doesn't work"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void WhereShouldThrowIfPredicateIsNull(MatchResult<string> result)
    {
        var action = () => result.Where(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value) =>
        (MatchResult.Success<object>(value.Get).Cast<object, string>() == MatchResult.Success(value.Get))
            .ToProperty();

    [Fact]
    public Property CastShouldSucceedIfResultContainsNull() =>
        MatchResult.Success<object>(null).Cast<object, string>().IsSuccessful.ToProperty();

    [Fact]
    public Property CastToNullableValueShouldFailIfResultContainsNull() =>
        MatchResult.Success<object>(null).Cast<object, int?>().IsSuccessful.ToProperty();

    [Fact]
    public Property CastToValueShouldFailIfResultContainsNull() =>
        (!MatchResult.Success<object>(null).Cast<object, int>().IsSuccessful).ToProperty();

    [Property]
    public Property CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value) =>
        (!MatchResult.Success<object>(value).Cast<object, int>().IsSuccessful).ToProperty();

    [Fact]
    public Property CastShouldBeUnsuccessfulIfResultIsUnsuccessful() =>
        (!MatchResult.Failure<object>().Cast<object, string>().IsSuccessful).ToProperty();

    [Property]
    public void DoShouldPerformActionIfResultIsSuccessful(string value)
    {
        int count = 0;
        MatchResult.Success(value).Do(_ => count++);
        count.Should().Be(1);
    }

    [Fact]
    public void DoShouldNotPerformActionIfResultIsUnsuccessful()
    {
        int count = 0;
        MatchResult.Failure<string>().Do(_ => count++);
        count.Should().Be(0);
    }

    [Property]
    public Property DoShouldReturnSameResult(MatchResult<string> result) =>
        (result == result.Do(_ => { })).ToProperty();

    [Property]
    public void DoShouldPassValueIfResultIsSuccessful(string value)
    {
        string actualValue = null;
        MatchResult.Success(value).Do(v => actualValue = v);
        actualValue.Should().Be(value);
    }

    [Property]
    public void DoShouldThrowIfActionIsNull(MatchResult<string> result)
    {
        var action = () => result.Do(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
