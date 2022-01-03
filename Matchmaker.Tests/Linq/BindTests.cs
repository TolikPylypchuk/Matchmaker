namespace Matchmaker.Linq;

using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

public class BindTests
{
    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder) =>
        (pattern.Bind(binder) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldNeverReturnNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get) != null).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldMatchSameAsBinderResult(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        string x)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
                pattern.Bind(binder).Match(x) == binder(result.Value).Match(x))
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldMatchSameAsBinderResult(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        string x,
        NonNull<string> description)
    {
        var result = pattern.Match(x);
        return result.IsSuccessful.ImpliesThat(() =>
            pattern.Bind(binder, description.Get).Match(x) == binder(result.Value).Match(x))
            .ToProperty();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternShouldHaveSameDescriptionAsPattern(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder) =>
        (pattern.Bind(binder).Description == pattern.Description).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description) =>
        (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternShouldThrowIfPatternIsNull(Func<string, IPattern<string, string>> binder)
    {
        var action = () => ((IPattern<string, string>)null).Bind(binder);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
        Func<string, IPattern<string, string>> binder,
        NonNull<string> description)
    {
        var action = () => ((IPattern<string, string>)null).Bind(binder, description.Get);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternShouldThrowIfBinderIsNull(IPattern<string, string> pattern)
    {
        var action = () => pattern.Bind((Func<string, IPattern<string, int>>)null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Property(Arbitrary = new[] { typeof(Generators) })]
    public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
        IPattern<string, string> pattern,
        Func<string, IPattern<string, string>> binder)
    {
        var action = () => pattern.Bind(binder, null);
        action.Should().Throw<ArgumentNullException>();
    }
}
