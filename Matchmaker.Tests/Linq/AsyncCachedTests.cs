namespace Matchmaker.Linq;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns.Async;

using Xunit;

public class AsyncCachedTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternShouldNeverReturnNull(IAsyncPattern<string, string> pattern) =>
        (pattern.Cached() != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternWithDescriptionShouldNeverReturnNull(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternShouldMatchSameAsPattern(IAsyncPattern<string, string> pattern, string x) =>
        (pattern.Cached().MatchAsync(x).Result == pattern.MatchAsync(x).Result).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternWithDescriptionShouldMatchSameAsPattern(
        IAsyncPattern<string, string> pattern,
        string x,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).MatchAsync(x).Result == pattern.MatchAsync(x).Result).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternShouldBeCached(string x)
    {
        int count = 0;

        var pattern = AsyncPattern.CreatePattern<string>(v =>
        {
            count++;
            return Task.FromResult(true);
        }).Cached();

        pattern.MatchAsync(x);
        pattern.MatchAsync(x);

        return (count == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternWithDescriptionShouldBeCached(string x, NonNull<string> description)
    {
        int count = 0;

        var pattern = AsyncPattern.CreatePattern<string>(v =>
        {
            count++;
            return Task.FromResult(true);
        }).Cached(description.Get);

        pattern.MatchAsync(x);
        pattern.MatchAsync(x);

        return (count == 1).ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternShouldHaveSameDescriptionAsPattern(IAsyncPattern<string, string> pattern) =>
        (pattern.Cached().Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property CachedPatternWithDescriptionShouldHaveSpecifiedDescription(
        IAsyncPattern<string, string> pattern,
        NonNull<string> description) =>
        (pattern.Cached(description.Get).Description == description.Get).ToProperty();

    [Fact]
    public void CachedPatternShouldThrowIfPatternIsNull()
    {
        var action = () => ((IAsyncPattern<string, string>)null).Cached();
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void CachedPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
    {
        var action = () => ((IAsyncPattern<string, string>)null).Cached(description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void CachedPatternWithDescriptionShouldThrowIfDescriptionIsNull(IAsyncPattern<string, string> pattern)
    {
        var action = () => pattern.Cached(null);
        action.Should().Throw<ArgumentNullException>();
    }
}
