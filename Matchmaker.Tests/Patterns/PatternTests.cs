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
        public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string input)
            => (Pattern.CreatePattern(predicate).Match(input).IsSuccessful == predicate(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldMatchSameAsMatcher(Func<string, MatchResult<string>> matcher, string input)
            => (Pattern.CreatePattern(matcher).Match(input) == matcher(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveCorrectDescription(Func<string, bool> predicate, string description)
        {
            Func<bool> descriptionIsCorrect = () => Pattern.CreatePattern(predicate, description).Description == description;
            return descriptionIsCorrect.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveCorrectDescription(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> descriptionIsCorrect = () => Pattern.CreatePattern(matcher, description).Description == description;
            return descriptionIsCorrect.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, MatchResult<string>> matcher)
            => (Pattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnDescription(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> toStringIsCorrect = () => Pattern.CreatePattern(matcher, description).ToString() == description;
            return toStringIsCorrect.When(!String.IsNullOrEmpty(description));
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
            string description)
        {
            Func<bool> patternIsNotNull = () => Pattern.CreatePattern(predicate, description) != null;
            return patternIsNotNull.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateShouldNeverReturnNull(Func<string, MatchResult<string>> matcher)
            => (Pattern.CreatePattern(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> patternIsNotNull = () => Pattern.CreatePattern(matcher, description) != null;
            return patternIsNotNull.When(description != null);
        }

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
        public Property AnyWithDescriptionShouldAlwaysSucceed(string x, string description)
        {
            Func<bool> anyAlwaysSucceeds = () => Pattern.Any<string>(description).Match(x).IsSuccessful;
            return anyAlwaysSucceeds.When(description != null);
        }

        [Fact]
        public Property AnyShouldHaveCorrectDefaultDescription()
            => (Pattern.Any<string>().Description == Pattern.DefaultAnyDescription).ToProperty();

        [Property]
        public Property AnyWithDescriptionShouldHaveSpecifiedDescription(string description)
        {
            Func<bool> anyHasSpecifiedDescription = () =>
                Pattern.Any<string>(description).Description == description;
            return anyHasSpecifiedDescription.When(description != null);
        }

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
        public Property ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(string x, string y, string description)
        {
            Func<bool> returnAlwaysReturnsSpecifiedValue = () =>
            {
                var result = Pattern.Return<string, string>(y, description).Match(x);
                return result.IsSuccessful && result.Value == y;
            };

            return returnAlwaysReturnsSpecifiedValue.When(description != null);
        }

        [Property]
        public Property ReturnShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.Return<string, string>(x).Description ==
                String.Format(Pattern.DefaultReturnDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> returnHasSpecifiedDescription = () =>
                Pattern.Return<string, string>(x, description).Description == description;
            return returnHasSpecifiedDescription.When(description != null);
        }

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
            string description)
        {
            Func<bool> returnAlwaysReturnsSpecifiedValue = () =>
            {
                var result = Pattern.Return<string, string>(() => y, description).Match(x);
                return result.IsSuccessful && result.Value == y;
            };

            return returnAlwaysReturnsSpecifiedValue.When(description != null);
        }

        [Property]
        public Property LazyReturnShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.Return<string, string>(() => x).Description == Pattern.DefaultLazyReturnDescription)
                .ToProperty();

        [Property]
        public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> returnHasSpecifiedDescription = () =>
                Pattern.Return<string, string>(() => x, description).Description == description;
            return returnHasSpecifiedDescription.When(description != null);
        }

        [Fact]
        public void LazyReturnShouldBeLazy()
        {
            Action action = () => Pattern.Return<string, string>(() => throw new AssertionFailedException("not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyReturnWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.Return<string, string>(
                    () => throw new AssertionFailedException("not lazy"), description);
                action.Should().NotThrow<AssertionFailedException>();
            }
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
        public Property NullWithDescriptionShouldSucceedOnlyOnNull(string x, string description)
        {
            Func<bool> nullSucceedsOnlyOnNull = () =>
                x is null == Pattern.Null<string>(description).Match(x).IsSuccessful;
            return nullSucceedsOnlyOnNull.When(description != null);
        }

        [Fact]
        public Property NullShouldHaveCorrectDefaultDescription()
            => (Pattern.Null<string>().Description == Pattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property NullShouldHaveSpecifiedDewcription(string description)
        {
            Func<bool> nulldHasSpecifiedDewcription = () =>
                Pattern.Null<string>(description).Description == description;
            return nulldHasSpecifiedDewcription.When(description != null);
        }

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
        public Property ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, string description)
        {
            Func<bool> nullSucceedsOnlyOnNull = () =>
                x is null == Pattern.ValueNull<int>(description).Match(x).IsSuccessful;
            return nullSucceedsOnlyOnNull.When(description != null);
        }

        [Fact]
        public Property ValueNullShouldHaveCorrectDefaultDescription()
            => (Pattern.ValueNull<int>().Description == Pattern.DefaultNullDescription).ToProperty();

        [Property]
        public Property ValueNullShouldHaveSpecifiedDewcription(string description)
        {
            Func<bool> nulldHasSpecifiedDewcription = () =>
                Pattern.ValueNull<int>(description).Description == description;
            return nulldHasSpecifiedDewcription.When(description != null);
        }

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
        public Property TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(int value, string description)
        {
            Func<bool> typeSucceedsOnlyWhenValueHasType = () =>
                Pattern.Type<object, int>(description).Match(value).IsSuccessful &&
                !Pattern.Type<object, string>(description).Match(value).IsSuccessful;
            return typeSucceedsOnlyWhenValueHasType.When(description != null);
        }

        [Property]
        public Property TypeWithDescriptionShouldFailOnNull(string description)
        {
            Func<bool> typeFailsOnNull = () =>
                !Pattern.Type<object, string>(description).Match(null).IsSuccessful;
            return typeFailsOnNull.When(description != null);
        }

        [Fact]
        public void TypeShouldHaveCorrectDefaultDescription()
            => Pattern.Type<object, int>().Description.Should().BeEquivalentTo(
                String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(int)));

        [Property]
        public Property TypeWithDescriptionShouldHaveSpecifiedDescription(string description)
        {
            Func<bool> typeSucceedsOnlyWhenValueHasType = () =>
                Pattern.Type<object, string>(description).Description == description;
            return typeSucceedsOnlyWhenValueHasType.When(description != null);
        }

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
        public Property NotShouldHaveSpecifiedDescription(IPattern<string, string> pattern, string description)
        {
            Func<bool> notHasSpecifiedDescription = () => Pattern.Not(pattern, description).Description == description;
            return notHasSpecifiedDescription.When(description != null);
        }

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
