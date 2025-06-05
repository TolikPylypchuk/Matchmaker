namespace Matchmaker.Linq;

public class WhereTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldMatchSameAsPatternAndPredicate(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input) =>
        (pattern.Where(predicate).Match(input).IsSuccessful ==
                   (pattern.Match(input).IsSuccessful && predicate(input)))
               .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldHaveSameResultAsPatternWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input)
    {
        var wherePattern = pattern.Where(predicate);
        return wherePattern.Match(input).IsSuccessful.ImpliesThat(() =>
                wherePattern.Match(input) == pattern.Match(input))
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Match(input).IsSuccessful ==
            (pattern.Match(input).IsSuccessful && predicate(input)))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description)
    {
        var wherePattern = pattern.Where(predicate, description.Get);
        return wherePattern.Match(input).IsSuccessful.ImpliesThat(() =>
            wherePattern.Match(input) == pattern.Match(input)).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternShouldThrowIfPatternIsNull(Func<string, bool> predicate)
    {
        var action = () => ((IPattern<string, string>)null).Where(predicate);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, bool> predicate,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Where(predicate, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternShouldThrowIfPredicateIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Where(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Where(null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, bool> predicate)
    {
        var action = () => pattern.Where(predicate, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
