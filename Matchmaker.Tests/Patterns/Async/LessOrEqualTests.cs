namespace Matchmaker.Patterns.Async;

public class LessOrEqualTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property LessOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x) != null).ToProperty();

    [Property]
    public Property LazyLessOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x) != null).ToProperty();

    [Property]
    public Property LessOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyLessOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property LessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property LessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> LessOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x).Description == AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property]
    public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x).Description == AsyncPattern.DefaultLessOrEqualDescription).ToProperty();

    [Property]
    public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x, StringComparer).Description == AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property]
    public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer).Description ==
            AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property]
    public Property LessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyLessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
    public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyLessOrEqualShouldBeLazy()
    {
        var action = () => AsyncPattern.LessOrEqual<string>(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyLessOrEqualWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.LessOrEqual(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyLessOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual<string>(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public async Task<Property> LazyLessOrEqualShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessOrEqual(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        await pattern.MatchAsync(input);
        await pattern.MatchAsync(input);

        return (counter == 1).ToProperty();
    }

    [Property]
    public async Task<Property> LazyLessOrEqualWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessOrEqual(
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
    public async Task<Property> LazyLessOrEqualWithDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessOrEqual(
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
    public async Task<Property> LazyLessOrEqualWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.LessOrEqual(
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
    public void LessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
