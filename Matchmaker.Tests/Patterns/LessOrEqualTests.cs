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
        public Property LessOrEqualShouldNeverReturnNull(string x)
            => (Pattern.LessOrEqual(x) != null).ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldNeverReturnNull(string x)
            => (Pattern.LessOrEqual(() => x) != null).ToProperty();

        [Property]
        public Property LessOrEqualWithComparerShouldNeverReturnNull(string x)
            => (Pattern.LessOrEqual(x, StringComparer) != null).ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerShouldNeverReturnNull(string x)
            => (Pattern.LessOrEqual(() => x, StringComparer) != null).ToProperty();

        [Property]
        public Property LessOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(x, description.Get) != null).ToProperty();

        [Property]
        public Property LazyLessOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(() => x, description.Get) != null).ToProperty();

        [Property]
        public Property LessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(x, StringComparer, description.Get) != null).ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
            string x,
            NonNull<string> description)
            => (Pattern.LessOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

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
        public Property LessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(y, description.Get).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(() => y, description.Get).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(y, StringComparer, description.Get).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
            string x,
            string y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y) <= 0 ==
                Pattern.LessOrEqual(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LessOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(x).Description ==
                String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(() => x).Description == Pattern.DefaultLazyLessOrEqualDescription)
                .ToProperty();

        [Property]
        public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(x, StringComparer).Description ==
                String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.LessOrEqual(() => x, StringComparer).Description ==
                Pattern.DefaultLazyLessOrEqualDescription)
                .ToProperty();

        [Property]
        public Property LessOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(x, description.Get).Description == description.Get).ToProperty();

        [Property]
        public Property LazyLessOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

        [Property]
        public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description)
            => (Pattern.LessOrEqual(x, StringComparer, description.Get).Description == description.Get).ToProperty();

        [Property]
        public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(
            string x,
            NonNull<string> description)
            => (Pattern.LessOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
                .ToProperty();

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
        public void LazyLessOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual<string>(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                StringComparer,
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
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
        public Property LazyLessOrEqualWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
        {
            int counter = 0;

            var pattern = Pattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return String.Empty;
                },
                description.Get);

            pattern.Match(input);
            pattern.Match(input);

            return (counter == 1).ToProperty();
        }

        [Property]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldBeMemoized(
            string input,
            NonNull<string> description)
        {
            int counter = 0;

            var pattern = Pattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return String.Empty;
                },
                StringComparer,
                description.Get);

            pattern.Match(input);
            pattern.Match(input);

            return (counter == 1).ToProperty();
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
        public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            string x,
            NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual(x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            string x,
            NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual(() => x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
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
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
            NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual((Func<string>)null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
            NonNull<string> description)
        {
            Action action = () => Pattern.LessOrEqual((Func<string>)null, StringComparer, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
