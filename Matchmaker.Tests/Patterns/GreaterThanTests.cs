using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class GreaterThanTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property]
        public Property GreaterThanShouldSucceedOnlyOnGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterThanShouldSucceedOnlyOnGreater(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property GreaterThanWithComparerShouldSucceedOnlyOnGreater(string x, string y)
            => (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyGreaterThanWithComparerShouldSucceedOnlyOnGreater(string x, string y)
            => (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(() => y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property GreaterThanWithDescriptionShouldSucceedOnlyOnGreater(string x, string y, string description)
        {
            Func<bool> greaterThanSucceedsOnlyOnGreater = () =>
                Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y, description).Match(x).IsSuccessful;
            return greaterThanSucceedsOnlyOnGreater.When(description != null);
        }

        [Property]
        public Property LazyGreaterThanWithDescriptionShouldSucceedOnlyOnGreater(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterThanSucceedsOnlyOnGreater = () =>
                Comparer<string>.Default.Compare(x, y) > 0 ==
                Pattern.GreaterThan(() => y, description).Match(x).IsSuccessful;
            return greaterThanSucceedsOnlyOnGreater.When(description != null);
        }

        [Property]
        public Property GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreater(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterThanSucceedsOnlyOnGreater = () =>
                StringComparer.Compare(x, y) > 0 ==
                Pattern.GreaterThan(y, StringComparer, description).Match(x).IsSuccessful;
            return greaterThanSucceedsOnlyOnGreater.When(description != null);
        }

        [Property]
        public Property LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreater(
            string x,
            string y,
            string description)
        {
            Func<bool> greaterThanSucceedsOnlyOnGreater = () =>
                StringComparer.Compare(x, y) > 0 ==
                Pattern.GreaterThan(() => y, StringComparer, description).Match(x).IsSuccessful;
            return greaterThanSucceedsOnlyOnGreater.When(description != null);
        }

        [Property]
        public Property GreaterThanShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterThan(x).Description == String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterThan(() => x).Description == Pattern.DefaultLazyGreaterThanDescription).ToProperty();

        [Property]
        public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterThan(x, StringComparer).Description ==
                String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.GreaterThan(() => x, StringComparer).Description == Pattern.DefaultLazyGreaterThanDescription)
                .ToProperty();

        [Property]
        public Property GreaterThanShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterThanHasCorrectDefaultDescription = () =>
                Pattern.GreaterThan(x, description).Description == description;
            return greaterThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyGreaterThanShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterThanHasCorrectDefaultDescription = () =>
                Pattern.GreaterThan(() => x, description).Description == description;
            return greaterThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterThanHasCorrectDefaultDescription = () =>
                Pattern.GreaterThan(x, StringComparer, description).Description == description;
            return greaterThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> greaterThanHasCorrectDefaultDescription = () =>
                Pattern.GreaterThan(() => x, StringComparer, description).Description == description;
            return greaterThanHasCorrectDefaultDescription.When(description != null);
        }

        [Fact]
        public void LazyGreaterThanShouldBeLazy()
        {
            Action action = () => Pattern.GreaterThan<string>(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyGreaterThanWithComparerShouldBeLazy()
        {
            Action action = () => Pattern.GreaterThan(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyGreaterThanWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan<string>(
                    () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan(
                    () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                    StringComparer,
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void GreaterThanShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GreaterThanShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan(x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan(() => x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.GreaterThan(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.GreaterThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.GreaterThan((Func<string>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan((Func<string>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
            string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.GreaterThan((Func<string>)null, StringComparer, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
