namespace Matchmaker.Patterns;

public class EqualToTests
{
    private static readonly EqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

    [Property(DisplayName = "EqualTo should never return null")]
    public Property EqualToShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(x) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should never return null")]
    public Property LazyEqualToShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(() => x) != null).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should never return null")]
    public Property EqualToWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(x, StringEqualityComparer) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should never return null")]
    public Property LazyEqualToWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer) != null).ToProperty();

    [Property(DisplayName = "EqualTo with description should never return null")]
    public Property EqualToWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with description should never return null")]
    public Property LazyEqualToWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "EqualTo with comparer and description should never return null")]
    public Property EqualToWithComparerAndDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer and description should never return null")]
    public Property LazyEqualToWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer, description.Get) != null).ToProperty();

    [Property(DisplayName = "EqualTo should succeed only on equal objects")]
    public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (Equals(x, y) == Pattern.EqualTo(y).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should succeed only on equal objects")]
    public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (Equals(x, y) == Pattern.EqualTo(() => y).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should succeed only on equal objects")]
    public Property EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(y, StringEqualityComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should succeed only on equal objects")]
    public Property LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(() => y, StringEqualityComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "EqualTo with description should succeed only on equal objects")]
    public Property EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (Equals(x, y) == Pattern.EqualTo(y, description.Get).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Lazy EqualTo with description should succeed only on equal objects")]
    public Property LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (Equals(x, y) == Pattern.EqualTo(() => y, description.Get).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "EqualTo with comparer and description should succeed only on equal objects")]
    public Property EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(y, StringEqualityComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer and description should succeed only on equal objects")]
    public Property LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
        string x,
        string y,
        NonNull<string> description) =>
        (StringEqualityComparer.Equals(x, y) ==
            Pattern.EqualTo(() => y, StringEqualityComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "EqualTo should have correct default description")]
    public Property EqualToShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(x).Description == String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo should have correct default description")]
    public Property LazyEqualToShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(() => x).Description == Pattern.DefaultLazyEqualToDescription).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should have correct default description")]
    public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(x, StringEqualityComparer).Description ==
            String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should have correct default description")]
    public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer).Description == Pattern.DefaultLazyEqualToDescription)
            .ToProperty();

    [Property(DisplayName = "EqualTo should have specified description")]
    public Property EqualToShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Lazy EqualTo should have specified description")]
    public Property LazyEqualToShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "EqualTo with comparer should have specified description")]
    public Property EqualToWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Property(DisplayName = "Lazy EqualTo with comparer should have specified description")]
    public Property LazyEqualToWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.EqualTo(() => x, StringEqualityComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy EqualTo should be lazy")]
    public void LazyEqualToShouldBeLazy()
    {
        var action = () => Pattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact(DisplayName = "Lazy EqualTo with comparer should be lazy")]
    public void LazyEqualToWithComparerShouldBeLazy()
    {
        var action = () => Pattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy EqualTo with description should be lazy")]
    public void LazyEqualToWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo<string>(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy EqualTo with comparer and description should be lazy")]
    public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(
            () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
            StringEqualityComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy EqualTo should be memoized")]
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

    [Property(DisplayName = "Lazy EqualTo with comparer should be memoized")]
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

    [Property(DisplayName = "Lazy EqualTo with description should be memoized")]
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

    [Property(DisplayName = "Lazy EqualTo with comparer and description should be memoized")]
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

    [Property(DisplayName = "EqualTo should throw if comparer is null")]
    public void EqualToShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is null")]
    public void LazyEqualToShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, (IEqualityComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "EqualTo should throw if description is null")]
    public void EqualToShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if description is null")]
    public void LazyEqualToShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "EqualTo should throw if comparer is null and description is not null")]
    public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is null and description is not null")]
    public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, NonNull<string> description)
    {
        var action = () => Pattern.EqualTo(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "EqualTo should throw if comparer is not null and description is null")]
    public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if comparer is not null and description is null")]
    public void LazyEqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.EqualTo(() => x, StringEqualityComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy EqualTo should throw if value provider is null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.EqualTo((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Lazy EqualTo should throw if value provider is null and comparer is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if value provider is null and description is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(NonNull<string> description)
    {
        var action = () => Pattern.EqualTo((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy EqualTo should throw if value provider is null " +
        "and comparer is not null and description is not null")]
    public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
