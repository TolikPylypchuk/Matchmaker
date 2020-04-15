using System;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Linq
{
    public class MatchResultExtensionsTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value)
            => (MatchResult.Success(value).GetValueOrDefault() == value).ToProperty();

        [Fact]
        public void GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful()
            => MatchResult.Failure<string>().GetValueOrDefault().Should().BeNull();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultValueShouldReturnValueIfResultIsSuccessful(string value, string defaultValue)
            => (MatchResult.Success(value).GetValueOrDefault(defaultValue) == value).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue)
            => (MatchResult.Failure<string>().GetValueOrDefault(defaultValue) == defaultValue).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
            string value,
            string defaultValue)
            => (MatchResult.Success(value).GetValueOrDefault(() => defaultValue) == value).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue)
            => (MatchResult.Failure<string>().GetValueOrDefault(() => defaultValue) == defaultValue).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GetValueOrDefaultLazyValueShouldBeLazy(string value)
        {
            Action action = () => MatchResult.Success(value).GetValueOrDefault(
                () => throw new AssertionFailedException("not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(MatchResult<string> result)
        {
            Action action = () => result.GetValueOrDefault((Func<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value)
            => (MatchResult.Success(value).GetValueOrThrow(() => new MatchException()) == value).ToProperty();

        [Fact]
        public void GetValueOrThrowShouldThrowIfResultIsUnsuccessful()
        {
            Action action = () => MatchResult.Failure<string>().GetValueOrThrow(() => new MatchException());
            action.Should().Throw<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectShouldMapValueIfResultIsSuccessful(string value, Func<string, int> mapper)
            => (MatchResult.Success(value).Select(mapper) == MatchResult.Success(mapper(value))).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, int> mapper)
            => (!MatchResult.Failure<string>().Select(mapper).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () => MatchResult.Failure<string>().Select<string, int>(
                _ => throw new AssertionFailedException("select doesn't work"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectShouldThrowIfMapperIsNull(MatchResult<string> result)
        {
            Action action = () => result.Select<string, int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindShouldFlatMapValueIfResultIsSuccessful(string value, Func<string, MatchResult<int>> binder)
            => (MatchResult.Success(value).Bind(binder) == binder(value)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, MatchResult<int>> binder)
            => (!MatchResult.Failure<string>().Bind(binder).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () => MatchResult.Failure<string>().Bind<string, int>(
                _ => throw new AssertionFailedException("bind doesn't work"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldThrowIfBinderIsNull(MatchResult<string> result)
        {
            Action action = () => result.Bind<string, int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldFilterValueIfResultIsSuccessful(string value, Func<string, bool> predicate)
            => (MatchResult.Success(value).Where(predicate).IsSuccessful == predicate(value)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldHaveSameValueIfResultIsSuccessful(string value)
            => (MatchResult.Success(value).Where(_ => true).Value == value).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, bool> predicate)
            => (!MatchResult.Failure<string>().Where(predicate).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WhereShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () => MatchResult.Failure<string>().Where(
                _ => throw new AssertionFailedException("where doesn't work"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WhereShouldThrowIfPredicateIsNull(MatchResult<string> result)
        {
            Action action = () => result.Where(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value)
            => (MatchResult.Success<object>(value.Get).Cast<object, string>() == MatchResult.Success(value.Get))
                .ToProperty();

        [Fact]
        public Property CastShouldFailIfResultIsSuccessfulAndContainsNull()
            => (!MatchResult.Success<object>(null).Cast<object, int>().IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value)
            => (!MatchResult.Success<object>(value).Cast<object, int>().IsSuccessful).ToProperty();

        [Fact]
        public Property CastShouldBeUnsuccessfulIfResultIsUnsuccessful()
            => (!MatchResult.Failure<object>().Cast<object, string>().IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
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

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property DoShouldReturnSameResult(MatchResult<string> result)
            => (result == result.Do(_ => { })).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void DoShouldPassValueIfResultIsSuccessful(string value)
        {
            string actualValue = null;
            MatchResult.Success(value).Do(v => actualValue = v);
            actualValue.Should().Be(value);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void DoShouldThrowIfActionIsNull(MatchResult<string> result)
        {
            Action action = () => result.Do(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
