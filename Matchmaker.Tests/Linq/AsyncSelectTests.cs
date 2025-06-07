namespace Matchmaker.Linq;

public class AsyncSelectTests
{
    [Property]
    public Property SelectPatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper) =>
        (pattern.Select(mapper) != null).ToProperty();

    [Property]
    public Property SelectPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get) != null).ToProperty();

    [Property]
    public async Task<Property> SelectPatternShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        ((await pattern.Select(mapper).MatchAsync(input)).IsSuccessful ==
            (await pattern.MatchAsync(input)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> SelectPatternShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input) =>
        (await (await pattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                (await pattern.Select(mapper).MatchAsync(input)).Value ==
                mapper((await pattern.MatchAsync(input)).Value)))
            .ToProperty();

    [Property]
    public async Task<Property> SelectPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        ((await pattern.Select(mapper, description.Get).MatchAsync(input)).IsSuccessful ==
            (await pattern.MatchAsync(input)).IsSuccessful)
            .ToProperty();

    [Property]
    public async Task<Property> SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        string input,
        NonNull<string> description) =>
        (await (await pattern.MatchAsync(input)).IsSuccessful.ImpliesThatAsync(async () =>
                (await pattern.Select(mapper, description.Get).MatchAsync(input)).Value ==
                mapper((await pattern.MatchAsync(input)).Value)))
            .ToProperty();

    [Property]
    public Property SelectPatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> mapper) =>
        (pattern.Select(mapper).Description == pattern.Description).ToProperty();

    [Property]
    public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper,
        NonNull<string> description) =>
        (pattern.Select(mapper, description.Get).Description == description.Get).ToProperty();

    [Property]
    public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Select(mapper);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, int> mapper,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Select(mapper, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SelectPatternShouldThrowIfMapperIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Select<string, string, int>(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Select<string, string, int>(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property]
    public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, int> mapper)
    {
        var action = () => pattern.Select(mapper, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
