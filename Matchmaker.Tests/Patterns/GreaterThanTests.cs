namespace Matchmaker.Patterns;

public class GreaterThanTests
{
    private static readonly Comparer<string> StringComparer = Comparer<string>.Default;

    [Property(DisplayName = "GreaterThan should never return null")]
    public Property GreaterThanShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(x) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should never return null")]
    public Property LazyGreaterThanShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(() => x) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should never return null")]
    public Property GreaterThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should never return null")]
    public Property LazyGreaterThanWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterThan(() => x, StringComparer) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with description should never return null")]
    public Property GreaterThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with description should never return null")]
    public Property LazyGreaterThanWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer and description should never return null")]
    public Property GreaterThanWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should never return null")]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "GreaterThan should succeed only on greater objects")]
    public Property GreaterThanShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should succeed only on greater objects")]
    public Property LazyGreaterThanShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) > 0 == Pattern.GreaterThan(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should succeed only on greater objects")]
    public Property GreaterThanWithComparerShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should succeed only on greater objects")]
    public Property LazyGreaterThanWithComparerShouldSucceedOnlyOnGreaterThan(string x, string y) =>
        (StringComparer.Compare(x, y) > 0 == Pattern.GreaterThan(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with description should succeed only on greater objects")]
    public Property GreaterThanWithDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) > 0 ==
            Pattern.GreaterThan(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with description should succeed only on greater objects")]
    public Property LazyGreaterThanWithDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) > 0 ==
            Pattern.GreaterThan(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with comparer and description should succeed only on greater objects")]
    public Property GreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) > 0 ==
            Pattern.GreaterThan(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should succeed only on greater objects")]
    public Property LazyGreaterThanWithComparerAndDescriptionShouldSucceedOnlyOnGreaterThan(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) > 0 ==
            Pattern.GreaterThan(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "GreaterThan should have correct default description")]
    public Property GreaterThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(x).Description ==
            String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should have correct default description")]
    public Property LazyGreaterThanShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(() => x).Description == Pattern.DefaultLazyGreaterThanDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should have correct default description")]
    public Property GreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(x, StringComparer).Description ==
            String.Format(Pattern.DefaultGreaterThanDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should have correct default description")]
    public Property LazyGreaterThanWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterThan(() => x, StringComparer).Description ==
            Pattern.DefaultLazyGreaterThanDescription)
            .ToProperty();

    [Property(DisplayName = "GreaterThan should have specified description")]
    public Property GreaterThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan should have specified description")]
    public Property LazyGreaterThanShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "GreaterThan with comparer should have specified description")]
    public Property GreaterThanWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterThan(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy GreaterThan with comparer should have specified description")]
    public Property LazyGreaterThanWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterThan(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy GreaterThan should be lazy")]
    public void LazyGreaterThanShouldBeLazy()
    {
        var action = () => Pattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan with comparer should be lazy")]
    public void LazyGreaterThanWithComparerShouldBeLazy()
    {
        var action = () => Pattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan with description should be lazy")]
    public void LazyGreaterThanWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan<string>(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should be lazy")]
    public void LazyGreaterThanWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(
            () => throw new AssertionFailedException("Lazy GreaterThan is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should be memoized")]
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

    [Property(DisplayName = "Lazy GreaterThan with comparer should be memoized")]
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

    [Property(DisplayName = "Lazy GreaterThan with description should be memoized")]
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

    [Property(DisplayName = "Lazy GreaterThan with comparer and description should be memoized")]
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

    [Property(DisplayName = "GreaterThan should throw if comparer is null")]
    public void GreaterThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if description is null")]
    public void GreaterThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if description is null")]
    public void LazyGreaterThanShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if comparer is null and description is not null")]
    public void GreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is null and description is not null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "GreaterThan should throw if comparer is not null and description is null")]
    public void GreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if comparer is not null and description is null")]
    public void LazyGreaterThanShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterThan(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan should throw if value provider is null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.GreaterThan((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy GreaterThan should throw if value provider is null and comparer is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if value provider is null and description is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy GreaterThan should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyGreaterThanShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterThan((Func<string>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
