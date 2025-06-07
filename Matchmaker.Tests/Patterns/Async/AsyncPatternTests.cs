namespace Matchmaker.Patterns.Async;

public class AsyncPatternTests
{
    [Property(DisplayName = "Simple pattern should match the same as predicate")]
    public async Task<Property> SimplePatternShouldMatchSameAsPredicate(
        Func<string, Task<bool>> predicate,
        string x) =>
        ((await AsyncPattern.CreatePattern(predicate).MatchAsync(x)).IsSuccessful == await predicate(x))
            .ToProperty();

    [Property(DisplayName = "Pattern should match the same as matcher")]
    public async Task<Property> PatternShouldMatchSameAsMatcher(
        Func<string, Task<MatchResult<string>>> matcher,
        string x) =>
        (await AsyncPattern.CreatePattern(matcher).MatchAsync(x) == await matcher(x)).ToProperty();

    [Property(DisplayName = "Simple pattern should have correct description")]
    public Property SimplePatternShouldHaveCorrectDescription(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Simple pattern should have empty description by default")]
    public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pattern should have correct description")]
    public Property PatternShouldHaveCorrectDescription(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Pattern should have empty description by default")]
    public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

    [Property(DisplayName = "Pattern.ToString() should return description")]
    public Property PatternToStringShouldReturnDescription(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (description.Get.Length > 0).ImpliesThat(() =>
                AsyncPattern.CreatePattern(matcher, description.Get).ToString() == description.Get)
            .ToProperty();

    [Property(DisplayName = "Pattern.ToString() should return type when description is empty")]
    public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(
        Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher).ToString() ==
            AsyncPattern.CreatePattern(matcher).GetType().ToString())
            .ToProperty();

    [Property(DisplayName = "Simple CreatePattern should never return null")]
    public Property SimpleCreatePatternShouldNeverReturnNull(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate) != null).ToProperty();

    [Property(DisplayName = "Simple CreatePattern with description should never return null")]
    public Property SimpleCreatePatternWithDescriptionShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property(DisplayName = "CreatePattern should never return null")]
    public Property CreatePatternShouldNeverReturnNull(Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher) != null).ToProperty();

    [Property(DisplayName = "CreatePattern with description should never return null")]
    public Property CreatePatternWithDescriptionShouldNeverReturnNull(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Fact(DisplayName = "Simple CreatePattern should throw if predicate is null")]
    public void SimpleCreatePatternShouldThrowIfPredicateIsNull()
    {
        var createWithNull = () => AsyncPattern.CreatePattern<string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Simple CreatePattern should throw if description is null")]
    public void SimpleCreatePatternShouldThrowIfDescriptionIsNull(Func<string, Task<bool>> predicate)
    {
        var createWithNull = () => AsyncPattern.CreatePattern(predicate, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "CreatePattern should throw if matcher is null")]
    public void CreatePatternShouldThrowIfMatcherIsNull()
    {
        var createWithNull = () => AsyncPattern.CreatePattern<string, string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "CreatePattern should throw if description is null")]
    public void CreatePatternShouldThrowIfDescriptionIsNull(Func<string, Task<MatchResult<string>>> matcher)
    {
        var createWithNull = () => AsyncPattern.CreatePattern(matcher, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Any should never return null")]
    public void AnyShouldNeverReturnNull() =>
        AsyncPattern.Any<string>().Should().NotBeNull();

    [Property(DisplayName = "Any with description should never return null")]
    public Property AnyWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Any<string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Any should always succeed")]
    public async Task<Property> AnyShouldAlwaysSucceed(string x) =>
        (await AsyncPattern.Any<string>().MatchAsync(x)).IsSuccessful.ToProperty();

    [Property(DisplayName = "Any with description should always succeed")]
    public async Task<Property> AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description) =>
        (await AsyncPattern.Any<string>(description.Get).MatchAsync(x)).IsSuccessful.ToProperty();

    [Fact(DisplayName = "Any should have correct default description")]
    public void AnyShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Any<string>().Description.Should().Be(AsyncPattern.DefaultAnyDescription);

    [Property(DisplayName = "Any with description should have the specified description")]
    public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.Any<string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Any should throw if description is null")]
    public void AnyShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Any<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Return should never return null")]
    public Property ReturnShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.Return<string, string>(x) != null).ToProperty();

    [Property(DisplayName = "Return with description should never return null")]
    public Property ReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Lazy Return should never return null")]
    public Property LazyReturnShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.Return<string, string>(() => x) != null).ToProperty();

    [Property(DisplayName = "Lazy Return with description should never return null")]
    public Property LazyReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(() => x, description.Get) != null).ToProperty();

    [Property(DisplayName = "Return should always return the specified value")]
    public async Task<Property> ReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
    {
        var result = await AsyncPattern.Return<string, string>(y).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property(DisplayName = "Return with description should always return specified value")]
    public async Task<Property> ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        Task<string> y,
        NonNull<string> description)
    {
        var result = await AsyncPattern.Return<string, string>(y, description.Get).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property(DisplayName = "Return should have correct default description")]
    public Property ReturnShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.Return<string, string>(x).Description == AsyncPattern.DefaultReturnDescription)
            .ToProperty();

    [Property(DisplayName = "Return with description should have the specified description")]
    public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Return should throw if description is null")]
    public void ReturnShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.Return<string, string>(x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Lazy Return should always return the pecified value")]
    public async Task<Property> LazyReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
    {
        var result = await AsyncPattern.Return<string, string>(() => y).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property(DisplayName = "Lazy Return with description should always return the specified value")]
    public async Task<Property> LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        Task<string> y,
        NonNull<string> description)
    {
        var result = await AsyncPattern.Return<string, string>(() => y, description.Get).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property(DisplayName = "Lazy Return should have correct default description")]
    public Property LazyReturnShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.Return<string, string>(() => x).Description == AsyncPattern.DefaultReturnDescription)
            .ToProperty();

    [Property(DisplayName = "Lazy Return with description should have the specified description")]
    public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(() => x, description.Get).Description == description.Get)
            .ToProperty();

    [Fact(DisplayName = "Lazy Return should be lazy")]
    public void LazyReturnShouldBeLazy()
    {
        var action = () => AsyncPattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy Return with description should be lazy")]
    public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"), description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property(DisplayName = "Lazy Return should be memoized")]
    public async Task<Property> LazyReturnShouldBeMemoized(string x)
    {
        int counter = 0;

        var pattern = AsyncPattern.Return<string, string>(() =>
        {
            counter++;
            return Task.FromResult(String.Empty);
        });

        await pattern.MatchAsync(x);
        await pattern.MatchAsync(x);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy Return with description should be memoized")]
    public async Task<Property> LazyReturnWithDescriptionShouldBeMemoized(string x, NonNull<string> description)
    {
        int counter = 0;

        var pattern = AsyncPattern.Return<string, string>(
            () =>
            {
                counter++;
                return Task.FromResult(String.Empty);
            },
            description.Get);

        await pattern.MatchAsync(x);
        await pattern.MatchAsync(x);

        return (counter == 1).ToProperty();
    }

    [Property(DisplayName = "Lazy Return should throw if description is null")]
    public void LazyReturnShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.Return<string, string>(() => x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Null should never return null")]
    public void NullShouldNeverReturnNull() =>
        AsyncPattern.Null<string>().Should().NotBeNull();

    [Property(DisplayName = "Null with description should never return null")]
    public Property NullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Null<string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Null should succeed only on null")]
    public async Task<Property> NullShouldSucceedOnlyOnNull(string x) =>
        (x is null == (await AsyncPattern.Null<string>().MatchAsync(x)).IsSuccessful).ToProperty();

    [Property(DisplayName = "Null with description should succeed only on null")]
    public async Task<Property> NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description) =>
        (x is null == (await AsyncPattern.Null<string>(description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "Null should have correct default description")]
    public void NullShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Null<string>().Description.Should().Be(AsyncPattern.DefaultNullDescription);

    [Property(DisplayName = "Null should have the specified description")]
    public Property NullShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.Null<string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Null should throw if description is null")]
    public void NullShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Null<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "ValueNull should never return null")]
    public void ValueNullShouldNeverReturnNull() =>
        AsyncPattern.ValueNull<int>().Should().NotBeNull();

    [Property(DisplayName = "ValueNull with description should never return null")]
    public Property ValueNullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.ValueNull<int>(description.Get) != null).ToProperty();

    [Property(DisplayName = "ValueNull should succeed only on null")]
    public async Task<Property> ValueNullShouldSucceedOnlyOnNull(int? x) =>
        (x is null == (await AsyncPattern.ValueNull<int>().MatchAsync(x)).IsSuccessful).ToProperty();

    [Property(DisplayName = "ValueNull with description should succeed only on null")]
    public async Task<Property> ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description) =>
        (x is null == (await AsyncPattern.ValueNull<int>(description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "ValueNull should have correct default description")]
    public void ValueNullShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.ValueNull<int>().Description.Should().Be(AsyncPattern.DefaultNullDescription);

    [Property(DisplayName = "ValueNull should have the specified description")]
    public Property ValueNullShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "ValueNull should throw if description is null")]
    public void ValueNullShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.ValueNull<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Type should never return null")]
    public void TypeShouldNeverReturnNull() =>
        AsyncPattern.Type<object, string>().Should().NotBeNull();

    [Property(DisplayName = "Type with description should never return null")]
    public Property TypeWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Type<object, string>(description.Get) != null).ToProperty();

    [Property(DisplayName = "Type should succeed only when the value has the specified type")]
    public async Task<Property> TypeShouldSucceedOnlyWhenValueHasSpecifiedType(int value) =>
        ((await AsyncPattern.Type<object, int>().MatchAsync(value)).IsSuccessful &&
            !(await AsyncPattern.Type<object, string>().MatchAsync(value)).IsSuccessful)
            .ToProperty();

    [Fact(DisplayName = "Type should succeed on null")]
    public async Task TypeShouldSucceedOnNull() =>
        (await AsyncPattern.Type<object, string>().MatchAsync(null)).IsSuccessful.Should().BeTrue();

    [Fact(DisplayName = "Type should succeed on nullable value null")]
    public async Task TypeShouldSucceedOnNullableValueNull() =>
        (await AsyncPattern.Type<object, int?>().MatchAsync(null)).IsSuccessful.Should().BeTrue();

    [Fact(DisplayName = "Type should fail on value null")]
    public async Task TypeShouldFailOnValueNull() =>
        (await AsyncPattern.Type<object, int>().MatchAsync(null)).IsSuccessful.Should().BeFalse();

    [Property(DisplayName = "Type with description should succeed only when the value has the specified type")]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnlyWhenValueHasSpecifiedType(
        int value, NonNull<string> description) =>
        ((await AsyncPattern.Type<object, int>(description.Get).MatchAsync(value)).IsSuccessful &&
            !(await AsyncPattern.Type<object, string>(description.Get).MatchAsync(value)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Type with description should succeed on null")]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnNull(NonNull<string> description) =>
        (await AsyncPattern.Type<object, string>(description.Get).MatchAsync(null)).IsSuccessful.ToProperty();

    [Property(DisplayName = "Type with description should succeed on nullable value null")]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnNullableValueNull(NonNull<string> description) =>
        (await AsyncPattern.Type<object, int?>(description.Get).MatchAsync(null)).IsSuccessful.ToProperty();

    [Property(DisplayName = "Type with description should fail on value null")]
    public async Task<Property> TypeWithDescriptionShouldFailOnValueNull(NonNull<string> description) =>
        (!(await AsyncPattern.Type<object, int>(description.Get).MatchAsync(null)).IsSuccessful).ToProperty();

    [Fact(DisplayName = "Type should have correct default description")]
    public void TypeShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Type<object, int>().Description.Should().Be(
            String.Format(AsyncPattern.DefaultTypeDescriptionFormat, typeof(int)));

    [Property(DisplayName = "Type with description should have the specified description")]
    public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact(DisplayName = "Type should throw if description is null")]
    public void TypeShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Type<object, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Not should never return null")]
    public Property NotShouldNeverReturnNull(IAsyncPattern<string, string> pattern) =>
        (AsyncPattern.Not(pattern) != null).ToProperty();

    [Property(DisplayName = "Not with description should never return null")]
    public Property NotWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (AsyncPattern.Not(pattern, description.Get) != null).ToProperty();

    [Property(DisplayName = "Not should be opposite to pattern")]
    public async Task<Property> NotShouldBeOppositeToPattern(IAsyncPattern<string, string> pattern, string x) =>
        ((await pattern.MatchAsync(x)).IsSuccessful == !(await AsyncPattern.Not(pattern).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property(DisplayName = "Not should have correct description")]
    public Property NotShouldHaveCorrectDescription(IAsyncPattern<string, string> pattern) =>
        (AsyncPattern.Not(pattern).Description ==
            String.Format(AsyncPattern.DefaultNotDescriptionFormat, pattern.Description))
            .ToProperty();

    [Property(DisplayName = "Not should have specified description")]
    public Property NotShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (AsyncPattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

    [Property(DisplayName = "Not should have empty description if pattern has empty description")]
    public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.Not(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
            .ToProperty();

    [Fact(DisplayName = "Not should throw if pattern is null")]
    public void NotShouldThrowIfPatternIsNull()
    {
        var action = () => AsyncPattern.Not<object, object>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(DisplayName = "Not should throw if description is null")]
    public void NotShouldThrowIfDescriptionIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => AsyncPattern.Not(pattern, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
