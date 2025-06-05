namespace Matchmaker.Linq;

public class AsyncSelectTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper) =>
        (pattern.Select(mapper) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        (pattern.Select(mapper).MatchAsync(input).Result.IsSuccessful ==
            pattern.MatchAsync(input).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        pattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper).MatchAsync(input).Result.Value ==
                mapper(pattern.MatchAsync(input).Result.Value))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).MatchAsync(input).Result.IsSuccessful ==
            pattern.MatchAsync(input).Result.IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        pattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper, description.Get).MatchAsync(input).Result.Value ==
                mapper(pattern.MatchAsync(input).Result.Value))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> mapper) =>
        (pattern.Select(mapper).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Select(mapper);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, int> mapper,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Select(mapper, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternShouldThrowIfMapperIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Select<string, string, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Select<string, string, int>(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper)
    {
        var action = () => pattern.Select(mapper, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
