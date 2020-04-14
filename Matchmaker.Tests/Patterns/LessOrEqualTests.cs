using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class LessOrEqualTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property]
        public Property LessOrEqualShouldSucceedOnlyOnLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldSucceedOnlyOnLessOrEqual(string x, string y)
            => (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessOrEqualWithComparerShouldSucceedOnlyOnLessOrEqual(string x, string y)
            => (StringComparer.Compare(x, y) <= 0 == Pattern.LessOrEqual(y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerShouldSucceedOnlyOnLessOrEqual(string x, string y)
            => (StringComparer.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y, StringComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(string x, string y, string description)
        {
            Func<bool> lessOrEqualSucceedsOnlyOnLessOrEqual = () =>
                Comparer<string>.Default.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(y, description).Match(x).IsSuccessful;
            return lessOrEqualSucceedsOnlyOnLessOrEqual.When(description != null);
        }

        [Property]
        public Property LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> lessOrEqualSucceedsOnlyOnLessOrEqual = () =>
                Comparer<string>.Default.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(() => y, description).Match(x).IsSuccessful;
            return lessOrEqualSucceedsOnlyOnLessOrEqual.When(description != null);
        }

        [Property]
        public Property LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> lessOrEqualSucceedsOnlyOnLessOrEqual = () =>
                StringComparer.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(y, StringComparer, description).Match(x).IsSuccessful;
            return lessOrEqualSucceedsOnlyOnLessOrEqual.When(description != null);
        }

        [Property]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            string description)
        {
            Func<bool> lessOrEqualSucceedsOnlyOnLessOrEqual = () =>
                StringComparer.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(() => y, StringComparer, description).Match(x).IsSuccessful;
            return lessOrEqualSucceedsOnlyOnLessOrEqual.When(description != null);
        }

        [Property]
        public Property LessOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(x).Description == String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(() => x).Description == Pattern.DefaultLazyLessOrEqualDescription).ToProperty();

        [Property]
        public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(x, StringComparer).Description ==
                String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(() => x, StringComparer).Description == Pattern.DefaultLazyLessOrEqualDescription)
                .ToProperty();

        [Property]
        public Property LessOrEqualShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessOrEqualHasCorrectDefaultDescription = () =>
                Pattern.LessOrEqual(x, description).Description == description;
            return lessOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyLessOrEqualShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessOrEqualHasCorrectDefaultDescription = () =>
                Pattern.LessOrEqual(() => x, description).Description == description;
            return lessOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessOrEqualHasCorrectDefaultDescription = () =>
                Pattern.LessOrEqual(x, StringComparer, description).Description == description;
            return lessOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> lessOrEqualHasCorrectDefaultDescription = () =>
                Pattern.LessOrEqual(() => x, StringComparer, description).Description == description;
            return lessOrEqualHasCorrectDefaultDescription.When(description != null);
        }

        [Fact]
        public void LazyLessOrEqualShouldBeLazy()
        {
            Action action = () => Pattern.LessOrEqual<string>(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyLessOrEqualWithComparerShouldBeLazy()
        {
            Action action = () => Pattern.LessOrEqual(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyLessOrEqualWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual<string>(
                    () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual(
                    () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                    StringComparer,
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public Property LazyLessOrEqualShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = Pattern.LessOrEqual(() =>
            {
                counter++;
                return String.Empty;
            });

            pattern.Match(input);
            pattern.Match(input);

            return (counter == 1).ToProperty();
        }

        [Property]
        public Property LazyLessOrEqualWithComparerShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = Pattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return String.Empty;
                },
                StringComparer);

            pattern.Match(input);
            pattern.Match(input);

            return (counter == 1).ToProperty();
        }

        [Property]
        public Property LazyLessOrEqualWithDescriptionShouldBeMemoized(string input, string description)
        {
            Func<bool> lazyReturnIsMemoized = () =>
            {
                int counter = 0;

                var pattern = Pattern.LessOrEqual(
                    () =>
                    {
                        counter++;
                        return String.Empty;
                    },
                    description);

                pattern.Match(input);
                pattern.Match(input);

                return counter == 1;
            };

            return lazyReturnIsMemoized.When(description != null);
        }

        [Property]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldBeMemoized(string input, string description)
        {
            Func<bool> lazyReturnIsMemoized = () =>
            {
                int counter = 0;

                var pattern = Pattern.LessOrEqual(
                    () =>
                    {
                        counter++;
                        return String.Empty;
                    },
                    StringComparer,
                    description);

                pattern.Match(input);
                pattern.Match(input);

                return counter == 1;
            };

            return lazyReturnIsMemoized.When(description != null);
        }

        [Property]
        public void LessOrEqualShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LessOrEqualShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual(x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual(() => x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.LessOrEqual(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.LessOrEqual((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.LessOrEqual((Func<string>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual((Func<string>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.LessOrEqual((Func<string>)null, StringComparer, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
