using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Linq
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class AsyncMatchResultExtensionsTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultShouldReturnValueIfResultIsSuccessful(string value)
            => (Task.FromResult(MatchResult.Success(value)).GetValueOrDefault().Result == value).ToProperty();

        [Fact]
        public void GetValueOrDefaultShouldReturnDefaultIfResultIsUnsuccessful()
            => Task.FromResult(MatchResult.Failure<string>()).Result.GetValueOrDefault().Should().BeNull();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultValueShouldReturnValueIfResultIsSuccessful(string value, string defaultValue)
            => (Task.FromResult(MatchResult.Success(value)).GetValueOrDefault(defaultValue).Result == value)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue)
            => (Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(defaultValue).Result == defaultValue)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
            string value,
            string defaultValue)
            => (Task.FromResult(MatchResult.Success(value)).GetValueOrDefault(() => defaultValue).Result == value)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncGetValueOrDefaultLazyValueShouldReturnValueIfResultIsSuccessful(
            string value,
            string defaultValue)
            => (Task.FromResult(MatchResult.Success(value)).GetValueOrDefault(() => Task.FromResult(defaultValue))
                    .Result == value)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(string defaultValue)
            => (Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => defaultValue).Result ==
                defaultValue)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncGetValueOrDefaultLazyValueShouldReturnSpecifiedValueIfResultIsUnsuccessful(
            string defaultValue)
            => (Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => Task.FromResult(defaultValue))
                    .Result == defaultValue)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GetValueOrDefaultLazyValueShouldBeLazy(string value)
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Success(value))
                    .GetValueOrDefault((Func<string>)(() => throw new AssertionFailedException("not lazy")))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncGetValueOrDefaultLazyValueShouldBeLazy(string value)
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Success(value))
                    .GetValueOrDefault((Func<Task<string>>)(() => throw new AssertionFailedException("not lazy")))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }
        [Fact]
        public void GetValueOrDefaultShouldThrowIfResultTaskIsNull()
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).GetValueOrDefault().Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GetValueOrDefaultValueShouldThrowIfResultTaskIsNull(string value)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).GetValueOrDefault(value).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).GetValueOrDefault(() => value).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void AsyncGetValueOrDefaultLazyValueShouldThrowIfResultTaskIsNull(string value)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).GetValueOrDefault(() => Task.FromResult(value)).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.GetValueOrDefault((Func<string>)null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.GetValueOrDefault((Func<Task<string>>)null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AsyncGetValueOrDefaultLazyValueShouldThrowIfValueProviderReturnsNull()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>()).GetValueOrDefault(() => (Task<string>)null).Result;
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetValueOrThrowShouldReturnValueIfResultIsSuccessful(string value)
            => (Task.FromResult(MatchResult.Success(value)).GetValueOrThrow(() => new MatchException()).Result == value)
                .ToProperty();

        [Fact]
        public void GetValueOrThrowShouldThrowIfResultIsUnsuccessful()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>()).GetValueOrThrow(() => new MatchException()).Result;
            };

            action.Should().Throw<MatchException>();
        }

        [Fact]
        public void GetValueOrThrowShouldThrowIfResultTaskIsNull()
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).GetValueOrThrow(() => new MatchException()).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GetValueOrThrowShouldThrowIfExceptionProviderIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.GetValueOrThrow(null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectShouldMapValueIfResultIsSuccessful(string value, Func<string, int> mapper)
            => (Task.FromResult(MatchResult.Success(value)).Select(mapper).Result == MatchResult.Success(mapper(value)))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, int> mapper)
            => (!Task.FromResult(MatchResult.Failure<string>()).Select(mapper).Result.IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>())
                    .Select<string, int>(_ => throw new AssertionFailedException("select doesn't work"))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectShouldThrowIfTaskResultIsNull(Func<string, int> mapper)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).Select(mapper).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectShouldThrowIfMapperIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.Select<string, int>(null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindShouldFlatMapValueIfResultIsSuccessful(
            string value,
            Func<string, Task<MatchResult<int>>> binder)
            => (Task.FromResult(MatchResult.Success(value)).Bind(binder).Result == binder(value).Result).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, Task<MatchResult<int>>> binder)
            => (!Task.FromResult(MatchResult.Failure<string>()).Bind(binder).Result.IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>())
                    .Bind<string, int>(_ => throw new AssertionFailedException("bind doesn't work"))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldThrowIfTaskResultIsNull(Func<string, Task<MatchResult<int>>> binder)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).Bind(binder).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldThrowIfBinderIsNull(MatchResult<string> result)
        {
            Action action = () =>
            {
                _ = Task.FromResult(result).Bind<string, int>(null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindShouldThrowIfBinderReturnsNull(string value)
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Success(value)).Bind<string, int>(_ => null).Result;
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldFilterValueIfResultIsSuccessful(string value, Func<string, bool> predicate)
            => (Task.FromResult(MatchResult.Success(value)).Where(predicate).Result.IsSuccessful == predicate(value))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncWhereShouldFilterValueIfResultIsSuccessful(
            string value,
            Func<string, Task<bool>> predicate)
            => (Task.FromResult(MatchResult.Success(value)).Where(predicate).Result.IsSuccessful ==
                predicate(value).Result)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldHaveSameValueIfResultIsSuccessful(string value)
            => (MatchResult.Success(value).Where(_ => true).Value == value).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncWhereShouldHaveSameValueIfResultIsSuccessful(string value)
            => (Task.FromResult(MatchResult.Success(value)).Where(_ => Task.FromResult(true)).Result.Value == value)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, bool> predicate)
            => (!MatchResult.Failure<string>().Where(predicate).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncWhereShouldBeUnsuccessfulIfResultIsUnsuccessful(Func<string, Task<bool>> predicate)
            => (!Task.FromResult(MatchResult.Failure<string>()).Where(predicate).Result.IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WhereShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>())
                    .Where((Func<string, bool>)(_ => throw new AssertionFailedException("where doesn't work")))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncWhereShouldDoNothingIfResultIsUnsuccessful()
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Failure<string>())
                    .Where((Func<string, Task<bool>>)(_ => throw new AssertionFailedException("where doesn't work")))
                    .Result;
            };

            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WhereShouldThrowIfTaskResultIsNull(Func<string, bool> predicate)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).Where(predicate).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncWhereShouldThrowIfTaskResultIsNull(Func<string, Task<bool>> predicate)
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<string>>)null).Where(predicate).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.Where((Func<string, bool>)null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncWhereShouldThrowIfPredicateIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.Where((Func<string, Task<bool>>)null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void AsyncWhereShouldThrowIfPredicateReturnsNull(string value)
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Success(value)).Where(_ => null).Result;
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastShouldCastValueOfCorrectTypeIfResultIsSuccessful(NonNull<string> value)
            => (Task.FromResult(MatchResult.Success<object>(value.Get)).Cast<object, string>().Result ==
                MatchResult.Success(value.Get))
                .ToProperty();

        [Fact]
        public Property CastShouldSucceedIfResultContainsNull()
            => Task.FromResult(MatchResult.Success<object>(null)).Cast<object, string>().Result.IsSuccessful
                .ToProperty();

        [Fact]
        public Property CastToNullableValueShouldFailIfResultContainsNull()
            => Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int?>().Result.IsSuccessful.ToProperty();

        [Fact]
        public Property CastToValueShouldFailIfResultContainsNull()
            => (!Task.FromResult(MatchResult.Success<object>(null)).Cast<object, int>().Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastShouldFailIfValueHasIncorrectTypeAndResultIsSuccessful(string value)
            => (!Task.FromResult(MatchResult.Success<object>(value)).Cast<object, int>().Result.IsSuccessful)
                .ToProperty();

        [Fact]
        public Property CastShouldBeUnsuccessfulIfResultIsUnsuccessful()
            => (!Task.FromResult(MatchResult.Failure<object>()).Cast<object, string>().Result.IsSuccessful)
                .ToProperty();

        [Fact]
        public void CastShouldThrowIfTaskResultIsNull()
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<object>>)null).Cast<object, string>().Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void DoShouldPerformActionIfResultIsSuccessful(string value)
        {
            int count = 0;
            _ = Task.FromResult(MatchResult.Success(value)).Do(_ => count++).Result;
            count.Should().Be(1);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncDoShouldPerformActionIfResultIsSuccessful(string value)
        {
            int count = 0;
            _ = Task.FromResult(MatchResult.Success(value)).Do(_ => Task.FromResult(count++)).Result;
            count.Should().Be(1);
        }

        [Fact]
        public void DoShouldNotPerformActionIfResultIsUnsuccessful()
        {
            int count = 0;
            _ = Task.FromResult(MatchResult.Failure<string>()).Do(_ => count++).Result;
            count.Should().Be(0);
        }

        [Fact]
        public void AsyncDoShouldNotPerformActionIfResultIsUnsuccessful()
        {
            int count = 0;
            _ = Task.FromResult(MatchResult.Failure<string>()).Do(_ => Task.FromResult(count++)).Result;
            count.Should().Be(0);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property DoShouldReturnSameResult(Task<MatchResult<string>> result)
            => (result.Result == result.Do(_ => { }).Result).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AsyncDoShouldReturnSameResult(Task<MatchResult<string>> result)
            => (result.Result == result.Do(_ => Task.CompletedTask).Result).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void DoShouldPassValueIfResultIsSuccessful(string value)
        {
            string actualValue = null;
            _ = Task.FromResult(MatchResult.Success(value)).Do(v => actualValue = v).Result;
            actualValue.Should().Be(value);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncDoShouldPassValueIfResultIsSuccessful(string value)
        {
            string actualValue = null;
            _ = Task.FromResult(MatchResult.Success(value)).Do(v => Task.FromResult(actualValue = v)).Result;
            actualValue.Should().Be(value);
        }

        [Fact]
        public void DoShouldThrowIfTaskResultIsNull()
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<object>>)null).Do(_ => { }).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AsyncDoShouldThrowIfTaskResultIsNull()
        {
            Action action = () =>
            {
                _ = ((Task<MatchResult<object>>)null).Do(_ => Task.CompletedTask).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void DoShouldThrowIfActionIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.Do((Action<string>)null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AsyncDoShouldThrowIfActionIsNull(Task<MatchResult<string>> result)
        {
            Action action = () =>
            {
                _ = result.Do(null).Result;
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void AsyncDoShouldThrowIfActionReturnsNull(string value)
        {
            Action action = () =>
            {
                _ = Task.FromResult(MatchResult.Success(value)).Do(_ => null).Result;
            };

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
