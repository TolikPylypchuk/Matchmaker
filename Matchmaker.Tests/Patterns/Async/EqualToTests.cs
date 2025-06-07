namespace Matchmaker.Patterns.Async;

public class EqualToTests
{
    private static readonly EqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

    [Property(DisplayName = "EqualTo should never return null")]
    public Property EqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should never return null")]
    public Property LazyEqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x) != null).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should never return null")]
    public Property EqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should never return null")]
    public Property LazyEqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer) != null).ToProperty();

    [Property(DisplayName = "EqualTo with description should never return null")]
    public Property EqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with description should never return null")]
    public Property LazyEqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "EqualTo with comparer and description should never return null")]
    public Property EqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer and description should never return null")]
    public Property LazyEqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "EqualTo should succeed only on equal objects")]
    public async Task<Property> EqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(y).MatchAsync(x)).IsSuccessful).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should succeed only on equal objects")]
    public async Task<Property> LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(() => y).MatchAsync(x)).IsSuccessful).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should succeed only on equal objects")]
    public async Task<Property> EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(y, StringEqualityComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should succeed only on equal objects")]
    public async Task<Property> LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(() => y, StringEqualityComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "EqualTo with description should succeed only on equal objects")]
    public async Task<Property> EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with description should succeed only on equal objects")]
    public async Task<Property> LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "EqualTo with comparer and description should succeed only on equal objects")]
    public async Task<Property> EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(y, StringEqualityComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer and description should succeed only on equal objects")]
    public async Task<Property> LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(() => y, StringEqualityComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "EqualTo should have correct default description")]
    public Property EqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo should have correct default description")]
    public Property LazyEqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x).Description == AsyncPattern.DefaultEqualToDescription).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should have correct default description")]
    public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should have correct default description")]
    public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer).Description ==
            AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property(DisplayName = "EqualTo should have specified description")]
    public Property EqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should have specified description")]
    public Property LazyEqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should have specified description")]
    public Property EqualToWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should have specified description")]
    public Property LazyEqualToWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy EqualTo should be lazy")]
    public void LazyEqualToShouldBeLazy()
    {
        var exception = Record.Exception(() =>
            AsyncPattern.EqualTo<string>(() => throw new InvalidOperationException("Lazy EqualTo is not lazy")));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "Lazy EqualTo with comparer should be lazy")]
    public void LazyEqualToWithComparerShouldBeLazy()
    {
        var exception = Record.Exception(() => AsyncPattern.EqualTo(
            () => throw new InvalidOperationException("Lazy EqualTo is not lazy"),
            StringEqualityComparer));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy EqualTo with description should be lazy")]
    public void LazyEqualToWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => AsyncPattern.EqualTo<string>(
            () => throw new InvalidOperationException("Lazy EqualTo is not lazy"),
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy EqualTo with comparer and description should be lazy")]
    public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => AsyncPattern.EqualTo(
            () => throw new InvalidOperationException("Lazy EqualTo is not lazy"),
            StringEqualityComparer,
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy EqualTo should be memoized")]
    public async Task<Property> LazyEqualToShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy EqualTo with comparer should be memoized")]
    public async Task<Property> LazyEqualToWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            StringEqualityComparer);

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy EqualTo with description should be memoized")]
    public async Task<Property> LazyEqualToWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.EqualTo(
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

    [Property(DisplayName = "Lazy EqualTo with comparer and description should be memoized")]
    public async Task<Property> LazyEqualToWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
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

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "EqualTo should throw if comparer is null")]
    public void EqualToShouldThrowIfComparerIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(x, (IEqualityComparer<string>)null));

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is null")]
    public void LazyEqualToShouldThrowIfComparerIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(() => x, (IEqualityComparer<string>)null));

    [Property(DisplayName = "EqualTo should throw if description is null")]
    public void EqualToShouldThrowIfDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(x, (string)null));

    [Property(DisplayName = "Lazy EqualTo should throw if description is null")]
    public void LazyEqualToShouldThrowIfDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(() => x, (string)null));

    [Property(DisplayName = "EqualTo should throw if comparer is null and description is not null")]
    public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(x, null, description.Get));

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is null and description is not null")]
    public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(() => x, null, description.Get));

    [Property(DisplayName = "EqualTo should throw if comparer is not null and description is null")]
    public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(x, StringEqualityComparer, null));

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is not null and description is null")]
    public void LazyEqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo(() => x, StringEqualityComparer, null));

    [Fact(DisplayName = "Lazy EqualTo should throw if value provider is null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo((Func<Task<int>>)null));

    [Fact(DisplayName = "Lazy EqualTo should throw if value provider is null and comparer is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNull() =>
        Assert.Throws<ArgumentNullException>(() =>
            AsyncPattern.EqualTo((Func<Task<string>>)null, StringEqualityComparer));

    [Property(DisplayName = "Lazy EqualTo should throw if value provider is null and description is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => AsyncPattern.EqualTo((Func<Task<string>>)null, description.Get));

    [Property(DisplayName = "Lazy EqualTo should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNullAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            AsyncPattern.EqualTo((Func<Task<string>>)null, StringEqualityComparer, description.Get));
}
