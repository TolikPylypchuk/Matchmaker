namespace Matchmaker.Patterns.Async;

public class AsyncPatternTests
{
    [Property]
    public Property SimplePatternShouldNeverReturnNull(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate) != null).ToProperty();

    [Property]
    public Property PatternShouldNeverReturnNull(Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher) != null).ToProperty();

    [Property]
    public Property SimplePatternWithDescriptionShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property]
    public Property PatternWithDescriptionShouldNeverReturnNull(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> SimplePatternShouldMatchSameAsPredicate(
        Func<string, Task<bool>> predicate,
        string x) =>
        ((await AsyncPattern.CreatePattern(predicate).MatchAsync(x)).IsSuccessful == await predicate(x))
            .ToProperty();

    [Property]
    public async Task<Property> PatternShouldMatchSameAsMatcher(
        Func<string, Task<MatchResult<string>>> matcher,
        string x) =>
        (await AsyncPattern.CreatePattern(matcher).MatchAsync(x) == await matcher(x)).ToProperty();

    [Property]
    public Property SimplePatternShouldHaveCorrectDescription(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(predicate, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property SimplePatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate).Description.Length == 0).ToProperty();

    [Property]
    public Property PatternShouldHaveCorrectDescription(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(matcher, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property PatternShouldHaveEmptyDescriptionByDefault(Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher).Description.Length == 0).ToProperty();

    [Property]
    public Property PatternToStringShouldReturnDescription(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (description.Get.Length > 0).ImpliesThat(() =>
                AsyncPattern.CreatePattern(matcher, description.Get).ToString() == description.Get)
            .ToProperty();

    [Property]
    public Property PatternToStringShouldReturnTypeWhenDescriptionIsEmpty(
        Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher).ToString() ==
            AsyncPattern.CreatePattern(matcher).GetType().ToString())
            .ToProperty();

    [Property]
    public Property SimplePatternCreateShouldNeverReturnNull(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.CreatePattern(predicate) != null).ToProperty();

    [Property]
    public Property SimplePatternCreateWithDescriptionShouldNeverReturnNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(predicate, description.Get) != null).ToProperty();

    [Property]
    public Property PatternCreateShouldNeverReturnNull(Func<string, Task<MatchResult<string>>> matcher) =>
        (AsyncPattern.CreatePattern(matcher) != null).ToProperty();

    [Property]
    public Property PatternCreateWithDescriptionShouldNeverReturnNull(
        Func<string, Task<MatchResult<string>>> matcher,
        NonNull<string> description) =>
        (AsyncPattern.CreatePattern(matcher, description.Get) != null).ToProperty();

    [Fact]
    public void SimplePatternCreateShouldThrowIfPredicateIsNull()
    {
        var createWithNull = () => AsyncPattern.CreatePattern<string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SimplePatternCreateShouldThrowIfDescriptionIsNull(Func<string, Task<bool>> predicate)
    {
        var createWithNull = () => AsyncPattern.CreatePattern(predicate, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void PatternCreateShouldThrowIfMatcherIsNull()
    {
        var createWithNull = () => AsyncPattern.CreatePattern<string, string>(null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void PatternCreateShouldThrowIfDescriptionIsNull(Func<string, Task<MatchResult<string>>> matcher)
    {
        var createWithNull = () => AsyncPattern.CreatePattern(matcher, null);
        createWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AnyShouldNeverReturnNull() =>
        AsyncPattern.Any<string>().Should().NotBeNull();

    [Property]
    public Property AnyWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Any<string>(description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> AnyShouldAlwaysSucceed(string x) =>
        (await AsyncPattern.Any<string>().MatchAsync(x)).IsSuccessful.ToProperty();

    [Property]
    public async Task<Property> AnyWithDescriptionShouldAlwaysSucceed(string x, NonNull<string> description) =>
        (await AsyncPattern.Any<string>(description.Get).MatchAsync(x)).IsSuccessful.ToProperty();

    [Fact]
    public void AnyShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Any<string>().Description.Should().Be(AsyncPattern.DefaultAnyDescription);

    [Property]
    public Property AnyWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.Any<string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void AnyShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Any<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property ReturnShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.Return<string, string>(x) != null).ToProperty();

    [Property]
    public Property ReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(x, description.Get) != null).ToProperty();

    [Property]
    public Property LazyReturnShouldNeverReturnNull(Task<string> x) =>
        (AsyncPattern.Return<string, string>(() => x) != null).ToProperty();

    [Property]
    public Property LazyReturnWithDescriptionShouldNeverReturnNull(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(() => x, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> ReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
    {
        var result = await AsyncPattern.Return<string, string>(y).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property]
    public async Task<Property> ReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        Task<string> y,
        NonNull<string> description)
    {
        var result = await AsyncPattern.Return<string, string>(y, description.Get).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property]
    public Property ReturnShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.Return<string, string>(x).Description == AsyncPattern.DefaultReturnDescription)
            .ToProperty();

    [Property]
    public Property ReturnWithDescriptionShouldHaveSpecifiedDescription(Task<string> x, NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(x, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void ReturnShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.Return<string, string>(x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public async Task<Property> LazyReturnShouldAlwaysReturnSpecifiedValue(string x, Task<string> y)
    {
        var result = await AsyncPattern.Return<string, string>(() => y).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property]
    public async Task<Property> LazyReturnWithDescriptionShouldAlwaysReturnSpecifiedValue(
        string x,
        Task<string> y,
        NonNull<string> description)
    {
        var result = await AsyncPattern.Return<string, string>(() => y, description.Get).MatchAsync(x);
        return (result.IsSuccessful && result.Value == await y).ToProperty();
    }

    [Property]
    public Property LazyReturnShouldHaveCorrectDefaultDescription(Task<string> x) =>
        (AsyncPattern.Return<string, string>(() => x).Description == AsyncPattern.DefaultReturnDescription)
            .ToProperty();

    [Property]
    public Property LazyReturnWithDescriptionShouldHaveSpecifiedDescription(
        Task<string> x,
        NonNull<string> description) =>
        (AsyncPattern.Return<string, string>(() => x, description.Get).Description == description.Get)
            .ToProperty();

    [Fact]
    public void LazyReturnShouldBeLazy()
    {
        var action = () => AsyncPattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"));
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
    public void LazyReturnWithDescriptionShouldBeLazy(NonNull<string> description)
    {
        var action = () => AsyncPattern.Return<string, string>(
            () => throw new AssertionFailedException("Lazy Return is not lazy"), description.Get);
        action.Should().NotThrow<AssertionFailedException>();
    }

    [Property]
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

    [Property]
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

    [Property]
    public void LazyReturnShouldThrowIfDescriptionIsNull(Task<string> x)
    {
        var action = () => AsyncPattern.Return<string, string>(() => x, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NullShouldNeverReturnNull() =>
        AsyncPattern.Null<string>().Should().NotBeNull();

    [Property]
    public Property NullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Null<string>(description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> NullShouldSucceedOnlyOnNull(string x) =>
        (x is null == (await AsyncPattern.Null<string>().MatchAsync(x)).IsSuccessful).ToProperty();

    [Property]
    public async Task<Property> NullWithDescriptionShouldSucceedOnlyOnNull(string x, NonNull<string> description) =>
        (x is null == (await AsyncPattern.Null<string>(description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Fact]
    public void NullShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Null<string>().Description.Should().Be(AsyncPattern.DefaultNullDescription);

    [Property]
    public Property NullShouldHaveSpecifiedDewcription(NonNull<string> description) =>
        (AsyncPattern.Null<string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void NullShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Null<string>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValueNullShouldNeverReturnNull() =>
        AsyncPattern.ValueNull<int>().Should().NotBeNull();

    [Property]
    public Property ValueNullWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.ValueNull<int>(description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> ValueNullShouldSucceedOnlyOnNull(int? x) =>
        (x is null == (await AsyncPattern.ValueNull<int>().MatchAsync(x)).IsSuccessful).ToProperty();

    [Property]
    public async Task<Property> ValueNullWithDescriptionShouldSucceedOnlyOnNull(int? x, NonNull<string> description) =>
        (x is null == (await AsyncPattern.ValueNull<int>(description.Get).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Fact]
    public void ValueNullShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.ValueNull<int>().Description.Should().Be(AsyncPattern.DefaultNullDescription);

    [Property]
    public Property ValueNullShouldHaveSpecifiedDewcription(NonNull<string> description) =>
        (AsyncPattern.ValueNull<int>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void ValueNullShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.ValueNull<int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TypeShouldNeverReturnNull() =>
        AsyncPattern.Type<object, string>().Should().NotBeNull();

    [Property]
    public Property TypeWithDescriptionShouldNeverReturnNull(NonNull<string> description) =>
        (AsyncPattern.Type<object, string>(description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> TypeShouldSucceedOnlyWhenTheValueHasType(int value) =>
        ((await AsyncPattern.Type<object, int>().MatchAsync(value)).IsSuccessful &&
            !(await AsyncPattern.Type<object, string>().MatchAsync(value)).IsSuccessful)
            .ToProperty();

    [Fact]
    public async Task TypeShouldSucceedOnNull() =>
        (await AsyncPattern.Type<object, string>().MatchAsync(null)).IsSuccessful.Should().BeTrue();

    [Fact]
    public async Task TypeShouldSucceedOnNullableValueNull() =>
        (await AsyncPattern.Type<object, int?>().MatchAsync(null)).IsSuccessful.Should().BeTrue();

    [Fact]
    public async Task TypeShouldFailOnValueNull() =>
        (await AsyncPattern.Type<object, int>().MatchAsync(null)).IsSuccessful.Should().BeFalse();

    [Property]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnlyWhenTheValueHasType(
        int value, NonNull<string> description) =>
        ((await AsyncPattern.Type<object, int>(description.Get).MatchAsync(value)).IsSuccessful &&
            !(await AsyncPattern.Type<object, string>(description.Get).MatchAsync(value)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnNull(NonNull<string> description) =>
        (await AsyncPattern.Type<object, string>(description.Get).MatchAsync(null)).IsSuccessful.ToProperty();

    [Property]
    public async Task<Property> TypeWithDescriptionShouldSucceedOnNullableValueNull(NonNull<string> description) =>
        (await AsyncPattern.Type<object, int?>(description.Get).MatchAsync(null)).IsSuccessful.ToProperty();

    [Property]
    public async Task<Property> TypeWithDescriptionShouldFailOnValueNull(NonNull<string> description) =>
        (!(await AsyncPattern.Type<object, int>(description.Get).MatchAsync(null)).IsSuccessful).ToProperty();

    [Fact]
    public void TypeShouldHaveCorrectDefaultDescription() =>
        AsyncPattern.Type<object, int>().Description.Should().Be(
            String.Format(AsyncPattern.DefaultTypeDescriptionFormat, typeof(int)));

    [Property]
    public Property TypeWithDescriptionShouldHaveSpecifiedDescription(NonNull<string> description) =>
        (AsyncPattern.Type<object, string>(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void TypeShouldThrowIfDescriptionIsNull()
    {
        var action = () => AsyncPattern.Type<object, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public Property NotShouldNeverReturnNull(IAsyncPattern<string, string> pattern) =>
        (AsyncPattern.Not(pattern) != null).ToProperty();

    [Property]
    public Property NotWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (AsyncPattern.Not(pattern, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> NotShouldBeOppositeToPattern(IAsyncPattern<string, string> pattern, string x) =>
        ((await pattern.MatchAsync(x)).IsSuccessful == !(await AsyncPattern.Not(pattern).MatchAsync(x)).IsSuccessful)
            .ToProperty();

    [Property]
    public Property NotShouldHaveCorrectDescription(IAsyncPattern<string, string> pattern) =>
        (AsyncPattern.Not(pattern).Description ==
            String.Format(AsyncPattern.DefaultNotDescriptionFormat, pattern.Description))
            .ToProperty();

    [Property]
    public Property NotShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (AsyncPattern.Not(pattern, description.Get).Description == description.Get).ToProperty();

    [Property]
    public Property NotShouldHaveEmptyDescriptionIfPatternHasEmptyDescription(Func<string, Task<bool>> predicate) =>
        (AsyncPattern.Not(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
            .ToProperty();

    [Fact]
    public void NotShouldThrowIfPatternIsNull()
    {
        var action = () => AsyncPattern.Not<object, object>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void NotShouldThrowIfDescriptionIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => AsyncPattern.Not(pattern, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
