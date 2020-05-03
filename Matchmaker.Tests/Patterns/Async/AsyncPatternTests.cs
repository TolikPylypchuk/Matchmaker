using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns.Async
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class AsyncPatternTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldNeverReturnNull(Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldNeverReturnNull(Func<string, Task<MatchResult<string>>> matcher)
            => (AsyncPattern.CreatePattern(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternWithDescriptionShouldNeverReturnNull(
            Func<string, Task<bool>> predicate,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(predicate, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternWithDescriptionShouldNeverReturnNull(
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(matcher, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldMatchSameAsPredicate(
            Func<string, Task<bool>> predicate,
            string x)
            => (AsyncPattern.CreatePattern(predicate).MatchAsync(x).Result.IsSuccessful == predicate(x).Result)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldMatchSameAsMatcher(
            Func<string, Task<MatchResult<string>>> matcher,
            string x)
            => (AsyncPattern.CreatePattern(matcher).MatchAsync(x).Result == matcher(x).Result).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveCorrectDescription(
            Func<string, Task<bool>> predicate,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveCorrectDescription(
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<MatchResult<string>>> matcher)
            => (AsyncPattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnDescription(
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (description.Get.Length > 0).ImpliesThat(() =>
                    AsyncPattern.CreatePattern(matcher, description.Get).ToString() == description.Get)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(
            Func<string, Task<MatchResult<string>>> matcher)
            => (AsyncPattern.CreatePattern(matcher).ToString() ==
                AsyncPattern.CreatePattern(matcher).GetType().ToString())
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateShouldNeverReturnNull(Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, Task<bool>> predicate,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(predicate, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateShouldNeverReturnNull(Func<string, Task<MatchResult<string>>> matcher)
            => (AsyncPattern.CreatePattern(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (AsyncPattern.CreatePattern(matcher, description.Get) != null).ToProperty();

        [Fact]
        public void SimplePatternCreateShouldThrowIfPredicateIsNull()
        {
            Action createWithNull = () => AsyncPattern.CreatePattern<string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SimplePatternCreateShouldThrowIfDescriptionIsNull(Func<string, Task<bool>> predicate)
        {
            Action createWithNull = () => AsyncPattern.CreatePattern(predicate, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatternCreateShouldThrowIfMatcherIsNull()
        {
            Action createWithNull = () => AsyncPattern.CreatePattern<string, string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PatternCreateShouldThrowIfDescriptionIsNull(Func<string, Task<MatchResult<string>>> matcher)
        {
            Action createWithNull = () => AsyncPattern.CreatePattern(matcher, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public Property AnyShouldNeverReturnNull()
            => (AsyncPattern.Any<string>() != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AnyWithDescriptionShouldNeverReturnNull(NonNull<string> description)
            => (AsyncPattern.Any<string>(description.Get) != null).ToProperty();

        [Property]
        public Property AnyShouldAlwaysSucceed(string x)
            => AsyncPattern.Any<string>().MatchAsync(x).Result.IsSuccessful.ToProperty();

        [Property]
        public Property AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description)
            => AsyncPattern.Any<string>(description.Get).MatchAsync(x).Result.IsSuccessful.ToProperty();

        [Fact]
        public Property AnyShouldHaveCorrectDefaultDescription()
            => (AsyncPattern.Any<string>().Description == AsyncPattern.DefaultAnyDescription).ToProperty();

        [Property]
        public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description)
            => (AsyncPattern.Any<string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void AnyShouldThrowIfDescriptionIsNull()
        {
            Action action = () => AsyncPattern.Any<string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.Return<string, string>(x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.Return<string, string>(x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.Return<string, string>(() => x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.Return<string, string>(() => x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
        {
            var result = AsyncPattern.Return<string, string>(y).MatchAsync(x).Result;
            return (result.IsSuccessful && result.Value == y.Result).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
            string x,
            Task<string> y,
            NonNull<string> description)
        {
            var result = AsyncPattern.Return<string, string>(y, description.Get).MatchAsync(x).Result;
            return (result.IsSuccessful && result.Value == y.Result).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.Return<string, string>(x).Description == AsyncPattern.DefaultReturnDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ReturnShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.Return<string, string>(x, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
        {
            var result = AsyncPattern.Return<string, string>(() => y).MatchAsync(x).Result;
            return (result.IsSuccessful && result.Value == y.Result).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
            string x,
            Task<string> y,
            NonNull<string> description)
        {
            var result = AsyncPattern.Return<string, string>(() => y, description.Get).MatchAsync(x).Result;
            return (result.IsSuccessful && result.Value == y.Result).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.Return<string, string>(() => x).Description == AsyncPattern.DefaultReturnDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.Return<string, string>(() => x, description.Get).Description == description.Get)
                .ToProperty();

        [Fact]
        public void LazyReturnShouldBeLazy()
        {
            Action action = () => AsyncPattern.Return<string, string>(
                () => throw new AssertionFailedException("Lazy Return is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => AsyncPattern.Return<string, string>(
                () => throw new AssertionFailedException("Lazy Return is not lazy"), description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public Property LazyReturnShouldBeMemoized(string x)
        {
            int counter = 0;

            var pattern = AsyncPattern.Return<string, string>(() =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            });

            pattern.MatchAsync(x);
            pattern.MatchAsync(x);

            return (counter == 1).ToProperty();
        }

        [Property]
        public Property LazyReturnWithDescriptionShouldBeMemoized(string x, NonNull<string> description)
        {
            int counter = 0;

            var pattern = AsyncPattern.Return<string, string>(
                () =>
                {
                    counter++;
                    return Task.FromResult(String.Empty);
                },
                description.Get);

            pattern.MatchAsync(x);
            pattern.MatchAsync(x);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyReturnShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.Return<string, string>(() => x, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public Property NullShouldNeverReturnNull()
            => (AsyncPattern.Null<string>() != null).ToProperty();

        [Property]
        public Property NullWithDescriptionShouldNeverReturnNull(NonNull<string> description)
            => (AsyncPattern.Null<string>(description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NullShouldSucceedOnlyOnNull(string x)
            => (x is null == AsyncPattern.Null<string>().MatchAsync(x).Result.IsSuccessful).ToProperty();

        [Property]
        public Property NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description)
            => (x is null == AsyncPattern.Null<string>(description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Fact]
        public Property NullShouldHaveCorrectDefaultDescription()
            => (AsyncPattern.Null<string>().Description == AsyncPattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property NullShouldHaveSpecifiedDewcription(NonNull<string> description)
            => (AsyncPattern.Null<string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void NullShouldThrowIfDescriptionIsNull()
        {
            Action action = () => AsyncPattern.Null<string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public Property ValueNullShouldNeverReturnNull()
            => (AsyncPattern.ValueNull<int>() != null).ToProperty();

        [Property]
        public Property ValueNullWithDescriptionShouldNeverReturnNull(NonNull<string> description)
            => (AsyncPattern.ValueNull<int>(description.Get) != null).ToProperty();

        [Property]
        public Property ValueNullShouldSucceedOnlyOnNull(int? x)
            => (x is null == AsyncPattern.ValueNull<int>().MatchAsync(x).Result.IsSuccessful).ToProperty();

        [Property]
        public Property ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description)
            => (x is null == AsyncPattern.ValueNull<int>(description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Fact]
        public Property ValueNullShouldHaveCorrectDefaultDescription()
            => (AsyncPattern.ValueNull<int>().Description == AsyncPattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property ValueNullShouldHaveSpecifiedDewcription(NonNull<string> description)
            => (AsyncPattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void ValueNullShouldThrowIfDescriptionIsNull()
        {
            Action action = () => AsyncPattern.ValueNull<int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public Property TypeShouldNeverReturnNull()
            => (AsyncPattern.Type<object, string>() != null).ToProperty();

        [Property]
        public Property TypeWithDescriptionShouldNeverReturnNull(NonNull<string> description)
            => (AsyncPattern.Type<object, string>(description.Get) != null).ToProperty();

        [Property]
        public Property TypeShouldSucceedOnlyWhenTheValueHasType(int value)
            => (AsyncPattern.Type<object, int>().MatchAsync(value).Result.IsSuccessful &&
                !AsyncPattern.Type<object, string>().MatchAsync(value).Result.IsSuccessful)
                .ToProperty();

        [Fact]
        public void TypeShouldSucceedOnNull()
            => AsyncPattern.Type<object, string>().MatchAsync(null).Result.IsSuccessful.Should().BeTrue();

        [Fact]
        public void TypeShouldSucceedOnNullableValueNull()
            => AsyncPattern.Type<object, int?>().MatchAsync(null).Result.IsSuccessful.Should().BeTrue();

        [Fact]
        public void TypeShouldFailOnValueNull()
            => AsyncPattern.Type<object, int>().MatchAsync(null).Result.IsSuccessful.Should().BeFalse();

        [Property]
        public Property TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(
            int value, NonNull<string> description)
            => (AsyncPattern.Type<object, int>(description.Get).MatchAsync(value).Result.IsSuccessful &&
                !AsyncPattern.Type<object, string>(description.Get).MatchAsync(value).Result.IsSuccessful)
                .ToProperty();

        [Property]
        public Property TypeWithDescriptionShouldSucceedOnNull(NonNull<string> description)
            => AsyncPattern.Type<object, string>(description.Get).MatchAsync(null).Result.IsSuccessful.ToProperty();

        [Property]
        public Property TypeWithDescriptionShouldSucceedOnNullableValueNull(NonNull<string> description)
            => AsyncPattern.Type<object, int?>(description.Get).MatchAsync(null).Result.IsSuccessful.ToProperty();

        [Property]
        public Property TypeWithDescriptionShouldFailOnValueNull(NonNull<string> description)
            => (!AsyncPattern.Type<object, int>(description.Get).MatchAsync(null).Result.IsSuccessful).ToProperty();

        [Fact]
        public void TypeShouldHaveCorrectDefaultDescription()
            => AsyncPattern.Type<object, int>().Description.Should().Be(
                String.Format(AsyncPattern.DefaultTypeDescriptionFormat, typeof(int)));

        [Property]
        public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description)
            => (AsyncPattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void TypeShouldThrowIfDescriptionIsNull()
        {
            Action action = () => AsyncPattern.Type<object, int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldNeverReturnNull(IAsyncPattern<string, string> pattern)
            => (AsyncPattern.Not(pattern) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
            => (AsyncPattern.Not(pattern, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldBeOppositeToPattern(IAsyncPattern<string, string> pattern, string x)
            => (pattern.MatchAsync(x).Result.IsSuccessful ==
                !AsyncPattern.Not(pattern).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveCorrectDescription(IAsyncPattern<string, string> pattern)
            => (AsyncPattern.Not(pattern).Description ==
                String.Format(AsyncPattern.DefaultNotDescriptionFormat, pattern.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
            => (AsyncPattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, Task<bool>> predicate)
            => (AsyncPattern.Not(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
                .ToProperty();

        [Fact]
        public void NotShouldThrowIfPatternIsNull()
        {
            Action action = () => AsyncPattern.Not<object, object>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NotShouldThrowIfDescriptionIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => AsyncPattern.Not(pattern, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
