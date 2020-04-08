using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class LessThanTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property]
        public Property LessThanShouldSucceedOnlyOnLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property LazyLessThanShouldSucceedOnlyOnLess(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessThanWithComparerShouldSucceedOnlyOnLess(string x, string y)
            => (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessThanWithComparerShouldSucceedOnlyOnLess(string x, string y)
            => (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(() => y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessThanWithDescriptionShouldSucceedOnlyOnLess(string x, string y, string description)
        {
            Func<bool> lessThanSucceedsOnlyOnLess = () =>
                Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y, description).Match(x).IsSuccessful;
            return lessThanSucceedsOnlyOnLess.When(description != null);
        }

        [Property]
        public Property LazyLessThanWithDescriptionShouldSucceedOnlyOnLess(
            string x,
            string y,
            string description)
        {
            Func<bool> lessThanSucceedsOnlyOnLess = () =>
                Comparer<string>.Default.Compare(x, y) < 0 ==
                Pattern.LessThan(() => y, description).Match(x).IsSuccessful;
            return lessThanSucceedsOnlyOnLess.When(description != null);
        }

        [Property]
        public Property LessThanWithComparerAndDescriptionShouldSucceedOnlyOnLess(
            string x,
            string y,
            string description)
        {
            Func<bool> lessThanSucceedsOnlyOnLess = () =>
                StringComparer.Compare(x, y) < 0 ==
                Pattern.LessThan(y, StringComparer, description).Match(x).IsSuccessful;
            return lessThanSucceedsOnlyOnLess.When(description != null);
        }

        [Property]
        public Property LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnLess(
            string x,
            string y,
            string description)
        {
            Func<bool> lessThanSucceedsOnlyOnLess = () =>
                StringComparer.Compare(x, y) < 0 ==
                Pattern.LessThan(() => y, StringComparer, description).Match(x).IsSuccessful;
            return lessThanSucceedsOnlyOnLess.When(description != null);
        }

        [Property]
        public Property LessThanShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessThan(x).Description == String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessThanShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessThan(() => x).Description == Pattern.DefaultLazyLessThanDescription).ToProperty();

        [Property]
        public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessThan(x, StringComparer).Description ==
                String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessThan(() => x, StringComparer).Description == Pattern.DefaultLazyLessThanDescription)
                .ToProperty();

        [Property]
        public Property LessThanShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessThanHasCorrectDefaultDescription = () =>
                Pattern.LessThan(x, description).Description == description;
            return lessThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyLessThanShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessThanHasCorrectDefaultDescription = () =>
                Pattern.LessThan(() => x, description).Description == description;
            return lessThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LessThanWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessThanHasCorrectDefaultDescription = () =>
                Pattern.LessThan(x, StringComparer, description).Description == description;
            return lessThanHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyLessThanWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessThanHasCorrectDefaultDescription = () =>
                Pattern.LessThan(() => x, StringComparer, description).Description == description;
            return lessThanHasCorrectDefaultDescription.When(description != null);
        }

        [Fact]
        public void LazyLessThanShouldBeLazy()
        {
            Action action = () => Pattern.LessThan<string>(
                () => throw new AssertionFailedException("Lazy LessThan is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyLessThanWithComparerShouldBeLazy()
        {
            Action action = () => Pattern.LessThan(
                () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyLessThanWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan<string>(
                    () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan(
                    () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
                    StringComparer,
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LessThanShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.LessThan(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.LessThan(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LessThanShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan(x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan(() => x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LessThanShouldThrowIfComparerAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(x, null, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfComparerAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessThan(() => x, null, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessThan((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.LessThan((Func<string>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan((Func<string>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan((Func<string>)null, StringComparer, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderAndDescriptionIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.LessThan((Func<string>)null, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessThanShouldThrowIfValueProviderAndComparerIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessThan<string>((Func<string>)null, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void LazyLessThanShouldThrowIfValueProviderAndComparerAndDescriptionIsNull()
        {
            Action action = () => Pattern.LessThan<string>((Func<string>)null, null, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
