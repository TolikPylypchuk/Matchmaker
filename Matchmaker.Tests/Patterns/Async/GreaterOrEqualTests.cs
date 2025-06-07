namespace Matchmaker.Patterns.Async;

public class GreaterOrEqualTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property GreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> GreaterOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterOrEqualShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) >= 0 ==
            (await AsyncPattern.GreaterOrEqual(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer).Description == AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterOrEqual(() => x, StringComparer).Description ==
            AsyncPattern.DefaultGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterOrEqual(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
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

    [Property]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
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

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterOrEqual(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
