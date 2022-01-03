namespace Matchmaker.Linq;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns.Async;

public class AsyncWhereTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input) =>
        (pattern.Where(predicate).MatchAsync(input).Result.IsSuccessful ==
                   (pattern.MatchAsync(input).Result.IsSuccessful && predicate(input)))
               .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input)
    {
        var wherePattern = pattern.Where(predicate);
        return wherePattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
                wherePattern.MatchAsync(input).Result == pattern.MatchAsync(input).Result)
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).MatchAsync(input).Result.IsSuccessful ==
            (pattern.MatchAsync(input).Result.IsSuccessful && predicate(input)))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        string input,
        NonNull<string> description)
    {
        var wherePattern = pattern.Where(predicate, description.Get);
        return wherePattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
            wherePattern.MatchAsync(input).Result == pattern.MatchAsync(input).Result).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property WherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternShouldThrowIfPatternIsNull(Func<string, bool> predicate)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, bool> predicate,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Where((Func<string, bool>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Where((Func<string, bool>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void WherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, bool> predicate)
    {
        var action = () => pattern.Where(predicate, null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input) =>
        (pattern.Where(predicate).MatchAsync(input).Result.IsSuccessful ==
                   (pattern.MatchAsync(input).Result.IsSuccessful && predicate(input).Result))
               .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input)
    {
        var wherePattern = pattern.Where(predicate);
        return wherePattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
                wherePattern.MatchAsync(input).Result == pattern.MatchAsync(input).Result)
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).MatchAsync(input).Result.IsSuccessful ==
            (pattern.MatchAsync(input).Result.IsSuccessful && predicate(input).Result))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        string input,
        NonNull<string> description)
    {
        var wherePattern = pattern.Where(predicate, description.Get);
        return wherePattern.MatchAsync(input).Result.IsSuccessful.ImpliesThat(() =>
            wherePattern.MatchAsync(input).Result == pattern.MatchAsync(input).Result).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternShouldHaveSameDescriptionAsPattern(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate) =>
        (pattern.Where(predicate).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property AsyncWherePatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate,
        NonNull<string> description) =>
        (pattern.Where(predicate, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsyncWherePatternShouldThrowIfPatternIsNull(Func<string, Task<bool>> predicate)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, Task<bool>> predicate,
        NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Where(predicate, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsyncWherePatternShouldThrowIfPredicateIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Where((Func<string, Task<bool>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsyncWherePatternWithDescriptionShouldThrowIfPredicateIsNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description)
    {
        var action = () => pattern.Where((Func<string, Task<bool>>)null, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void AsyncWherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IAsyncPattern<string, string> pattern,
        Func<string, Task<bool>> predicate)
    {
        var action = () => pattern.Where(predicate, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
