using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns.Async
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class LessOrEqualTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.LessOrEqual(x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.LessOrEqual(() => x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.LessOrEqual(x, StringComparer) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.LessOrEqual(() => x, StringComparer) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.LessOrEqual(x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.LessOrEqual(() => x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.LessOrEqual(x, StringComparer, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (Comparer<string>.Default.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(y).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (Comparer<string>.Default.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(() => y).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (StringComparer.Compare(x, y.Result) <= 0 ==
                    AsyncPattern.LessOrEqual(y, StringComparer).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (StringComparer.Compare(x, y.Result) <= 0 ==
                    AsyncPattern.LessOrEqual(() => y, StringComparer).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(y, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(() => y, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y.Result) <= 0 ==
                AsyncPattern.LessOrEqual(() => y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.LessOrEqual(x).Description == AsyncPattern.DefaultLessOrEqualDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.LessOrEqual(() => x).Description == AsyncPattern.DefaultLessOrEqualDescription).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.LessOrEqual(x, StringComparer).Description == AsyncPattern.DefaultLessOrEqualDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.LessOrEqual(() => x, StringComparer).Description ==
                AsyncPattern.DefaultLessOrEqualDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.LessOrEqual(x, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.LessOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.LessOrEqual(x, StringComparer, description.Get).Description == description.Get)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
                .ToProperty();

        [Fact]
        public void LazyLessOrEqualShouldBeLazy()
        {
            Action action = () => AsyncPattern.LessOrEqual<string>(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyLessOrEqualWithComparerShouldBeLazy()
        {
            Action action = () => AsyncPattern.LessOrEqual(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual<string>(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual(
                () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
                StringComparer,
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = AsyncPattern.LessOrEqual(() =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            });

            pattern.MatchAsync(input);
            pattern.MatchAsync(input);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = AsyncPattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return Task.FromResult(String.Empty);
                },
                StringComparer);

            pattern.MatchAsync(input);
            pattern.MatchAsync(input);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
        {
            int counter = 0;

            var pattern = AsyncPattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return Task.FromResult(String.Empty);
                },
                description.Get);

            pattern.MatchAsync(input);
            pattern.MatchAsync(input);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyLessOrEqualWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
        {
            int counter = 0;

            var pattern = AsyncPattern.LessOrEqual(
                () =>
                {
                    counter++;
                    return Task.FromResult(String.Empty);
                },
                StringComparer,
                description.Get);

            pattern.MatchAsync(input);
            pattern.MatchAsync(input);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            Task<string> x,
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual(x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            Task<string> x,
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual(() => x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.LessOrEqual(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
        {
            Action action = () => AsyncPattern.LessOrEqual((Func<Task<int>>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.LessOrEqual(
                (Func<Task<string>>)null, StringComparer, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
