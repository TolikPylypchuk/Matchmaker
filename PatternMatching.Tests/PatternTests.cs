using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using LanguageExt;

using Xunit;

namespace PatternMatching
{
    public class PatternTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string input)
            => (new SimplePattern<string>(predicate).Match(input).IsSome == predicate(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternShouldMatchSameAsMatcher(Func<string, OptionUnsafe<string>> matcher, string input)
            => (new Pattern<string, string>(matcher).Match(input) == matcher(input)).ToProperty();

        [Fact]
        public void SimplePatternConstructorShouldThrowForNull()
        {
            Action action = () => { var _ = new SimplePattern<string>(null); };
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatternConstructorShouldThrowForNull()
        {
            Action action = () => { var _ = new Pattern<string, string>(null); };
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SimplePatternWithWhenShouldMatchSameAsPredicates(
            List<Func<string, bool>> predicates,
            string input)
        {
            Func<bool> property = () => predicates
                .Skip(1)
                .Aggregate(
                    new SimplePattern<string>(predicates.First()),
                    (pattern, predicate) => pattern.When(predicate))
                .Match(input)
                .IsSome == predicates.All(predicate => predicate(input));

            return property.When(predicates != null && predicates.Count > 1);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PatternWithWhenShouldMatchSameAsMatcherAndPredicates(
            Func<string, OptionUnsafe<string>> matcher,
            List<Func<string, bool>> predicates,
            string input)
        {
            Func<bool> property = () => predicates
                .Aggregate(
                    new Pattern<string, string>(matcher),
                    (pattern, predicate) => pattern.When(predicate))
                .Match(input) == matcher(input).Filter(i => predicates.All(predicate => predicate(i)));

            return property.When(matcher != null && predicates != null && predicates.Count > 1);
        }

        [Property]
        public Property AnyShouldAlwaysSucceed(string x)
            => Pattern.Any<string>().Match(x).IsSome.ToProperty();

        [Property]
        public Property NullShouldSucceedOnlyOnNull(string x)
            => (x is null == Pattern.Null<string>().Match(x).IsSome).ToProperty();

        [Property]
        public Property ValueNullShouldSucceedOnlyOnNull(int? x)
            => (x is null == Pattern.ValueNull<int>().Match(x).IsSome).ToProperty();

        [Property]
        public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(y).Match(x).IsSome).ToProperty();

        [Property]
        public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(() => y).Match(x).IsSome).ToProperty();

        [Fact]
        public void LazyEqualToShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.EqualTo((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property LessThanShouldSucceedOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y).Match(x).IsSome).ToProperty();

        [Property]
        public Property LazyLessThanShouldSucceedOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(() => y).Match(x).IsSome).ToProperty();

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property LessOrEqualShouldSucceedOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(y).Match(x).IsSome).ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldSucceedOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y).Match(x).IsSome).ToProperty();

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property GreaterThanShouldSucceedOnlyWhenValueIsLGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y).Match(x).IsSome).ToProperty();

        [Property]
        public Property LazyGreaterThanShouldSucceedOnlyWhenValueIsGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(() => y).Match(x).IsSome).ToProperty();

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public Property GreaterOrEqualShouldSucceedOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y).Match(x).IsSome).ToProperty();

        [Property]
        public Property LazyGreaterOrEqualShouldSucceedOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y).Match(x).IsSome).ToProperty();

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TypeShouldSucceedOnlyWhenTheValueHasType()
        {
            Pattern.Type<object, int>().Match(1).IsSome.Should().BeTrue();
            Pattern.Type<object, string>().Match("string").IsSome.Should().BeTrue();
            Pattern.Type<object, object>().Match(null).IsSome.Should().BeFalse();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldBeSameAsBothPatterns(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsNone || pattern2.Match(x).IsNone == pattern1.And(pattern2).Match(x).IsNone).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorAndPatternShouldBeSameAsBothPatterns(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsNone || pattern2.Match(x).IsNone == (pattern1 & pattern2).Match(x).IsNone).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldBeSameAsEitherPattern(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsSome || pattern2.Match(x).IsSome == pattern1.Or(pattern2).Match(x).IsSome).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorOrPatternShouldBeSameAsEitherPattern(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsSome || pattern2.Match(x).IsSome == (pattern1 | pattern2).Match(x).IsSome).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldBeSameAsExcusiveEitherPattern(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsSome ^ pattern2.Match(x).IsSome == pattern1.Xor(pattern2).Match(x).IsSome).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorXorPatternShouldBeSameAsExlusiveEitherPattern(SimplePattern<string> pattern1, SimplePattern<string> pattern2, string x)
            => (pattern1.Match(x).IsSome ^ pattern2.Match(x).IsSome == (pattern1 ^ pattern2).Match(x).IsSome).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NotPatternShouldBeOppositeToPattern(SimplePattern<string> pattern, string x)
            => (pattern.Match(x).IsSome == Pattern.Not(pattern).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotValueNullShouldBeOppositeToValueNull(int? x)
            => (Pattern.ValueNull<int>().Match(x).IsSome == Pattern.Not(Pattern.ValueNull<int>()).Match(x).IsNone).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OperatorNotPatternShouldBeOppositeToPattern(SimplePattern<string> pattern, string x)
            => (pattern.Match(x).IsSome == (~pattern).Match(x).IsNone).ToProperty();

        [Property]
        public Property OperatorNotValueNullShouldBeOppositeToValueNull(int? x)
            => (Pattern.ValueNull<int>().Match(x).IsSome == (~Pattern.ValueNull<int>()).Match(x).IsNone).ToProperty();

        [Fact]
        public void NotTypeShouldFailOnlyWhenTheValueHasType()
        {
            Pattern.Not(Pattern.Type<object, int>()).Match(1).IsSome.Should().BeFalse();
            Pattern.Not(Pattern.Type<object, string>()).Match("string").IsSome.Should().BeFalse();
            Pattern.Not(Pattern.Type<object, object>()).Match(null).IsSome.Should().BeTrue();
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
            (~Pattern.Type<object, int>()).Match(1).IsSome.Should().BeFalse();
            (~Pattern.Type<object, string>()).Match("string").IsSome.Should().BeFalse();
            (~Pattern.Type<object, object>()).Match(null).IsSome.Should().BeTrue();
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
