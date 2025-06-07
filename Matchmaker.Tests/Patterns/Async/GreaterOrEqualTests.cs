namespace Matchmaker.Patterns.Async;

public class GreaterOrEqualTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "GreaterOrEqual should never return null")]
    public Property GreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should never return null")]
    public Property LazyGreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should never return null")]
    public Property GreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should never return null")]
    public Property LazyGreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with description should never return null")]
    public Property GreaterOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with description should never return null")]
    public Property LazyGreaterOrEqualWithDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer and description should never return null")]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should never return null")]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual should succeed only on greater or equal objects")]
    public async Task<Property> GreaterOrEqualShouldSucceedOnlyOnGreaterOrEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should succeed only on greater or equal objects")]
    public async Task<Property> LazyGreaterOrEqualShouldSucceedOnlyOnGreaterOrEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should succeed only on greater or equal objects")]
    public async Task<Property> GreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterEqualObjects(
        string x,
        Task<string> y) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should succeed only on greater or equal objects")]
    public async Task<Property> LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqualObjects(
        string x,
        Task<string> y) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with description should succeed only on greater or equal objects")]
    public async Task<Property> GreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with description should succeed only on greater or equal objects")]
    public async Task<Property> LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer and description should succeed only " +
        "on greater or equal objects")]
    public async Task<Property> GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should succeed only " +
        "on greater or equal objects")]
    public async Task<Property> LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual should have correct default description")]
    public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should have correct default description")]
    public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should have correct default description")]
    public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should have correct default description")]
    public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer).Description ==
            AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual should have specified description")]
    public Property GreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should have specified description")]
    public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should have specified description")]
    public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should have specified description")]
    public Property LazyGreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy GreaterOrEqual should be lazy")]
    public void LazyGreaterOrEqualShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy GreaterOrEqual with comparer should be lazy")]
    public void LazyGreaterOrEqualWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with description should be lazy")]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should be lazy")]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should be memoized")]
    public async Task<Property> LazyGreaterOrEqualShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should be memoized")]
    public async Task<Property> LazyGreaterOrEqualWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            StringComparer);

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with description should be memoized")]
    public async Task<Property> LazyGreaterOrEqualWithDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterOrEqual(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            description.Get);

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should be memoized")]
    public async Task<Property> LazyGreaterOrEqualWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
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

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterOrEqual should throw if description is null")]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if description is null")]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is null and description is not null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is not null and description is null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is not null and description is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and comparer is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
