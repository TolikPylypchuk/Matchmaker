namespace Matchmaker.Patterns.Async;

public class EqualToTests
{
    private static readonly IEqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

    [Property]
    public Property EqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x) != null).ToProperty();

    [Property]
    public Property LazyEqualToShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x) != null).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer) != null).ToProperty();

    [Property]
    public Property EqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property EqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> EqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(y).MatchAsync(x)).IsSuccessful).ToProperty();

    [Property]
    public async Task<Property> LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(() => y).MatchAsync(x)).IsSuccessful).ToProperty();

    [Property]
    public async Task<Property> EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(y, StringEqualityComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, Task<string> y) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(() => y, StringEqualityComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Equals(x, await y) == (await AsyncPattern.EqualTo(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(y, StringEqualityComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, await y) ==
            (await AsyncPattern.EqualTo(() => y, StringEqualityComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property EqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property]
    public Property LazyEqualToShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x).Description == AsyncPattern.DefaultEqualToDescription).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer).Description == AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.EqualTo(() => x, StringEqualityComparer).Description ==
            AsyncPattern.DefaultEqualToDescription)
            .ToProperty();

    [Property]
    public Property EqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyEqualToShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.EqualTo(x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
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

    [Property]
    public void LazyEqualToWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void EqualToShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(() => x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.EqualTo(x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
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

    [Property]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.EqualTo(
            (Func<Task<string>>)null, StringEqualityComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
