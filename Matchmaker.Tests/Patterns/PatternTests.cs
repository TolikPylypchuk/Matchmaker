using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class PatternTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string input)
            => (Pattern.Create(predicate).Match(input).IsSuccessful == predicate(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldMatchSameAsMatcher(Func<string, MatchResult<string>> matcher, string input)
            => (Pattern.Create(matcher).Match(input) == matcher(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveCorrectDescription(Func<string, bool> predicate, string description)
        {
            Func<bool> descriptionIsCorrect = () => Pattern.Create(predicate, description).Description == description;
            return descriptionIsCorrect.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, bool> predicate)
            => (Pattern.Create(predicate).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveCorrectDescription(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> descriptionIsCorrect = () => Pattern.Create(matcher, description).Description == description;
            return descriptionIsCorrect.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, MatchResult<string>> matcher)
            => (Pattern.Create(matcher).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternToStringShouldReturnToString(Func<string, bool> predicate, string description)
        {
            Func<bool> toStringIsCorrect = () => Pattern.Create(predicate, description).ToString() == description;
            return toStringIsCorrect.When(!String.IsNullOrEmpty(description));
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternToStringShouldReturnTypeWhenDescriptionIsEmpty(Func<string, bool> predicate)
            => (Pattern.Create(predicate).ToString() == typeof(SimplePattern<string>).ToString()).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnToString(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> toStringIsCorrect = () => Pattern.Create(matcher, description).ToString() == description;
            return toStringIsCorrect.When(!String.IsNullOrEmpty(description));
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(Func<string, MatchResult<string>> matcher)
            => (Pattern.Create(matcher).ToString() == typeof(Pattern<string, string>).ToString()).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateShouldNeverReturnNull(Func<string, bool> predicate)
            => (Pattern.Create(predicate) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, bool> predicate,
            string description)
        {
            Func<bool> patternIsNotNull = () => Pattern.Create(predicate, description) != null;
            return patternIsNotNull.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateShouldNeverReturnNull(Func<string, MatchResult<string>> matcher)
            => (Pattern.Create(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternCreateWithDescriptionShouldNeverReturnNull(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> patternIsNotNull = () => Pattern.Create(matcher, description) != null;
            return patternIsNotNull.When(description != null);
        }

        [Fact]
        public void SimplePatternCreateShouldThrowForNullPredicate()
        {
            Action createWithNull = () => Pattern.Create<string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SimplePatternCreateShouldThrowForNullDescription(Func<string, bool> predicate)
        {
            Action createWithNull = () => Pattern.Create(predicate, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatternCreateShouldThrowForNullMatcher()
        {
            Action createWithNull = () => Pattern.Create<string, string>(null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PatternCreateShouldThrowForNullDescription(Func<string, MatchResult<string>> matcher)
        {
            Action createWithNull = () => Pattern.Create(matcher, null);
            createWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternWithWhenShouldMatchSameAsPredicates(
            List<Func<string, bool>> predicates,
            string input)
        {
            Func<bool> property = () => predicates
                .Skip(1)
                .Aggregate(
                    Pattern.Create(predicates.First()),
                    (pattern, predicate) => pattern.When(predicate))
                .Match(input)
                .IsSuccessful == predicates.All(predicate => predicate(input));

            return property.When(predicates != null && predicates.Count > 1);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternWithWhenShouldMatchSameAsMatcherAndPredicates(
            Func<string, MatchResult<string>> matcher,
            List<Func<string, bool>> predicates,
            string input)
        {
            Func<bool> property = () =>
            {
                var result = matcher(input);
                var actualResult = result.IsSuccessful
                    ? predicates.All(predicate => predicate(result.Value))
                        ? result
                        : MatchResult.Failure<string>()
                    : MatchResult.Failure<string>();

                return predicates
                    .Aggregate(
                        Pattern.Create(matcher),
                        (pattern, predicate) => pattern.When(predicate))
                    .Match(input) == actualResult;
            };

            return property.When(matcher != null && predicates != null && predicates.Count > 1);
        }

        [Property]
        public Property AnyShouldAlwaysSucceed(string x)
            => Pattern.Any<string>().Match(x).IsSuccessful.ToProperty();

        [Property]
        public Property NullShouldSucceedOnlyOnNull(string x)
            => (x is null == Pattern.Null<string>().Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property ValueNullShouldSucceedOnlyOnNull(int? x)
            => (x is null == Pattern.ValueNull<int>().Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(y).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(() => y).Match(x).IsSuccessful).ToProperty();

        [Fact]
        public void LazyEqualToShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.EqualTo((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property LessThanShouldSucceedOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessThanShouldSucceedOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property LessOrEqualShouldSucceedOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldSucceedOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property GreaterThanShouldSucceedOnlyWhenValueIsLGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterThanShouldSucceedOnlyWhenValueIsGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property GreaterOrEqualShouldSucceedOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterOrEqualShouldSucceedOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TypeShouldSucceedOnlyWhenTheValueHasType()
        {
            Pattern.Type<object, int>().Match(1).IsSuccessful.Should().BeTrue();
            Pattern.Type<object, string>().Match("string").IsSuccessful.Should().BeTrue();
            Pattern.Type<object, object>().Match(null).IsSuccessful.Should().BeFalse();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldBeSameAsBothPatterns(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2, string x)
            => (!pattern1.Match(x).IsSuccessful || !pattern2.Match(x).IsSuccessful ==
                    !pattern1.And(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorAndPatternShouldBeSameAsBothPatterns(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2,
            string x)
            => (!pattern1.Match(x).IsSuccessful || !pattern2.Match(x).IsSuccessful ==
                !(pattern1 & pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldBeSameAsEitherPattern(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2,
            string x)
            => (pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful ==
                pattern1.Or(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorOrPatternShouldBeSameAsEitherPattern(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2,
            string x)
            => (pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful ==
                (pattern1 | pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldBeSameAsExcusiveEitherPattern(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2,
            string x)
            => (pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful ==
                pattern1.Xor(pattern2).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorXorPatternShouldBeSameAsExlusiveEitherPattern(
            SimplePattern<string> pattern1,
            SimplePattern<string> pattern2,
            string x)
            => (pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful ==
                (pattern1 ^ pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotPatternShouldBeOppositeToPattern(SimplePattern<string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property NotValueNullShouldBeOppositeToValueNull(int? x)
            => (Pattern.ValueNull<int>().Match(x).IsSuccessful ==
                !Pattern.Not(Pattern.ValueNull<int>()).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorNotPatternShouldBeOppositeToPattern(SimplePattern<string> pattern, string x)
            => (pattern.Match(x).IsSuccessful == !(~pattern).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property OperatorNotValueNullShouldBeOppositeToValueNull(int? x)
            => (Pattern.ValueNull<int>().Match(x).IsSuccessful ==
                !(~Pattern.ValueNull<int>()).Match(x).IsSuccessful)
                .ToProperty();

        [Fact]
        public void NotTypeShouldFailOnlyWhenTheValueHasType()
        {
            Pattern.Not(Pattern.Type<object, int>()).Match(1).IsSuccessful.Should().BeFalse();
            Pattern.Not(Pattern.Type<object, string>()).Match("string").IsSuccessful.Should().BeFalse();
            Pattern.Not(Pattern.Type<object, object>()).Match(null).IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void NotShouldThrowIfPatternIsNull()
        {
            Action action = () => Pattern.Not<object, object>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void OperatorNotTypeShouldFailOnlyWhenTheValueHasType()
        {
            (~Pattern.Type<object, int>()).Match(1).IsSuccessful.Should().BeFalse();
            (~Pattern.Type<object, string>()).Match("string").IsSuccessful.Should().BeFalse();
            (~Pattern.Type<object, object>()).Match(null).IsSuccessful.Should().BeTrue();
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void OperatorNotShouldThrowIfPatternIsNull()
        {
            Pattern<object, object> pattern = null;
            Action action = () => { var _ = ~pattern; };
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
