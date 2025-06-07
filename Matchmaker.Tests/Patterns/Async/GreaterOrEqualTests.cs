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
        var exception = Record.Exception(() => AsyncPattern.GreaterOrEqual<string>(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy")));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "Lazy GreaterOrEqual with comparer should be lazy")]
    public void LazyGreaterOrEqualWithComparerShouldBeLazy()
    {
        var exception = Record.Exception(() => AsyncPattern.GreaterOrEqual(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            StringComparer));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with description should be lazy")]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => AsyncPattern.GreaterOrEqual<string>(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should be lazy")]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => AsyncPattern.GreaterOrEqual(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get));

        Assert.Null(exception);
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
    public void GreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(x, (IComparer<string>)null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(() => x, (IComparer<string>)null));

    [Property(DisplayName = "GreaterOrEqual should throw if description is null")]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(x, (string)null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if description is null")]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(() => x, (string)null));

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is null and description is not null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(x, null, description.Get));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(() => x, null, description.Get));

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is not null and description is null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(x, StringComparer, null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is not null and description is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual(() => x, StringComparer, null));

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.GreaterOrEqual((Func<Task<int>>)null));

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and comparer is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull() =>
        Assert.Throws<ArgumentNullException>(() =>
            AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, StringComparer));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, description.Get));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, StringComparer, description.Get));
}
