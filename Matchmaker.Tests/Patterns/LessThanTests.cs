namespace Matchmaker.Patterns;

public class LessThanTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property LessThanShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(x) != null).ToProperty();

    [Property]
    public Property LazyLessThanShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(() => x) != null).ToProperty();

    [Property]
    public Property LessThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.LessThan(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property LessThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property LessThanWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyLessThanWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.LessThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LessThanShouldSucceedOnlyOnLessThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyLessThanShouldSucceedOnlyOnLessThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) < 0 == Pattern.LessThan(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessThanWithComparerShouldSucceedOnlyOnLessThan(string x, string y) =>
        (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldSucceedOnlyOnLessThan(string x, string y) =>
        (StringComparer.Compare(x, y) < 0 == Pattern.LessThan(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessThanWithDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) < 0 ==
            Pattern.LessThan(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyLessThanWithDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) < 0 ==
            Pattern.LessThan(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessThanWithComparerAndDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) < 0 ==
            Pattern.LessThan(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyLessThanWithComparerAndDescriptionShouldSucceedOnlyOnLessThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) < 0 ==
            Pattern.LessThan(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LessThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(x).Description ==
            String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyLessThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(() => x).Description == Pattern.DefaultLazyLessThanDescription)
            .ToProperty();

    [Property]
    public Property LessThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(x, StringComparer).Description ==
            String.Format(Pattern.DefaultLessThanDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.LessThan(() => x, StringComparer).Description ==
            Pattern.DefaultLazyLessThanDescription)
            .ToProperty();

    [Property]
    public Property LessThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyLessThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LessThanWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.LessThan(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyLessThanWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.LessThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyLessThanShouldBeLazy()
    {
        var action = () => Pattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyLessThanWithComparerShouldBeLazy()
    {
        var action = () => Pattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyLessThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.LessThan<string>(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyLessThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.LessThan(
            () => throw new AssertionFailedException("Lazy LessThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void LessThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.LessThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.LessThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.LessThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.LessThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.LessThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.LessThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.LessThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.LessThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.LessThan((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.LessThan((Func<string>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.LessThan((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyLessThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.LessThan((Func<string>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
