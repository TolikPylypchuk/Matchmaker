namespace Matchmaker.Patterns.Async;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

public class LessThanTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(y).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(() => y).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(y, StringComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(() => y, StringComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(() => y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y.Result) < 0 ==
            AsyncPattern.LessThan(() => y, StringComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x).Description == AsyncPattern.DefaultLessThanDescription).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer).Description ==
            AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LessThanWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyLessThanShouldBeLazy()
    {
        var action = () => AsyncPattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyLessThanWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        pattern.MatchAsync(input);
        pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyLessThanWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(
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
    public Property LazyLessThanWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(
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
    public Property LazyLessThanWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(
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
    public void LessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.LessThan((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.LessThan((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
