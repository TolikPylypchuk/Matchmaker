namespace Matchmaker.Patterns;

public class EqualToTests
{
    private static readonly IEqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

    [Property]
    public Property EqualToShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(x) != null).ToProperty();

    [Property]
    public Property LazyEqualToShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(() => x) != null).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(x, StringEqualityComparer) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer) != null).ToProperty();

    [Property]
    public Property EqualToWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property EqualToWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyEqualToWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property]
    public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (Equals(x, y) == Pattern.EqualTo(y).Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (Equals(x, y) == Pattern.EqualTo(() => y).Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(y, StringEqualityComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(() => y, StringEqualityComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (Equals(x, y) == Pattern.EqualTo(y, description.Get).Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (Equals(x, y) == Pattern.EqualTo(() => y, description.Get).Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(y, StringEqualityComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(() => y, StringEqualityComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property EqualToShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(x).Description == String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyEqualToShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(() => x).Description == Pattern.DefaultLazyEqualToDescription).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(x, StringEqualityComparer).Description ==
            String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer).Description == Pattern.DefaultLazyEqualToDescription)
            .ToProperty();

    [Property]
    public Property EqualToShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyEqualToShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property EqualToWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property]
    public Property LazyEqualToWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyEqualToShouldBeLazy()
    {
        var action = () => Pattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyEqualToWithComparerShouldBeLazy()
    {
        var action = () => Pattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyEqualToWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public Property LazyEqualToShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.EqualTo(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property]
    public Property LazyEqualToWithComparerShouldBeMemoized(string input)
    {
        int counter = 0;

        var pattern = Pattern.EqualTo(
            () =>
            {
                counter++;
                return String.Empty;
            },
            StringEqualityComparer);

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property]
    public Property LazyEqualToWithDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.EqualTo(
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
    public Property LazyEqualToWithComparerAndDescriptionShouldBeMemoized(string input, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.EqualTo(
            () =>
            {
                counter++;
                return String.Empty;
            },
            StringEqualityComparer,
            description.Get);

        pattern.Match(input);
        pattern.Match(input);

        return (counter == 1).ToProperty();
    }

    [Property]
    public void EqualToShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyEqualToShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.EqualTo((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
