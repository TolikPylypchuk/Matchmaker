using System;
using System.Collections.Generic;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace PatternMatching
{
    public class PatternTests
    {
        [Property]
        public Property AnyShouldAlwaysSucceed(string x)
            => Pattern.Any<string>().Match(x).IsSome.ToProperty();

        [Property]
        public Property NullShouldSucceedOnlyOnNull(string x)
            => (x is null == Pattern.Null<string>().Match(x).IsSome).ToProperty();

        [Property]
        public Property StructNullShouldSucceedOnlyOnNull(int? x)
            => (x is null == Pattern.StructNull<int>().Match(x).IsSome).ToProperty();

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

        [Property]
        public Property NotAnyShouldAlwaysFail(string x)
            => Pattern.Not(Pattern.Any<string>()).Match(x).IsNone.ToProperty();

        [Property]
        public Property NotNullShouldFailOnlyOnNull(string x)
            => (x is null == Pattern.Not(Pattern.Null<string>()).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotStructNullShouldFailOnlyOnNull(int? x)
            => (x is null == Pattern.Not(Pattern.StructNull<int>()).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotEqualToShouldFailOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.Not(Pattern.EqualTo(y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLazyEqualToShouldFailOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.Not(Pattern.EqualTo(() => y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLessThanShouldFailOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.Not(Pattern.LessThan(y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLazyLessThanShouldFailOnlyWhenValueIsLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.Not(Pattern.LessThan(() => y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLessOrEqualShouldFailOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.Not(Pattern.LessOrEqual(y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLazyLessOrEqualShouldFailOnlyWhenValueIsLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.Not(Pattern.LessOrEqual(() => y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotGreaterThanShouldFailOnlyWhenValueIsLGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.Not(Pattern.GreaterThan(y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLazyGreaterThanShouldFailOnlyWhenValueIsGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.Not(Pattern.GreaterThan(() => y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotGreaterOrEqualShouldFailOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.Not(Pattern.GreaterOrEqual(y)).Match(x).IsNone).ToProperty();

        [Property]
        public Property NotLazyGreaterOrEqualShouldFailOnlyWhenValueIsGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.Not(Pattern.GreaterOrEqual(() => y)).Match(x).IsNone).ToProperty();

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
    }
}
