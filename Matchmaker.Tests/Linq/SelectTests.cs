using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class SelectTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternShouldMatchSameAsPattern(
            IPattern<string, string> pattern,
            Func<string, int> mapper,
            string input)
            => (pattern.Select(mapper).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternShouldHaveMappedResultWhenSuccessful(
            IPattern<string, string> pattern,
            Func<string, int> mapper,
            string input)
        {
            return pattern.Match(input).IsSuccessful.ImpliesThat(
                    pattern.Select(mapper).Match(input).Value == mapper(pattern.Match(input).Value))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternWithDescriptionShouldMatchSameAsPattern(
            IPattern<string, string> pattern,
            Func<string, int> mapper,
            string input,
            string description)
        {
            Func<bool> selectPatternMatchesSameAsPattern = () =>
                pattern.Select(mapper, description).Match(input).IsSuccessful == pattern.Match(input).IsSuccessful;
            return selectPatternMatchesSameAsPattern.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternWithDescriptionShouldHaveMappedResultWhenSuccessful(
            IPattern<string, string> pattern,
            Func<string, int> mapper,
            string input,
            string description)
        {
            Func<bool> selectPatternHasMappedResult = () =>
                pattern.Match(input).IsSuccessful.ImpliesThat(
                    pattern.Select(mapper, description).Match(input).Value == mapper(pattern.Match(input).Value));

            return selectPatternHasMappedResult.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternShouldHaveSameDescriptionAsPattern(
            IPattern<string, string> pattern,
            Func<string, bool> mapper)
            => (pattern.Select(mapper).Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property SelectPatternWithDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern,
            Func<string, int> mapper,
            string description)
        {
            Func<bool> selectPatternWithDescriptionHasCorrectDescription = () =>
                pattern.Select(mapper, description).Description == description;
            return selectPatternWithDescriptionHasCorrectDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectPatternShouldThrowIfPatternIsNull(Func<string, int> mapper)
        {
            Action action = () => ((IPattern<string, string>)null).Select(mapper);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectPatternWithDescriptionShouldThrowIfPatternIsNull(
            Func<string, int> mapper,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Select(mapper, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectPatternShouldThrowIfMapperIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Select<string, string, int>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectPatternWithDescriptionShouldThrowIfMapperIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Select<string, string, int>(null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void SelectPatternWithDescriptionShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern,
            Func<string, int> mapper)
        {
            Action action = () => pattern.Select(mapper, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
