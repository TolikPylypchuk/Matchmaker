using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

using Xunit;

namespace Matchmaker.Linq
{
    public class CachedTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternShouldMatchSameAsPattern(IPattern<string, string> pattern, string input)
               => (pattern.Cached().Match(input) ==pattern.Match(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternWithDescriptionShouldMatchSameAsPattern(
            IPattern<string, string> pattern,
            string input,
            NonNull<string> description)
            => (pattern.Cached(description.Get).Match(input) == pattern.Match(input)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternShouldBeCached(string input)
        {
            int count = 0;

            var pattern = Pattern.CreatePattern<string>(v =>
            {
                count++;
                return true;
            }).Cached();

            pattern.Match(input);
            pattern.Match(input);

            return (count == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternWithDescriptionShouldBeCached(string input, NonNull<string> description)
        {
            int count = 0;

            var pattern = Pattern.CreatePattern<string>(v =>
            {
                count++;
                return true;
            }).Cached(description.Get);

            pattern.Match(input);
            pattern.Match(input);

            return (count == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternShouldHaveSameDescriptionAsPattern(IPattern<string, string> pattern)
            => (pattern.Cached().Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CachedPatternWithDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern,
            NonNull<string> description)
            => (pattern.Cached(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void CachedPatternShouldThrowIfPatternIsNull()
        {
            Action action = () => ((IPattern<string, string>)null).Cached();
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CachedPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
        {
            Action action = () => ((IPattern<string, string>)null).Cached(description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CachedPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Cached(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
