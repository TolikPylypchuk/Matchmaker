namespace Matchmaker.Patterns.Async;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

public class EqualToTests
{
    private static readonly IEqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, y.Result) == AsyncPattern.EqualTo(y).MatchAsync(x).Result.IsSuccessful).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, y.Result) == AsyncPattern.EqualTo(() => y).MatchAsync(x).Result.IsSuccessful).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, y.Result) ==
            AsyncPattern.EqualTo(y, StringEqualityComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, y.Result) ==
            AsyncPattern.EqualTo(() => y, StringEqualityComparer).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, y.Result) == AsyncPattern.EqualTo(y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, y.Result) == AsyncPattern.EqualTo(() => y, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y.Result) ==
            AsyncPattern.EqualTo(y, StringEqualityComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y.Result) ==
            AsyncPattern.EqualTo(() => y, StringEqualityComparer, description.Get).MatchAsync(x).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x).Description == AsyncPattern.DefaultEqualToDescription).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer).Description ==
            AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property EqualToWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyEqualToShouldBeLazy()
    {
        var action = () => AsyncPattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyEqualToWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        pattern.MatchAsync(input);
        pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            StringEqualityComparer);

        pattern.MatchAsync(input);
        pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property LazyEqualToWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(
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
    public Property LazyEqualToWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            StringEqualityComparer,
            description.Get);

        pattern.MatchAsync(input);
        pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void EqualToShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(() => x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void EqualToShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(() => x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyEqualToShouldThrowIfValueProviderIsNull()
    {
        Action action = () => AsyncPattern.EqualTo((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.EqualTo((Func<Task<string>>)null, StringEqualityComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(
            (Func<Task<string>>)null, StringEqualityComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
