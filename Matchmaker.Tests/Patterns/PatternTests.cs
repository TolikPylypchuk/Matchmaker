using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Linq;

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
            => (Pattern.CreatePattern(matcher).ToString() == typeof(SimplePattern<string, string>).ToString())
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
        public void SimplePatternCreateShouldThrowForNullPredicate()
        {
            Action createWithNull = () => Pattern.CreatePattern<string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SimplePatternCreateShouldThrowForNullDescription(Func<string, bool> predicate)
        {
            Action createWithNull = () => Pattern.CreatePattern(predicate, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatternCreateShouldThrowForNullMatcher()
        {
            Action createWithNull = () => Pattern.CreatePattern<string, string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PatternCreateShouldThrowForNullDescription(Func<string, MatchResult<string>> matcher)
        {
            Action createWithNull = () => Pattern.CreatePattern(matcher, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternWithWhereShouldMatchSameAsMatcherAndPredicates(
            Func<string, MatchResult<string>> matcher,
            List<Func<string, bool>> predicates,
            string input)
        {
            Func<bool> patternWithWhenMatchesSameAsPredicates = () =>
            {
                var result = matcher(input);
                var actualResult = result.IsSuccessful
                    ? predicates.All(predicate => predicate(result.Value))
                        ? result
                        : MatchResult.Failure<string>()
                    : MatchResult.Failure<string>();

                return predicates
                    .Aggregate(
                        Pattern.CreatePattern(matcher),
                        (pattern, predicate) => pattern.Where(predicate))
                    .Match(input) == actualResult;
            };

            return patternWithWhenMatchesSameAsPredicates
                .When(matcher != null && predicates != null && predicates.Count > 1);
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
        public Property AnyShouldHaveSpecifiedDescription(string description)
        {
            Func<bool> anyHasSpecifiedDescription = () =>
                Pattern.Any<string>(description).Description == description;
            return anyHasSpecifiedDescription.When(description != null);
        }

        [Fact]
        public void AnyShouldThrowOnNullDescription()
        {
            Action action = () => Pattern.Any<string>(null);
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
        public void NullShouldThrowOnNullDescription()
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
        public void ValueNullShouldThrowOnNullDescription()
        {
            Action action = () => Pattern.ValueNull<int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property TypeShouldSucceedOnlyWhenTheValueHasType(int value)
            => Pattern.Type<object, int>().Match(value).IsSuccessful.ToProperty();

        [Fact]
        public void TypeShouldFailOnNull()
            => Pattern.Type<object, object>().Match(null).IsSuccessful.Should().BeFalse();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                    pattern1.And(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> andPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                pattern1.And(pattern2, description).Match(x).IsSuccessful;
            return andPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldBeSameAsEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Or(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> orPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Or(pattern2, description).Match(x).IsSuccessful;
            return orPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldBeSameAsExcusiveEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Xor(pattern2).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> xorPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Xor(pattern2, description).Match(x).IsSuccessful;
            return xorPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.And(pattern2).Description ==
                String.Format(Pattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.And(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).And(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> andPatternHasSpecifiedDescription = () =>
                pattern1.And(pattern2, description).Description == description;
            return andPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Or(pattern2).Description ==
                String.Format(Pattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Or(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).Or(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> orPatternHasSpecifiedDescription = () =>
                pattern1.Or(pattern2, description).Description == description;
            return orPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Xor(pattern2).Description ==
                String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Xor(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).Xor(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> xorPatternHasSpecifiedDescription = () =>
                pattern1.Xor(pattern2, description).Description == description;
            return xorPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldBeOppositeToSimplePattern(IPattern<string, string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldBeOppositeToPattern(IPattern<string, string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotShouldBeOppositeToGeneralPattern(IPattern<string, string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

        [Property]
        public void NotTypeShouldFailOnlyWhenTheValueHasType(int value)
            => (!Pattern.Not(Pattern.Type<object, int>()).Match(value).IsSuccessful).ToProperty();

        [Fact]
        public void NotTypeShouldSucceedOnNull()
            => Pattern.Not(Pattern.Type<object, object>()).Match(null).IsSuccessful.Should().BeTrue();

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
        public void NotShouldThrowIfDescribablePatternIsNull()
        {
            Action action = () => Pattern.Not<object, object>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NotShouldThrowIfPatternIsNull()
        {
            Action action = () => Pattern.Not((IPattern<object, object>)null);
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
