namespace Matchmaker.Patterns;

public class PatternTests
{
    [Property(DisplayName = "Simple pattern should match the same as predicate")]
    public Property SimplePatternShouldMatchSameAsPredicate(Func<string, bool> predicate, string x) =>
        (Pattern.CreatePattern(predicate).Match(x).IsSuccessful == predicate(x)).ToProperty();

    [Property(DisplayName = "Pattern should match the same as matcher")]
    public Property PatternShouldMatchSameAsMatcher(Func<string, MatchResult<string>> matcher, string x) =>
        (Pattern.CreatePattern(matcher).Match(x) == matcher(x)).ToProperty();

    [Property(DisplayName = "Simple pattern should have correct description")]
    public Property SimplePatternShouldHaveCorrectDescription(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (Pattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Simple pattern should have empty description by default")]
    public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pattern should have correct description")]
    public Property PatternShouldHaveCorrectDescription(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (Pattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pattern should have empty description by default")]
    public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pattern.ToString() should return description")]
    public Property PatternToStringShouldReturnDescription(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (description.Get.Length > 0).ImpliesThat(() =>
            Pattern.CreatePattern(matcher, description.Get).ToString() == description.Get)
            .ToProperty();

    [Property(DisplayName = "Pattern.ToString() should return type when description is empty")]
    public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher).ToString() == Pattern.CreatePattern(matcher).GetType().ToString())
            .ToProperty();

    [Property(DisplayName = "Simple CreatePattern should never return null")]
    public Property SimpleCreatePatternShouldNeverReturnNull(Func<string, bool> predicate) =>
        (Pattern.CreatePattern(predicate) != null).ToProperty();

    [Property(DisplayName = "Simple CreatePattern with description should never return null")]
    public Property SimpleCreatePatternWithDescriptionShouldNeverReturnNull(
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (Pattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property(DisplayName = "CreatePattern should never return null")]
    public Property CreatePatternShouldNeverReturnNull(Func<string, MatchResult<string>> matcher) =>
        (Pattern.CreatePattern(matcher) != null).ToProperty();

    [Property(DisplayName = "CreatePattern with description should never return null")]
    public Property CreatePatternWithDescriptionShouldNeverReturnNull(
        Func<string, MatchResult<string>> matcher,
        NonNull<string> description) =>
        (Pattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Fact(DisplayName = "Simple CreatePattern should throw if predicate is null")]
    public void SimpleCreatePatternShouldThrowIfPredicateIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.CreatePattern<string>(null));

    [Property(DisplayName = "Simple CreatePattern should throw if description is null")]
    public void SimpleCreatePatternShouldThrowIfDescriptionIsNull(Func<string, bool> predicate) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.CreatePattern(predicate, null));

    [Fact(DisplayName = "CreatePattern should throw if matcher is null")]
    public void CreatePatternShouldThrowIfMatcherIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.CreatePattern<string, string>(null));

    [Property(DisplayName = "CreatePattern should throw if description is null")]
    public void CreatePatternShouldThrowIfDescriptionIsNull(Func<string, MatchResult<string>> matcher) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.CreatePattern(matcher, null));

    [Fact(DisplayName = "Any should never return null")]
    public void AnyShouldNeverReturnNull() =>
        Assert.NotNull(Pattern.Any<string>());

    [Property(DisplayName = "Any with description should never return null")]
    public Property AnyWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Any<string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Any should always succeed")]
    public Property AnyShouldAlwaysSucceed(string x) =>
        Pattern.Any<string>().Match(x).IsSuccessful.ToProperty();

