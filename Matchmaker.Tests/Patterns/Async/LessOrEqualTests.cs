namespace Matchmaker.Patterns.Async;

public class LessOrEqualTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "LessOrEqual should never return null")]
    public Property LessOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should never return null")]
    public Property LazyLessOrEqualShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should never return null")]
    public Property LessOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should never return null")]
    public Property LazyLessOrEqualWithComparerShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with description should never return null")]
    public Property LessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with description should never return null")]
    public Property LazyLessOrEqualWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer and description should never return null")]
    public Property LessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should never return null")]
    public Property LazyLessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual should succeed only on lesser or equal objects")]
    public async Task<Property> LessOrEqualShouldSucceedOnlyOnLesserOrEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should succeed only on lesser or equal objects")]
    public async Task<Property> LazyLessOrEqualShouldSucceedOnlyOnLesserOrEqualObjects(string x, Task<string> y) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should succeed only on lesser or equal objects")]
    public async Task<Property> LessOrEqualWithComparerShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should succeed only on lesser or equal objects")]
    public async Task<Property> LazyLessOrEqualWithComparerShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, StringComparer).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with description should succeed only on lesser or equal objects")]
    public async Task<Property> LessOrEqualWithDescriptionShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with description should succeed only on lesser or equal objects")]
    public async Task<Property> LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer and description should succeed only " +
        "on lesser or equal objects")]
    public async Task<Property> LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should succeed only " +
        "on lesser or equal objects")]
    public async Task<Property> LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLesserOrEqualObjects(
        string x,
        Task<string> y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, await y) <= 0 ==
            (await AsyncPattern.LessOrEqual(() => y, StringComparer, description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual should have correct default description")]
    public Property LessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x).Description == AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should have correct default description")]
    public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x).Description == AsyncPattern.DefaultLessOrEqualDescription).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should have correct default description")]
    public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(x, StringComparer).Description == AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should have correct default description")]
    public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer).Description ==
            AsyncPattern.DefaultLessOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual should have specified description")]
    public Property LessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should have specified description")]
    public Property LazyLessOrEqualShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should have specified description")]
    public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should have specified description")]
    public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.LessOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy LessOrEqual should be lazy")]
    public void LazyLessOrEqualShouldBeLazy()
    {
        var action = () => AsyncPattern.LessOrEqual<string>(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy LessOrEqual with comparer should be lazy")]
    public void LazyLessOrEqualWithComparerShouldBeLazy()
    {
        var action = () => AsyncPattern.LessOrEqual(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual with description should be lazy")]
    public void LazyLessOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual<string>(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should be lazy")]
    public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(
            () => throw new AssertionFailedException("Lazy LessOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should be memoized")]
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

    [Property(DisplayName = "Lazy LessOrEqual with comparer should be memoized")]
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

    [Property(DisplayName = "Lazy LessOrEqual with description should be memoized")]
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

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should be memoized")]
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

    [Property(DisplayName = "LessOrEqual should throw if comparer is null")]
    public void LessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessOrEqual should throw if description is null")]
    public void LessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if description is null")]
    public void LazyLessOrEqualShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessOrEqual should throw if comparer is null and description is not null")]
    public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        Task<string> x,
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "LessOrEqual should throw if comparer is not null and description is null")]
    public void LessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is not null and description is null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.LessOrEqual(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy LessOrEqual should throw if value provider is null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNull()
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy LessOrEqual should throw if value provider is null and comparer is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if value provider is null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual((Func<Task<string>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy LessOrEqual should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => AsyncPattern.LessOrEqual(
            (Func<Task<string>>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
