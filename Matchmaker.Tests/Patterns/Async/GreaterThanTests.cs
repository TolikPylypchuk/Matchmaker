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
    public class GreaterThanTests
    {
        private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.GreaterThan(x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.GreaterThan(() => x) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.GreaterThan(x, StringComparer) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerShouldNeverReturnNull(Task<string> x)
            => (AsyncPattern.GreaterThan(() => x, StringComparer) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.GreaterThan(x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description)
            => (AsyncPattern.GreaterThan(() => x, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.GreaterThan(x, StringComparer, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (Comparer<string>.Default.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(y).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (Comparer<string>.Default.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(() => y).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (StringComparer.Compare(x, y.Result) > 0 ==
                    AsyncPattern.GreaterThan(y, StringComparer).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y)
            => (StringComparer.Compare(x, y.Result) > 0 ==
                    AsyncPattern.GreaterThan(() => y, StringComparer).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(y, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (Comparer<string>.Default.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(() => y, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            Task<string> y,
            NonNull<string> description)
            => (StringComparer.Compare(x, y.Result) > 0 ==
                AsyncPattern.GreaterThan(() => y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.GreaterThan(x).Description == AsyncPattern.DefaultGreaterThanDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.GreaterThan(() => x).Description == AsyncPattern.DefaultGreaterThanDescription).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.GreaterThan(x, StringComparer).Description == AsyncPattern.DefaultGreaterThanDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x)
            => (AsyncPattern.GreaterThan(() => x, StringComparer).Description ==
                AsyncPattern.DefaultGreaterThanDescription)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.GreaterThan(x, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.GreaterThan(() => x, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description)
            => (AsyncPattern.GreaterThan(x, StringComparer, description.Get).Description == description.Get)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(
            Task<string> x,
            NonNull<string> description)
            => (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get).Description == description.Get)
                .ToProperty();

        [Fact]
        public void LazyGreaterThanShouldBeLazy()
        {
            Action action = () => AsyncPattern.GreaterThan<string>(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyGreaterThanWithComparerShouldBeLazy()
        {
            Action action = () => AsyncPattern.GreaterThan(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                StringComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanWithDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan<string>(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan(
                () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
                StringComparer,
                description.Get);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = AsyncPattern.GreaterThan(() =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            });

            pattern.MatchAsync(input);
            pattern.MatchAsync(input);

            return (counter == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property LazyGreaterThanWithComparerShouldBeMemoized(string input)
        {
            int counter = 0;

            var pattern = AsyncPattern.GreaterThan(
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
        public Property LazyGreaterThanWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
        {
            int counter = 0;

            var pattern = AsyncPattern.GreaterThan(
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
        public Property LazyGreaterThanWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
        {
            int counter = 0;

            var pattern = AsyncPattern.GreaterThan(
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
        public void GreaterThanShouldThrowIfComparerIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfComparerIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(() => x, (IComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            Task<string> x,
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan(x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
            Task<string> x,
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan(() => x, null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
        {
            Action action = () => AsyncPattern.GreaterThan(() => x, StringComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
        {
            Action action = () => AsyncPattern.GreaterThan((Func<Task<int>>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, StringComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
            NonNull<string> description)
        {
            Action action = () => AsyncPattern.GreaterThan(
                (Func<Task<string>>)null, StringComparer, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