    [Property(DisplayName = "Any with description should always succeed")]
    public Property AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description) =>
        Pattern.Any<string>(description.Get).Match(x).IsSuccessful.ToProperty();

    [Fact(DisplayName = "Any should have correct default description")]
    public void AnyShouldHaveCorrectDefaultDescription() =>
        Assert.Equal(Pattern.DefaultAnyDescription, Pattern.Any<string>().Description);

    [Property(DisplayName = "Any with description should have the specified description")]
    public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (Pattern.Any<string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Any should throw if description is null")]
    public void AnyShouldThrowIfDescriptionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Any<string>(null));

    [Property(DisplayName = "Return should never return null")]
    public Property ReturnShouldNeverReturnNull(string x) =>
        (Pattern.Return<string, string>(x) != null).ToProperty();

    [Property(DisplayName = "Return with description should never return null")]
    public Property ReturnWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy Return should never return null")]
    public Property LazyReturnShouldNeverReturnNull(string x) =>
        (Pattern.Return<string, string>(() => x) != null).ToProperty();

    [Property(DisplayName = "Lazy Return with description should never return null")]
    public Property LazyReturnWithDescriptionShouldNeverReturnNull(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Return should always return the specified value")]
    public Property ReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
    {
        var result = Pattern.Return<string, string>(y).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property(DisplayName = "Return with description should always return specified value")]
    public Property ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        string y,
        NonNull<string> description)
    {
        var result = Pattern.Return<string, string>(y, description.Get).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property(DisplayName = "Return should have correct default description")]
    public Property ReturnShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.Return<string, string>(x).Description ==
            String.Format(Pattern.DefaultReturnDescriptionFormat, x))
            .ToProperty();

    [Property(DisplayName = "Return with description should have the specified description")]
    public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Return should throw if description is null")]
    public void ReturnShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Return<string, string>(x, null));

    [Property(DisplayName = "Lazy Return should always return the pecified value")]
    public Property LazyReturnShouldAlwaysReturnSpecifiedValue(string x, string y)
    {
        var result = Pattern.Return<string, string>(() => y).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property(DisplayName = "Lazy Return with description should always return the specified value")]
    public Property LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        string y,
        NonNull<string> description)
    {
        var result = Pattern.Return<string, string>(() => y, description.Get).Match(x);
        return (result.IsSuccessful && result.Value == y).ToProperty();
    }

    [Property(DisplayName = "Lazy Return should have correct default description")]
    public Property LazyReturnShouldHaveCorrectDefaultDescription(string x) =>
        (Pattern.Return<string, string>(() => x).Description == Pattern.DefaultLazyReturnDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy Return with description should have the specified description")]
    public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(string x, NonNull<string> description) =>
        (Pattern.Return<string, string>(() => x, description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Lazy Return should be lazy")]
    public void LazyReturnShouldBeLazy()
    {
        var exception = Record.Exception(() => Pattern.Return<string, string>(
            () => throw new InvalidOperationException("Lazy Return is not lazy")));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy Return with description should be lazy")]
    public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var exception = Record.Exception(() => Pattern.Return<string, string>(
            () => throw new InvalidOperationException("Lazy Return is not lazy"), description.Get));

        Assert.Null(exception);
    }

    [Property(DisplayName = "Lazy Return should be memoized")]
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

    [Property(DisplayName = "Lazy Return with description should be memoized")]
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

    [Property(DisplayName = "Lazy Return should throw if description is null")]
    public void LazyReturnShouldThrowIfDescriptionIsNull(string x) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Return<string, string>(() => x, null));

    [Fact(DisplayName = "Null should never return null")]
    public void NullShouldNeverReturnNull() =>
        Assert.NotNull(Pattern.Null<string>());

    [Property(DisplayName = "Null with description should never return null")]
    public Property NullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Null<string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Null should succeed only on null")]
    public Property NullShouldSucceedOnlyOnNull(string x) =>
        (x is null == Pattern.Null<string>().Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Null with description should succeed only on null")]
    public Property NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description) =>
        (x is null == Pattern.Null<string>(description.Get).Match(x).IsSuccessful).ToProperty();

    [Fact(DisplayName = "Null should have correct default description")]
    public void NullShouldHaveCorrectDefaultDescription() =>
        Assert.Equal(Pattern.DefaultNullDescription, Pattern.Null<string>().Description);

    [Property(DisplayName = "Null should have the specified description")]
    public Property NullShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (Pattern.Null<string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Null should throw if description is null")]
    public void NullShouldThrowIfDescriptionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Null<string>(null));

    [Fact(DisplayName = "ValueNull should never return null")]
    public void ValueNullShouldNeverReturnNull() =>
        Assert.NotNull(Pattern.ValueNull<int>());

    [Property(DisplayName = "ValueNull with description should never return null")]
    public Property ValueNullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.ValueNull<int>(description.Get) != null).ToProperty();

    [Property(DisplayName = "ValueNull should succeed only on null")]
    public Property ValueNullShouldSucceedOnlyOnNull(int? x) =>
        (x is null == Pattern.ValueNull<int>().Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "ValueNull with description should succeed only on null")]
    public Property ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description) =>
        (x is null == Pattern.ValueNull<int>(description.Get).Match(x).IsSuccessful).ToProperty();

    [Fact(DisplayName = "ValueNull should have correct default description")]
    public void ValueNullShouldHaveCorrectDefaultDescription() =>
        Assert.Equal(Pattern.DefaultNullDescription, Pattern.ValueNull<int>().Description);

    [Property(DisplayName = "ValueNull should have the specified description")]
    public Property ValueNullShouldHaveSpecifiedDewcription(NonNull<string> description) =>
        (Pattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "ValueNull should throw if description is null")]
    public void ValueNullShouldThrowIfDescriptionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.ValueNull<int>(null));

    [Fact(DisplayName = "Type should never return null")]
    public void TypeShouldNeverReturnNull() =>
        Assert.NotNull(Pattern.Type<object, string>());

    [Property(DisplayName = "Type with description should never return null")]
    public Property TypeWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (Pattern.Type<object, string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Type should succeed only when the value has the specified type")]
    public Property TypeShouldSucceedOnlyWhenTheValueHasType(int value) =>
        (Pattern.Type<object, int>().Match(value).IsSuccessful &&
            !Pattern.Type<object, string>().Match(value).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "Type should succeed on null")]
    public void TypeShouldSucceedOnNull() =>
        Assert.True(Pattern.Type<object, string>().Match(null).IsSuccessful);

    [Fact(DisplayName = "Type should succeed on nullable value null")]
    public void TypeShouldSucceedOnNullableValueNull() =>
        Assert.True(Pattern.Type<object, int?>().Match(null).IsSuccessful);

    [Fact(DisplayName = "Type should fail on value null")]
    public void TypeShouldFailOnValueNull() =>
        Assert.False(Pattern.Type<object, int>().Match(null).IsSuccessful);

    [Property(DisplayName = "Type with description should succeed only when the value has the specified type")]
    public Property TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(int value, NonNull<string> description) =>
        (Pattern.Type<object, int>(description.Get).Match(value).IsSuccessful &&
            !Pattern.Type<object, string>(description.Get).Match(value).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Type with description should succeed on null")]
    public Property TypeWithDescriptionShouldSucceedOnNull(NonNull<string> description) =>
        Pattern.Type<object, string>(description.Get).Match(null).IsSuccessful.ToProperty();

    [Property(DisplayName = "Type with description should succeed on nullable value null")]
    public Property TypeWithDescriptionShouldSucceedOnNullableValueNull(NonNull<string> description) =>
        Pattern.Type<object, int?>(description.Get).Match(null).IsSuccessful.ToProperty();

    [Property(DisplayName = "Type with description should fail on value null")]
    public Property TypeWithDescriptionShouldFailOnValueNull(NonNull<string> description) =>
        (!Pattern.Type<object, int>(description.Get).Match(null).IsSuccessful).ToProperty();

    [Fact(DisplayName = "Type should have correct default description")]
    public void TypeShouldHaveCorrectDefaultDescription() =>
        Assert.Equal(
            String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(int)),
            Pattern.Type<object, int>().Description);

    [Property(DisplayName = "Type with description should have the specified description")]
    public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (Pattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Type should throw if description is null")]
    public void TypeShouldThrowIfDescriptionIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Type<object, int>(null));

    [Property(DisplayName = "Not should never return null")]
    public Property NotShouldNeverReturnNull(IPattern<string, string> pattern) =>
        (Pattern.Not(pattern) != null).ToProperty();

    [Property(DisplayName = "Not with description should never return null")]
    public Property NotWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        NonNull<string> description) =>
        (Pattern.Not(pattern, description.Get) != null).ToProperty();

    [Property(DisplayName = "Not should be opposite to pattern")]
    public Property NotShouldBeOppositeToPattern(IPattern<string, string> pattern, string x) =>
        (pattern.Match(x).IsSuccessful == !Pattern.Not(pattern).Match(x).IsSuccessful).ToProperty();

    [Property(DisplayName = "Not should have correct description")]
    public Property NotShouldHaveCorrectDescription(IPattern<string, string> pattern) =>
        (Pattern.Not(pattern).Description ==
            String.Format(Pattern.DefaultNotDescriptionFormat, pattern.Description))
            .ToProperty();

    [Property(DisplayName = "Not should have specified description")]
    public Property NotShouldHaveSpecifiedDescription(IPattern<string, string> pattern, NonNull<string> description) =>
        (Pattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Not should have empty description if pattern has empty description")]
    public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, bool> predicate) =>
        (Pattern.Not(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

    [Fact(DisplayName = "Not should throw if pattern is null")]
    public void NotShouldThrowIfPatternIsNull() =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Not<object, object>(null));

    [Property(DisplayName = "Not should throw if description is null")]
    public void NotShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern) =>
        Assert.Throws<ArgumentNullException>(() => Pattern.Not(pattern, null));
}
