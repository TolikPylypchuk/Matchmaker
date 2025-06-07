namespace Matchmaker.Patterns;

public class LessOrEqualTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "LessOrEqual should never return null")]
    public Property LessOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.LessOrEqual(x) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should never return null")]
    public Property LazyLessOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.LessOrEqual(() => x) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should never return null")]
    public Property LessOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessOrEqual(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should never return null")]
    public Property LazyLessOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with description should never return null")]
    public Property LessOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with description should never return null")]
    public Property LazyLessOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer and description should never return null")]
    public Property LessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should never return null")]
    public Property LazyLessOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.LessOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessOrEqual should succeed only on lesser or equal objects")]
    public Property LessOrEqualShouldSucceedOnlyOnLessOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should succeed only on lesser or equal objects")]
    public Property LazyLessOrEqualShouldSucceedOnlyOnLessOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should succeed only on lesser or equal objects")]
    public Property LessOrEqualWithComparerShouldSucceedOnlyOnLessOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) <= 0 == Pattern.LessOrEqual(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should succeed only on lesser or equal objects")]
    public Property LazyLessOrEqualWithComparerShouldSucceedOnlyOnLessOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) <= 0 == Pattern.LessOrEqual(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with description should succeed only on lesser or equal objects")]
    public Property LessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) <= 0 ==
            Pattern.LessOrEqual(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with description should succeed only on lesser or equal objects")]
    public Property LazyLessOrEqualWithDescriptionShouldSucceedOnlyOnLessOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) <= 0 ==
            Pattern.LessOrEqual(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer and description should succeed only " +
        "on lesser or equal objects")]
    public Property LessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) <= 0 ==
            Pattern.LessOrEqual(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should succeed only " +
        "on lesser or equal objects")]
    public Property LazyLessOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnLessOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) <= 0 ==
            Pattern.LessOrEqual(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual should have correct default description")]
    public Property LessOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessOrEqual(x).Description ==
            String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should have correct default description")]
    public Property LazyLessOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessOrEqual(() => x).Description == Pattern.DefaultLazyLessOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should have correct default description")]
    public Property LessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessOrEqual(x, StringComparer).Description ==
            String.Format(Pattern.DefaultLessOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should have correct default description")]
    public Property LazyLessOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessOrEqual(() => x, StringComparer).Description ==
            Pattern.DefaultLazyLessOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "LessOrEqual should have specified description")]
    public Property LessOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual should have specified description")]
    public Property LazyLessOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "LessOrEqual with comparer should have specified description")]
    public Property LessOrEqualWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessOrEqual(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessOrEqual with comparer should have specified description")]
    public Property LazyLessOrEqualWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.LessOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy LessOrEqual should be lazy")]
    public void LazyLessOrEqualShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.LessOrEqual<string>(
            () => throw new InvalidOperationException("Lazy LessOrEqual is not lazy")));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "Lazy LessOrEqual with comparer should be lazy")]
    public void LazyLessOrEqualWithComparerShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.LessOrEqual(
            () => throw new InvalidOperationException("Lazy LessOrEqual is not lazy"),
            StringComparer));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessOrEqual with description should be lazy")]
    public void LazyLessOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.LessOrEqual<string>(
            () => throw new InvalidOperationException("Lazy LessOrEqual is not lazy"),
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should be lazy")]
    public void LazyLessOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.LessOrEqual(
            () => throw new InvalidOperationException("Lazy LessOrEqual is not lazy"),
            StringComparer,
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessOrEqual should be memoized")]
    public Property LazyLessOrEqualShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.LessOrEqual(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy LessOrEqual with comparer should be memoized")]
    public Property LazyLessOrEqualWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.LessOrEqual(
            () =>
            {
                counter++;
                return String.Empty;
            },
            StringComparer);

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy LessOrEqual with description should be memoized")]
    public Property LazyLessOrEqualWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.LessOrEqual(
            () =>
            {
                counter++;
                return String.Empty;
            },
            description.Get);

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy LessOrEqual with comparer and description should be memoized")]
    public Property LazyLessOrEqualWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.LessOrEqual(
            () =>
            {
                counter++;
                return String.Empty;
            },
            StringComparer,
            description.Get);

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "LessOrEqual should throw if comparer is null")]
    public void LessOrEqualShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(x, (IComparer<string>)null));

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(() => x, (IComparer<string>)null));

    [Property(DisplayName = "LessOrEqual should throw if description is null")]
    public void LessOrEqualShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(x, (string)null));

    [Property(DisplayName = "Lazy LessOrEqual should throw if description is null")]
    public void LazyLessOrEqualShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(() => x, (string)null));

    [Property(DisplayName = "LessOrEqual should throw if comparer is null and description is not null")]
    public void LessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(x, null, description.Get));

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(() => x, null, description.Get));

    [Property(DisplayName = "LessOrEqual should throw if comparer is not null and description is null")]
    public void LessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(x, StringComparer, null));

    [Property(DisplayName = "Lazy LessOrEqual should throw if comparer is not null and description is null")]
    public void LazyLessOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual(() => x, StringComparer, null));

    [Fact(DisplayName = "Lazy LessOrEqual should throw if value provider is null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual((Func<int>)null));

    [Fact(DisplayName = "Lazy LessOrEqual should throw if value provider is null and comparer is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual((Func<string>)null, StringComparer));

    [Property(DisplayName = "Lazy LessOrEqual should throw if value provider is null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessOrEqual((Func<string>)null, description.Get));

    [Property(DisplayName = "Lazy LessOrEqual should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyLessOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            Pattern.LessOrEqual((Func<string>)null, StringComparer, description.Get));
}
