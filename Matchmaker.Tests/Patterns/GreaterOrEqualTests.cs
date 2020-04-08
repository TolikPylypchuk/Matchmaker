using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class GreaterOrEqualTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property]
        public Property GreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property GreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y)
            => (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y)
            => (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property GreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(string x, string y, string description)
        {
            Func<bool> greaterOrEqualSucceedsOnlyOnGreaterOrEqual = () =>
                Comparer<string>.Default.Compare(x, y) >= 0 ==
                Pattern.GreaterOrEqual(y, description).Match(x).IsSuccessful;
            return greaterOrEqualSucceedsOnlyOnGreaterOrEqual.When(description != null);
        }

        [Property]
        public Property LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterOrEqualSucceedsOnlyOnGreaterOrEqual = () =>
                Comparer<string>.Default.Compare(x, y) >= 0 ==
                Pattern.GreaterOrEqual(() => y, description).Match(x).IsSuccessful;
            return greaterOrEqualSucceedsOnlyOnGreaterOrEqual.When(description != null);
        }

        [Property]
        public Property GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterOrEqualSucceedsOnlyOnGreaterOrEqual = () =>
                StringComparer.Compare(x, y) >= 0 ==
                Pattern.GreaterOrEqual(y, StringComparer, description).Match(x).IsSuccessful;
            return greaterOrEqualSucceedsOnlyOnGreaterOrEqual.When(description != null);
        }

        [Property]
        public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterOrEqualSucceedsOnlyOnGreaterOrEqual = () =>
                StringComparer.Compare(x, y) >= 0 ==
                Pattern.GreaterOrEqual(() => y, StringComparer, description).Match(x).IsSuccessful;
            return greaterOrEqualSucceedsOnlyOnGreaterOrEqual.When(description != null);
        }

        [Property]
        public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterOrEqual(x).Description == String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterOrEqual(() => x).Description == Pattern.DefaultLazyGreaterOrEqualDescription).ToProperty();

        [Property]
        public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterOrEqual(x, StringComparer).Description ==
                String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterOrEqual(() => x, StringComparer).Description == Pattern.DefaultLazyGreaterOrEqualDescription)
                .ToProperty();

        [Property]
        public Property GreaterOrEqualShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterOrEqualHasCorrectDefaultDescription = () =>
                Pattern.GreaterOrEqual(x, description).Description == description;
            return greaterOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterOrEqualHasCorrectDefaultDescription = () =>
                Pattern.GreaterOrEqual(() => x, description).Description == description;
            return greaterOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterOrEqualHasCorrectDefaultDescription = () =>
                Pattern.GreaterOrEqual(x, StringComparer, description).Description == description;
            return greaterOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyGreaterOrEqualWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterOrEqualHasCorrectDefaultDescription = () =>
                Pattern.GreaterOrEqual(() => x, StringComparer, description).Description == description;
            return greaterOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Fact]
        public void LazyGreaterOrEqualShouldBeLazy()
        {
            Action action = () => Pattern.GreaterOrEqual<string>(
                () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyGreaterOrEqualWithComparerShouldBeLazy()
        {
            Action action = () => Pattern.GreaterOrEqual(
                () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual<string>(
                    () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual(
                    () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
                    StringComparer,
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void GreaterOrEqualShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GreaterOrEqualShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual(x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual(() => x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GreaterOrEqualShouldThrowIfComparerAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(x, null, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfComparerAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterOrEqual(() => x, null, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.GreaterOrEqual((Func<string>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual((Func<string>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual((Func<string>)null, StringComparer, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderAndDescriptionIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.GreaterOrEqual((Func<string>)null, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderAndComparerIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterOrEqual<string>((Func<string>)null, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void LazyGreaterOrEqualShouldThrowIfValueProviderAndComparerAndDescriptionIsNull()
        {
            Action action = () => Pattern.GreaterOrEqual<string>((Func<string>)null, null, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
