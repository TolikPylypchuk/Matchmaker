namespace Matchmaker.Patterns;

public class GreaterOrEqualTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "GreaterOrEqual should never return null")]
    public Property GreaterOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(x) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should never return null")]
    public Property LazyGreaterOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(() => x) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should never return null")]
    public Property GreaterOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should never return null")]
    public Property LazyGreaterOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with description should never return null")]
    public Property GreaterOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with description should never return null")]
    public Property LazyGreaterOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer and description should never return null")]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should never return null")]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterOrEqual should succeed only on greater or equal objects")]
    public Property GreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should succeed only on greater or equal objects")]
    public Property LazyGreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should succeed only on greater or equal objects")]
    public Property GreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should succeed only on greater or equal objects")]
    public Property LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with description should succeed only on greater or equal objects")]
    public Property GreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with description should succeed only on greater or equal objects")]
    public Property LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer and description should succeed only " +
        "on greater or equal objects")]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should succeed only " +
        "on greater or equal objects")]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual should have correct default description")]
    public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(x).Description ==
            String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should have correct default description")]
    public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(() => x).Description == Pattern.DefaultLazyGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should have correct default description")]
    public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(x, StringComparer).Description ==
            String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should have correct default description")]
    public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer).Description ==
            Pattern.DefaultLazyGreaterOrEqualDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterOrEqual should have specified description")]
    public Property GreaterOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual should have specified description")]
    public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "GreaterOrEqual with comparer should have specified description")]
    public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should have specified description")]
    public Property LazyGreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy GreaterOrEqual should be lazy")]
    public void LazyGreaterOrEqualShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.GreaterOrEqual<string>(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy")));

        Assert.Null(exception);
    }

    [Fact(DisplayName = "Lazy GreaterOrEqual with comparer should be lazy")]
    public void LazyGreaterOrEqualWithComparerShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.GreaterOrEqual(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            StringComparer));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with description should be lazy")]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.GreaterOrEqual<string>(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should be lazy")]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.GreaterOrEqual(
            () => throw new InvalidOperationException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy GreaterOrEqual should be memoized")]
    public Property LazyGreaterOrEqualShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.GreaterOrEqual(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer should be memoized")]
    public Property LazyGreaterOrEqualWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.GreaterOrEqual(
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

    [Property(DisplayName = "Lazy GreaterOrEqual with description should be memoized")]
    public Property LazyGreaterOrEqualWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.GreaterOrEqual(
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

    [Property(DisplayName = "Lazy GreaterOrEqual with comparer and description should be memoized")]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.GreaterOrEqual(
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

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(x, (IComparer<string>)null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(() => x, (IComparer<string>)null));

    [Property(DisplayName = "GreaterOrEqual should throw if description is null")]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(x, (string)null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if description is null")]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(() => x, (string)null));

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is null and description is not null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(x, null, description.Get));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(() => x, null, description.Get));

    [Property(DisplayName = "GreaterOrEqual should throw if comparer is not null and description is null")]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(x, StringComparer, null));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if comparer is not null and description is null")]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual(() => x, StringComparer, null));

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual((Func<int>)null));

    [Fact(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and comparer is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual((Func<string>)null, StringComparer));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.GreaterOrEqual((Func<string>)null, description.Get));

    [Property(DisplayName = "Lazy GreaterOrEqual should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description) =>
        Assert.Throws<ArgumentNullException>(() =>
            Pattern.GreaterOrEqual((Func<string>)null, StringComparer, description.Get));
}
