namespace Matchmaker.Patterns.Async;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

public class GreaterOrEqualTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(y).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(() => y).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(y, StringComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(() => y, StringComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(() => y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y.Result) >= 0 ==
            AsyncPattern.GreaterOrEqual(() => y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x).Description == AsyncPattern.DefaultGreaterOrEqualDescription).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer).Description ==
            AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyGreaterOrEqualShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyGreaterOrEqualWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        pattern.MatchAsync(input);
        pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyGreaterOrEqualWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(
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
    public Property LazyGreaterOrEqualWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(
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
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(
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
    public void GreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
