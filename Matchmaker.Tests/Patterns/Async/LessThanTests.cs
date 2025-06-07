namespace Matchmaker.Patterns.Async;

public class LessThanTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property LessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x) != null).ToProperty();

    [Property]
    public Property LazyLessThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x) != null).ToProperty();

    [Property]
    public Property LessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property LessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property LessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> LessThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) < 0 ==
            (await AsyncPattern.LessThan(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property]
    public Property LazyLessThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x).Description == AsyncPattern.DefaultLessThanDescription).ToProperty();

    [Property]
    public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(x, StringComparer).Description == AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessThan(() => x, StringComparer).Description ==
            AsyncPattern.DefaultLessThanDescription)
            .ToProperty();

    [Property]
    public Property LessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyLessThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LessThanWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessThan(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
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

    [Property]
    public void LazyLessThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void LessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
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

    [Property]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessThan(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
