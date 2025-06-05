namespace Matchmaker.Patterns;

public class GreaterThanTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property GreaterThanShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(x) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(() => x) != null).ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property GreaterThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterThanWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterThanShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterThanWithDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) > 0 ==
            Pattern.GreaterThan(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) > 0 ==
            Pattern.GreaterThan(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) > 0 ==
            Pattern.GreaterThan(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) > 0 ==
            Pattern.GreaterThan(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(x).Description ==
            String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(() => x).Description == Pattern.DefaultLazyGreaterThanDescription)
            .ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(x, StringComparer).Description ==
            String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(() => x, StringComparer).Description ==
            Pattern.DefaultLazyGreaterThanDescription)
            .ToProperty();

    [Property]
    public Property GreaterThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyGreaterThanShouldBeLazy()
    {
        var action = () => Pattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyGreaterThanWithComparerShouldBeLazy()
    {
        var action = () => Pattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public Property LazyGreaterThanShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.GreaterThan(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property]
    public Property LazyGreaterThanWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.GreaterThan(
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
    public Property LazyGreaterThanWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.GreaterThan(
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
    public Property LazyGreaterThanWithComparerAndDescriptionShouldBeMemoized(
        string input,
        NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.GreaterThan(
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
    public void GreaterThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.GreaterThan((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
