namespace Matchmaker.Patterns;

using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

public class GreaterOrEqualTests
{
    private static readonly IComparer<string> StringComparer = Comparer<string>.Default;

    [Property]
    public Property GreaterOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(x) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(() => x) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(x, StringComparer) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldNeverReturnNull(string x) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldNeverReturnNull(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer, description.Get) != null).ToProperty();

    [Property]
    public Property GreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldSucceedOnlyOnGreaterOrEqual(string x, string y) =>
        (StringComparer.Compare(x, y) >= 0 == Pattern.GreaterOrEqual(() => y, StringComparer).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (Comparer<string>.Default.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(() => y, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerAndDescriptionShouldSucceedOnlyOnGreaterOrEqual(
        string x,
        string y,
        NonNull<string> description) =>
        (StringComparer.Compare(x, y) >= 0 ==
            Pattern.GreaterOrEqual(() => y, StringComparer, description.Get).Match(x).IsSuccessful)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(x).Description ==
            String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(() => x).Description == Pattern.DefaultLazyGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(x, StringComparer).Description ==
            String.Format(Pattern.DefaultGreaterOrEqualDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer).Description ==
            Pattern.DefaultLazyGreaterOrEqualDescription)
            .ToProperty();

    [Property]
    public Property GreaterOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property GreaterOrEqualWithComparerShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.GreaterOrEqual(x, StringComparer, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property LazyGreaterOrEqualWithComparerShouldHaveSpecifiedDescription(
        string x,
        NonNull<string> description) =>
        (Pattern.GreaterOrEqual(() => x, StringComparer, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyGreaterOrEqualShouldBeLazy()
    {
        var action = () => Pattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Fact]
    public void LazyGreaterOrEqualWithComparerShouldBeLazy()
    {
        var action = () => Pattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterOrEqualWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual<string>(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyGreaterOrEqualWithComparerAndDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual(
            () => throw new AssertionFailedException("Lazy GreaterOrEqual is not lazy"),
            StringComparer,
            description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
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

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(() => x, (IComparer<string>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(() => x, (string)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual(x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNullAndDescriptionIsNotNull(
        string x,
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual(() => x, null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void GreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
    {
        var action = () => Pattern.GreaterOrEqual(() => x, StringComparer, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNull()
    {
        var action = () => Pattern.GreaterOrEqual((Func<int>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
    {
        var action = () => Pattern.GreaterOrEqual((Func<string>)null, StringComparer);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual((Func<string>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void LazyGreaterOrEqualShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(
        NonNull<string> description)
    {
        var action = () => Pattern.GreaterOrEqual((Func<string>)null, StringComparer, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }
}
