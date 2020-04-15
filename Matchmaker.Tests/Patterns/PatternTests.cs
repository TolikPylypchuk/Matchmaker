using System;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class PatternTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string x)
            => (Pattern.CreatePattern(predicate).Match(x).IsSuccessful == predicate(x)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldMatchSameAsMatcher(Func<string, MatchResult<string>> matcher, string x)
            => (Pattern.CreatePattern(matcher).Match(x) == matcher(x)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveCorrectDescription(
            Func<string, bool> predicate,
            NonNull<string> description)
            => (Pattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveCorrectDescription(
            Func<string, MatchResult<string>> matcher,
            NonNull<string> description)
            => (Pattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, MatchResult<string>> matcher)
            => (Pattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnDescription(
            Func<string, MatchResult<string>> matcher,
            NonNull<string> description)
        {
            Func<bool> toStringIsCorrect = () =>
                Pattern.CreatePattern(matcher, description.Get).ToString() == description.Get;
            return toStringIsCorrect.When(description.Get.Length > 0);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(Func<string, MatchResult<string>> matcher)
            => (Pattern.CreatePattern(matcher).ToString() == Pattern.CreatePattern(matcher).GetType().ToString())
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateShouldNeverReturnNull(Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, bool> predicate,
            NonNull<string> description)
            => (Pattern.CreatePattern(predicate, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateShouldNeverReturnNull(Func<string, MatchResult<string>> matcher)
            => (Pattern.CreatePattern(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, MatchResult<string>> matcher,
            NonNull<string> description)
            => (Pattern.CreatePattern(matcher, description.Get) != null).ToProperty();

        [Fact]
        public void SimplePatternCreateShouldThrowIfPredicateIsNull()
        {
            Action createWithNull = () => Pattern.CreatePattern<string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SimplePatternCreateShouldThrowIfDescriptionIsNull(Func<string, bool> predicate)
        {
            Action createWithNull = () => Pattern.CreatePattern(predicate, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatternCreateShouldThrowIfMatcherIsNull()
        {
            Action createWithNull = () => Pattern.CreatePattern<string, string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PatternCreateShouldThrowIfDescriptionIsNull(Func<string, MatchResult<string>> matcher)
        {
            Action createWithNull = () => Pattern.CreatePattern(matcher, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property AnyShouldAlwaysSucceed(string x)
            => Pattern.Any<string>().Match(x).IsSuccessful.ToProperty();

        [Property]
        public Property AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description)
            => Pattern.Any<string>(description.Get).Match(x).IsSuccessful.ToProperty();

        [Fact]
        public Property AnyShouldHaveCorrectDefaultDescription()
            => (Pattern.Any<string>().Description == Pattern.DefaultAnyDescription).ToProperty();

        [Property]
        public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description)
            => (Pattern.Any<string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void AnyShouldThrowIfDescriptionIsNull()
        {
            Action action = () => Pattern.Any<string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property ReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
        {
            var result = Pattern.Return<string, string>(y).Match(x);
            return (result.IsSuccessful && result.Value == y).ToProperty();
        }

        [Property]
        public Property ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
            string x,
            string y,
            NonNull<string> description)
        {
            var result = Pattern.Return<string, string>(y, description.Get).Match(x);
            return (result.IsSuccessful && result.Value == y).ToProperty();
        }

        [Property]
        public Property ReturnShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.Return<string, string>(x).Description ==
                String.Format(Pattern.DefaultReturnDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description)
            => (Pattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

        [Property]
        public void ReturnShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.Return<string, string>(x, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property LazyReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
        {
            var result = Pattern.Return<string, string>(() => y).Match(x);
            return (result.IsSuccessful && result.Value == y).ToProperty();
        }

        [Property]
        public Property LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
            string x,
            string y,
            NonNull<string> description)
        {
            var result = Pattern.Return<string, string>(() => y, description.Get).Match(x);
            return (result.IsSuccessful && result.Value == y).ToProperty();
        }

        [Property]
        public Property LazyReturnShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.Return<string, string>(() => x).Description == Pattern.DefaultLazyReturnDescription)
                .ToProperty();

        [Property]
        public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description)
            => (Pattern.Return<string, string>(() => x, description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void LazyReturnShouldBeLazy()
        {
            Action action = () => Pattern.Return<string, string>(
                () => throw new AssertionFailedException("Lazy Return is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => Pattern.Return<string, string>(
                () => throw new AssertionFailedException("Lazy Return is not lazy"), description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public Property LazyReturnShouldBeMemoized(string x)
        {
            int counter = 0;

            var pattern = Pattern.Return<string, string>(() =>
            {
                counter++;
                return String.Empty;
            });

            pattern.Match(x);
            pattern.Match(x);

            return (counter == 1).ToProperty();
        }

        [Property]
        public Property LazyReturnWithDescriptionShouldBeMemoized(string x, NonNull<string> description)
        {
            int counter = 0;

            var pattern = Pattern.Return<string, string>(
                () =>
                {
                    counter++;
                    return String.Empty;
                },
                description.Get);

            pattern.Match(x);
            pattern.Match(x);

            return (counter == 1).ToProperty();
        }

        [Property]
        public void LazyReturnShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.Return<string, string>(() => x, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property NullShouldSucceedOnlyOnNull(string x)
            => (x is null == Pattern.Null<string>().Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description)
            => (x is null == Pattern.Null<string>(description.Get).Match(x).IsSuccessful).ToProperty();

        [Fact]
        public Property NullShouldHaveCorrectDefaultDescription()
            => (Pattern.Null<string>().Description == Pattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property NullShouldHaveSpecifiedDewcription(NonNull<string> description)
            => (Pattern.Null<string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void NullShouldThrowIfDescriptionIsNull()
        {
            Action action = () => Pattern.Null<string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property ValueNullShouldSucceedOnlyOnNull(int? x)
            => (x is null == Pattern.ValueNull<int>().Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description)
            => (x is null == Pattern.ValueNull<int>(description.Get).Match(x).IsSuccessful).ToProperty();

        [Fact]
        public Property ValueNullShouldHaveCorrectDefaultDescription()
            => (Pattern.ValueNull<int>().Description == Pattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property ValueNullShouldHaveSpecifiedDewcription(NonNull<string> description)
            => (Pattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void ValueNullShouldThrowIfDescriptionIsNull()
        {
            Action action = () => Pattern.ValueNull<int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property TypeShouldSucceedOnlyWhenTheValueHasType(int value)
            => (Pattern.Type<object, int>().Match(value).IsSuccessful &&
                !Pattern.Type<object, string>().Match(value).IsSuccessful)
                .ToProperty();

        [Fact]
        public void TypeShouldFailOnNull()
            => Pattern.Type<object, object>().Match(null).IsSuccessful.Should().BeFalse();

        [Property]
        public Property TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(int value, NonNull<string> description)
            => (Pattern.Type<object, int>(description.Get).Match(value).IsSuccessful &&
                !Pattern.Type<object, string>(description.Get).Match(value).IsSuccessful)
                .ToProperty();

        [Property]
        public Property TypeWithDescriptionShouldFailOnNull(NonNull<string> description)
            => (!Pattern.Type<object, string>(description.Get).Match(null).IsSuccessful).ToProperty();

        [Fact]
        public void TypeShouldHaveCorrectDefaultDescription()
            => Pattern.Type<object, int>().Description.Should().Be(
                String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(int)));

        [Property]
        public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description)
            => (Pattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void TypeShouldThrowIfDescriptionIsNull()
        {
            Action action = () => Pattern.Type<object, int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldBeOppositeToPattern(IPattern<string, string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveCorrectDescription(IPattern<string, string> pattern)
            => (Pattern.Not(pattern).Description ==
                String.Format(Pattern.DefaultNotDescriptionFormat, pattern.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveSpecifiedDescription(IPattern<string, string> pattern, NonNull<string> description)
            => (Pattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, bool> predicate)
            => (Pattern.Not(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Fact]
        public void NotShouldThrowIfPatternIsNull()
        {
            Action action = () => Pattern.Not<object, object>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NotShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
        {
            Action action = () => Pattern.Not(pattern, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
