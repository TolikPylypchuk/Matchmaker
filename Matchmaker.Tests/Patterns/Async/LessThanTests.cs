namespace Matchmaker.Patterns.Async;

public class LessThanTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "LessThan should never return null")]
    public Property LessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan should never return null")]
    public Property LazyLessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x) != null).ToProperty();

    [Property(DisplayName = "LessThan with comparer should never return null")]
    public Property LessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should never return null")]
    public Property LazyLessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "LessThan with description should never return null")]
    public Property LessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with description should never return null")]
    public Property LazyLessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessThan with comparer and description should never return null")]
    public Property LessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer and description should never return null")]
    public Property LazyLessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessThan should succeed only on lesser objects")]
    public async Task<Property> LessThanShouldSucceedOnlyOnLesserObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan should succeed only on lesser objects")]
    public async Task<Property> LazyLessThanShouldSucceedOnlyOnLesserObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with comparer should succeed only on lesser objects")]
    public async Task<Property> LessThanWithComparerShouldSucceedOnlyOnLesserObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should succeed only on lesser objects")]
    public async Task<Property> LazyLessThanWithComparerShouldSucceedOnlyOnLesserObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with description should succeed only on lesser objects")]
    public async Task<Property> LessThanWithDescriptionShouldSucceedOnlyOnLesserObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with description should succeed only on lesser objects")]
    public async Task<Property> LazyLessThanWithDescriptionShouldSucceedOnlyOnLesserObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with comparer and description should succeed only on lesser objects")]
    public async Task<Property> LessThanWithComparerAndDescriptionShouldSucceedOnlyOnLesserObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer and description should succeed only on lesser objects")]
    public async Task<Property> LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnLesserObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan should have correct default description")]
    public Property LessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan should have correct default description")]
    public Property LazyLessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x).Description == AsyncPattern.DefaultLessThanDescription).ToProperty();

    [Property(DisplayName = "LessThan with comparer should have correct default description")]
    public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should have correct default description")]
    public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer).Description ==
            AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property(DisplayName = "LessThan should have specified description")]
    public Property LessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessThan should have specified description")]
    public Property LazyLessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "LessThan with comparer should have specified description")]
    public Property LessThanWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should have specified description")]
    public Property LazyLessThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy LessThan should be lazy")]
    public void LazyLessThanShouldBeLazy()
    {
        var action = () => AsyncPattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy LessThan with comparer should be lazy")]
    public void LazyLessThanWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessThan with description should be lazy")]
    public void LazyLessThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessThan with comparer and description should be lazy")]
    public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessThan should be memoized")]
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

    [Property(DisplayName = "Lazy LessThan with comparer should be memoized")]
    public async Task<Property> LazyLessThanWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(
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

    [Property(DisplayName = "Lazy LessThan with description should be memoized")]
    public async Task<Property> LazyLessThanWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessThan(
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

    [Property(DisplayName = "Lazy LessThan with comparer and description should be memoized")]
    public async Task<Property> LazyLessThanWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
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

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "LessThan should throw if comparer is null")]
    public void LessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if comparer is null")]
    public void LazyLessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessThan should throw if description is null")]
    public void LessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if description is null")]
    public void LazyLessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessThan should throw if comparer is null and description is not null")]
    public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if comparer is null and description is not null")]
    public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessThan should throw if comparer is not null and description is null")]
    public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if comparer is not null and description is null")]
    public void LazyLessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy LessThan should throw if value provider is null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.LessThan((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy LessThan should throw if value provider is null and comparer is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.LessThan((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if value provider is null and description is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessThan should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
