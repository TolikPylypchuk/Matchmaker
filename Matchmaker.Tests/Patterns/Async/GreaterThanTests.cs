namespace Matchmaker.Patterns.Async;

public class GreaterThanTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "GreaterThan should never return null")]
    public Property GreaterThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(x) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should never return null")]
    public Property LazyGreaterThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should never return null")]
    public Property GreaterThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should never return null")]
    public Property LazyGreaterThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with description should never return null")]
    public Property GreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with description should never return null")]
    public Property LazyGreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer and description should never return null")]
    public Property GreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should never return null")]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterThan should succeed only on greater objects")]
    public async Task<Property> GreaterThanShouldSucceedOnlyOnGreaterObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should succeed only on greater objects")]
    public async Task<Property> LazyGreaterThanShouldSucceedOnlyOnGreaterObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should succeed only on greater objects")]
    public async Task<Property> GreaterThanWithComparerShouldSucceedOnlyOnGreaterObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should succeed only on greater objects")]
    public async Task<Property> LazyGreaterThanWithComparerShouldSucceedOnlyOnGreaterObjects(
        string x,
        Task<string> y) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with description should succeed only on greater objects")]
    public async Task<Property> GreaterThanWithDescriptionShouldSucceedOnlyOnGreaterObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with description should succeed only on greater objects")]
    public async Task<Property> LazyGreaterThanWithDescriptionShouldSucceedOnlyOnGreaterObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with comparer and description should succeed only on greater objects")]
    public async Task<Property> GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should succeed only on greater objects")]
    public async Task<Property> LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan should have correct default description")]
    public Property GreaterThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(x).Description == AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should have correct default description")]
    public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x).Description == AsyncPattern.DefaultGreaterThanDescription).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should have correct default description")]
    public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(x, StringComparer).Description == AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should have correct default description")]
    public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer).Description ==
            AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterThan should have specified description")]
    public Property GreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should have specified description")]
    public Property LazyGreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should have specified description")]
    public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should have specified description")]
    public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy GreaterThan should be lazy")]
    public void LazyGreaterThanShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan with comparer should be lazy")]
    public void LazyGreaterThanWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan with description should be lazy")]
    public void LazyGreaterThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should be lazy")]
    public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should be memoized")]
    public async Task<Property> LazyGreaterThanShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterThan(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy GreaterThan with comparer should be memoized")]
    public async Task<Property> LazyGreaterThanWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterThan(
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

    [Property(DisplayName = "Lazy GreaterThan with description should be memoized")]
    public async Task<Property> LazyGreaterThanWithDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.GreaterThan(
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

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should be memoized")]
    public async Task<Property> LazyGreaterThanWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
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

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "GreaterThan should throw if comparer is null")]
    public void GreaterThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if description is null")]
    public void GreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if description is null")]
    public void LazyGreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if comparer is null and description is not null")]
    public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is null and description is not null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if comparer is not null and description is null")]
    public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is not null and description is null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan should throw if value provider is null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan should throw if value provider is null and comparer is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if value provider is null and description is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
