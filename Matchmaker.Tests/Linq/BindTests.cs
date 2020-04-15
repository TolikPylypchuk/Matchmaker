using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class BindTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindPatternShouldMatchSameAsBinderResult(
            IPattern<string, string> pattern,
            Func<string, IPattern<string, string>> binder,
            string input)
        {
            var result = pattern.Match(input);
            return result.IsSuccessful.ImpliesThat(() =>
                    pattern.Bind(binder).Match(input) == binder(result.Value).Match(input))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindPatternWithDescriptionShouldMatchSameAsBinderResult(
            IPattern<string, string> pattern,
            Func<string, IPattern<string, string>> binder,
            string input,
            NonNull<string> description)
        {
            var result = pattern.Match(input);
            return result.IsSuccessful.ImpliesThat(() =>
                pattern.Bind(binder, description.Get).Match(input) == binder(result.Value).Match(input))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindPatternShouldHaveSameDescriptionAsPattern(
            IPattern<string, string> pattern,
            Func<string, IPattern<string, string>> binder)
            => (pattern.Bind(binder).Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property BindPatternWithDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern,
            Func<string, IPattern<string, string>> binder,
            NonNull<string> description)
            => (pattern.Bind(binder, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindPatternShouldThrowIfPatternIsNull(Func<string, IPattern<string, string>> binder)
        {
            Action action = () => ((IPattern<string, string>)null).Bind(binder);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindPatternWithDescriptionShouldThrowIfPatternIsNull(
            Func<string, IPattern<string, string>> binder,
            NonNull<string> description)
        {
            Action action = () => ((IPattern<string, string>)null).Bind(binder, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindPatternShouldThrowIfBinderIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Pipe((IPattern<string, int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void BindPatternWithDescriptionShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern,
            Func<string, IPattern<string, string>> binder)
        {
            Action action = () => pattern.Bind(binder, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
