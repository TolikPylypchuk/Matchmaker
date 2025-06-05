namespace Matchmaker.Linq;

public class SelectTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper) =>
        (pattern.Select(mapper) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        (pattern.Select(mapper).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldHaveMappedResultWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        pattern.Match(input).IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper).Match(input).Value == mapper(pattern.Match(input).Value))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldMatchSameAsPattern(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful)
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        pattern.Match(input).IsSuccessful.ImpliesThat(() =>
                pattern.Select(mapper, description.Get).Match(input).Value == mapper(pattern.Match(input).Value))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, bool> mapper) =>
        (pattern.Select(mapper).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper)
    {
        var action = () => ((IPattern<string, string>)null).Select(mapper);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, int> mapper,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Select(mapper, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternShouldThrowIfMapperIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Select<string, string, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Select<string, string, int>(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, int> mapper)
    {
        var action = () => pattern.Select(mapper, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
