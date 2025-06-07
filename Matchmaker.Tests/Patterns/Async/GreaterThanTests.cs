namespace Matchmaker.Patterns.Async;

public class GreaterThanTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property GreaterThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(x) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x) != null).ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property GreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> GreaterThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterThanShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterThanWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterThanWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) > 0 ==
            (await AsyncPattern.GreaterThan(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(x).Description == AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x).Description == AsyncPattern.DefaultGreaterThanDescription).ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(x, StringComparer).Description == AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer).Description ==
            AsyncPattern.DefaultGreaterThanDescription)
            .ToProperty();

    [Property]
    public Property GreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterThanShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.GreaterThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyGreaterThanShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyGreaterThanWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void GreaterThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.GreaterThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.GreaterThan(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
