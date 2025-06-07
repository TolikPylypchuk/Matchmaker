namespace Matchmaker.Patterns;

public class PatternTests
{
    [Property]
    public Property SimplePatternShouldNeverReturnNull(Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate) != null).ToProperty();

    [Property]
    public Property PatternShouldNeverReturnNull(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher) != null).ToProperty();

    [Property]
    public Property SimplePatternWithDescriptionShouldNeverReturnNull(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (Pattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property]
    public Property PatternWithDescriptionShouldNeverReturnNull(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (Pattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Property]
    public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string x) =>
        (Pattern.CreatePattern(predicate).Match(x).IsSuccessful == predicate(x)).ToProperty();

    [Property]
    public Property PatternShouldMatchSameAsMatcher(Func<string, MatchResult<string>> matcher, string x) =>
        (Pattern.CreatePattern(matcher).Match(x) == matcher(x)).ToProperty();

    [Property]
    public Property SimplePatternShouldHaveCorrectDescription(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (Pattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

    [Property]
    public Property PatternShouldHaveCorrectDescription(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (Pattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

    [Property]
    public Property PatternToStringShouldReturnDescription(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (description.Get.Length > 0).ImpliesThat(() =>
            Pattern.CreatePattern(matcher, description.Get).ToString() == description.Get)
            .ToProperty();

    [Property]
    public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher).ToString() == Pattern.CreatePattern(matcher).GetType().ToString())
            .ToProperty();

    [Property]
    public Property SimplePatternCreateShouldNeverReturnNull(Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate) != null).ToProperty();

    [Property]
    public Property SimplePatternCreateWithDescriptionShouldNeverReturnNull(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (Pattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property]
    public Property PatternCreateShouldNeverReturnNull(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher) != null).ToProperty();

    [Property]
    public Property PatternCreateWithDescriptionShouldNeverReturnNull(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (Pattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Fact]
    public void SimplePatternCreateShouldThrowIfPredicateIsNull()
    {
        var createWithNull = () => Pattern.CreatePattern<string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SimplePatternCreateShouldThrowIfDescriptionIsNull(Func<string, bool> predicate)
    {
        var createWithNull = () => Pattern.CreatePattern(predicate, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void PatternCreateShouldThrowIfMatcherIsNull()
    {
        var createWithNull = () => Pattern.CreatePattern<string, string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PatternCreateShouldThrowIfDescriptionIsNull(Func<string, MatchResult<string>> matcher)
    {
        var createWithNull = () => Pattern.CreatePattern(matcher, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AnyShouldNeverReturnNull() =>
        Pattern.Any<string>().Should().NotBeNull();

    [Property]
    public Property AnyWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Any<string>(description.Get) != null).ToProperty();

    [Property]
    public Property AnyShouldAlwaysSucceed(string x) =>
        Pattern.Any<string>().Match(x).IsSuccessful.ToProperty();

    [Property]
    public Property AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description) =>
        Pattern.Any<string>(description.Get).Match(x).IsSuccessful.ToProperty();

    [Fact]
    public void AnyShouldHaveCorrectDefaultDescription() =>
        Pattern.Any<string>().Description.Should().Be(Pattern.DefaultAnyDescription);

    [Property]
    public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (Pattern.Any<string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void AnyShouldThrowIfDescriptionIsNull()
    {
        var action = () => Pattern.Any<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property ReturnShouldNeverReturnNull(string x) =>
        (Pattern.Return<string, string>(x) != null).ToProperty();

    [Property]
    public Property ReturnWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyReturnShouldNeverReturnNull(string x) =>
        (Pattern.Return<string, string>(() => x) != null).ToProperty();

    [Property]
    public Property LazyReturnWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(() => x, description.Get) != null).ToProperty();

    [Property]
    public Property ReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
    {
        var result = Pattern.Return<string, string>(y).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property]
    public Property ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        string y,
        NonNull<string> description)
    {
        var result = Pattern.Return<string, string>(y, description.Get).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property]
    public Property ReturnShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.Return<string, string>(x).Description ==
            String.Format(Pattern.DefaultReturnDescriptionFormat, x))
            .ToProperty();

    [Property]
    public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void ReturnShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.Return<string, string>(x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property LazyReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
    {
        var result = Pattern.Return<string, string>(() => y).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property]
    public Property LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        string y,
        NonNull<string> description)
    {
        var result = Pattern.Return<string, string>(() => y, description.Get).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property]
    public Property LazyReturnShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.Return<string, string>(() => x).Description == Pattern.DefaultLazyReturnDescription)
            .ToProperty();

    [Property]
    public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(() => x, description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void LazyReturnShouldBeLazy()
    {
        var action = () => Pattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => Pattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"), description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public Property LazyReturnShouldBeMemoized(string x)
    {
        int counter = 0;

        var pattern = Pattern.Return<string, string>(() =>
        {
            counter++;
            return String.Empty;
        });

        pattern.Match(x);
        pattern.Match(x);

        return (counter == 1).ToProperty();
    }

    [Property]
    public Property LazyReturnWithDescriptionShouldBeMemoized(string x, NonNull<string> description)
    {
        int counter = 0;

        var pattern = Pattern.Return<string, string>(
            () =>
            {
                counter++;
                return String.Empty;
            },
            description.Get);

        pattern.Match(x);
        pattern.Match(x);

        return (counter == 1).ToProperty();
    }

    [Property]
    public void LazyReturnShouldThrowIfDescriptionIsNull(string x)
    {
        var action = () => Pattern.Return<string, string>(() => x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NullShouldNeverReturnNull() =>
        Pattern.Null<string>().Should().NotBeNull();

    [Property]
    public Property NullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Null<string>(description.Get) != null).ToProperty();

    [Property]
    public Property NullShouldSucceedOnlyOnNull(string x) =>
        (x is null == Pattern.Null<string>().Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description) =>
        (x is null == Pattern.Null<string>(description.Get).Match(x).IsSuccessful).ToProperty();

    [Fact]
    public void NullShouldHaveCorrectDefaultDescription() =>
        Pattern.Null<string>().Description.Should().Be(Pattern.DefaultNullDescription);

    [Property]
    public Property NullShouldHaveSpecifiedDewcription(NonNull<string> description) =>
        (Pattern.Null<string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void NullShouldThrowIfDescriptionIsNull()
    {
        var action = () => Pattern.Null<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValueNullShouldNeverReturnNull() =>
        Pattern.ValueNull<int>().Should().NotBeNull();

    [Property]
    public Property ValueNullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.ValueNull<int>(description.Get) != null).ToProperty();

    [Property]
    public Property ValueNullShouldSucceedOnlyOnNull(int? x) =>
        (x is null == Pattern.ValueNull<int>().Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description) =>
        (x is null == Pattern.ValueNull<int>(description.Get).Match(x).IsSuccessful).ToProperty();

    [Fact]
    public void ValueNullShouldHaveCorrectDefaultDescription() =>
        Pattern.ValueNull<int>().Description.Should().Be(Pattern.DefaultNullDescription);

    [Property]
    public Property ValueNullShouldHaveSpecifiedDewcription(NonNull<string> description) =>
        (Pattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void ValueNullShouldThrowIfDescriptionIsNull()
    {
        var action = () => Pattern.ValueNull<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TypeShouldNeverReturnNull() =>
        Pattern.Type<object, string>().Should().NotBeNull();

    [Property]
    public Property TypeWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Type<object, string>(description.Get) != null).ToProperty();

    [Property]
    public Property TypeShouldSucceedOnlyWhenTheValueHasType(int value) =>
        (Pattern.Type<object, int>().Match(value).IsSuccessful &&
            !Pattern.Type<object, string>().Match(value).IsSuccessful)
            .ToProperty();

    [Fact]
    public void TypeShouldSucceedOnNull() =>
        Pattern.Type<object, string>().Match(null).IsSuccessful.Should().BeTrue();

    [Fact]
    public void TypeShouldSucceedOnNullableValueNull() =>
        Pattern.Type<object, int?>().Match(null).IsSuccessful.Should().BeTrue();

    [Fact]
    public void TypeShouldFailOnValueNull() =>
        Pattern.Type<object, int>().Match(null).IsSuccessful.Should().BeFalse();

    [Property]
    public Property TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(int value, NonNull<string> description) =>
        (Pattern.Type<object, int>(description.Get).Match(value).IsSuccessful &&
            !Pattern.Type<object, string>(description.Get).Match(value).IsSuccessful)
            .ToProperty();

    [Property]
    public Property TypeWithDescriptionShouldSucceedOnNull(NonNull<string> description) =>
        Pattern.Type<object, string>(description.Get).Match(null).IsSuccessful.ToProperty();

    [Property]
    public Property TypeWithDescriptionShouldSucceedOnNullableValueNull(NonNull<string> description) =>
        Pattern.Type<object, int?>(description.Get).Match(null).IsSuccessful.ToProperty();

    [Property]
    public Property TypeWithDescriptionShouldFailOnValueNull(NonNull<string> description) =>
        (!Pattern.Type<object, int>(description.Get).Match(null).IsSuccessful).ToProperty();

    [Fact]
    public void TypeShouldHaveCorrectDefaultDescription() =>
        Pattern.Type<object, int>().Description.Should().Be(
            String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(int)));

    [Property]
    public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (Pattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void TypeShouldThrowIfDescriptionIsNull()
    {
        var action = () => Pattern.Type<object, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property NotShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (Pattern.Not(pattern) != null).ToProperty();

    [Property]
    public Property NotWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (Pattern.Not(pattern, description.Get) != null).ToProperty();

    [Property]
    public Property NotShouldBeOppositeToPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

    [Property]
    public Property NotShouldHaveCorrectDescription(IPattern<string, string> pattern) =>
        (Pattern.Not(pattern).Description ==
            String.Format(Pattern.DefaultNotDescriptionFormat, pattern.Description))
            .ToProperty();

    [Property]
    public Property NotShouldHaveSpecifiedDescription(IPattern<string, string> pattern, NonNull<string> description) =>
        (Pattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, bool> predicate) =>
        (Pattern.Not(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Fact]
    public void NotShouldThrowIfPatternIsNull()
    {
        var action = () => Pattern.Not<object, object>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void NotShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
    {
        var action = () => Pattern.Not(pattern, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
