namespace Matchmaker.Patterns;

public class LessThanTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "LessThan should never return null")]
    public Property LessThanShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(x) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan should never return null")]
    public Property LazyLessThanShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(() => x) != null).ToProperty();

    [Property(DisplayName = "LessThan with comparer should never return null")]
    public Property LessThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should never return null")]
    public Property LazyLessThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "LessThan with description should never return null")]
    public Property LessThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with description should never return null")]
    public Property LazyLessThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessThan with comparer and description should never return null")]
    public Property LessThanWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer and description should never return null")]
    public Property LazyLessThanWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.LessThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "LessThan should succeed only on lesser objects")]
    public Property LessThanShouldSucceedOnlyOnLessThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan should succeed only on lesser objects")]
    public Property LazyLessThanShouldSucceedOnlyOnLessThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with comparer should succeed only on lesser objects")]
    public Property LessThanWithComparerShouldSucceedOnlyOnLessThan(string x, string y) =>
        (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should succeed only on lesser objects")]
    public Property LazyLessThanWithComparerShouldSucceedOnlyOnLessThan(string x, string y) =>
        (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with description should succeed only on lesser objects")]
    public Property LessThanWithDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) < 0 ==
            Pattern.LessThan(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with description should succeed only on lesser objects")]
    public Property LazyLessThanWithDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) < 0 ==
            Pattern.LessThan(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan with comparer and description should succeed only on lesser objects")]
    public Property LessThanWithComparerAndDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) < 0 ==
            Pattern.LessThan(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer and description should succeed only on lesser objects")]
    public Property LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) < 0 ==
            Pattern.LessThan(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "LessThan should have correct default description")]
    public Property LessThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(x).Description ==
            String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan should have correct default description")]
    public Property LazyLessThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(() => x).Description == Pattern.DefaultLazyLessThanDescription)
            .ToProperty();

    [Property(DisplayName = "LessThan with comparer should have correct default description")]
    public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(x, StringComparer).Description ==
            String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should have correct default description")]
    public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(() => x, StringComparer).Description ==
            Pattern.DefaultLazyLessThanDescription)
            .ToProperty();

    [Property(DisplayName = "LessThan should have specified description")]
    public Property LessThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessThan should have specified description")]
    public Property LazyLessThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "LessThan with comparer should have specified description")]
    public Property LessThanWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy LessThan with comparer should have specified description")]
    public Property LazyLessThanWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.LessThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy LessThan should be lazy")]
    public void LazyLessThanShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.LessThan<string>(
            () => throw new InvalidOperationException("Lazy LessThan is not lazy")));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "Lazy LessThan with comparer should be lazy")]
    public void LazyLessThanWithComparerShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.LessThan(
            () => throw new InvalidOperationException("Lazy LessThan is not lazy"),
            StringComparer));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessThan with description should be lazy")]
    public void LazyLessThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.LessThan<string>(
            () => throw new InvalidOperationException("Lazy LessThan is not lazy"),
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessThan with comparer and description should be lazy")]
    public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.LessThan(
            () => throw new InvalidOperationException("Lazy LessThan is not lazy"),
            StringComparer,
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy LessThan should be memoized")]
    public Property LazyLessThanShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.LessThan(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy LessThan with comparer should be memoized")]
    public Property LazyLessThanWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.LessThan(
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

    [Property(DisplayName = "Lazy LessThan with description should be memoized")]
    public Property LazyLessThanWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.LessThan(
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

    [Property(DisplayName = "Lazy LessThan with comparer and description should be memoized")]
    public Property LazyLessThanWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.LessThan(
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

    [Property(DisplayName = "LessThan should throw if comparer is null")]
    public void LessThanShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(x, (IComparer<string>)null));

    [Property(DisplayName = "Lazy LessThan should throw if comparer is null")]
    public void LazyLessThanShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(() => x, (IComparer<string>)null));

    [Property(DisplayName = "LessThan should throw if description is null")]
    public void LessThanShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(x, (string)null));

    [Property(DisplayName = "Lazy LessThan should throw if description is null")]
    public void LazyLessThanShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(() => x, (string)null));

    [Property(DisplayName = "LessThan should throw if comparer is null and description is not null")]
    public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(x, null, description.Get));

    [Property(DisplayName = "Lazy LessThan should throw if comparer is null and description is not null")]
    public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(() => x, null, description.Get));

    [Property(DisplayName = "LessThan should throw if comparer is not null and description is null")]
    public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(x, StringComparer, null));

    [Property(DisplayName = "Lazy LessThan should throw if comparer is not null and description is null")]
    public void LazyLessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan(() => x, StringComparer, null));

    [Fact(DisplayName = "Lazy LessThan should throw if value provider is null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan((Func<int>)null));

    [Fact(DisplayName = "Lazy LessThan should throw if value provider is null and comparer is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan((Func<string>)null, StringComparer));

    [Property(DisplayName = "Lazy LessThan should throw if value provider is null and description is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.LessThan((Func<string>)null, description.Get));

    [Property(DisplayName = "Lazy LessThan should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            Pattern.LessThan((Func<string>)null, StringComparer, description.Get));
}
