using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class WhereTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternShouldMatchSameAsPatternAndPredicate(
               IPattern<string, string> pattern,
               Func<string, bool> predicate,
               string input)
               => (pattern.Where(predicate).Match(input).IsSuccessful ==
                       (pattern.Match(input).IsSuccessful && predicate(input)))
                   .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternShouldHaveSameResultAsPatternWhenSuccessful(
            IPattern<string, string> pattern,
            Func<string, bool> predicate,
            string input)
        {
            var wherePattern = pattern.Where(predicate);
            return wherePattern.Match(input).IsSuccessful.ImpliesThat(wherePattern.Match(input) == pattern.Match(input))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternWithDescriptionShouldMatchSameAsPatternAndPredicate(
            IPattern<string, string> pattern,
            Func<string, bool> predicate,
            string input,
            string description)
        {
            Func<bool> wherePatternMatchesSameAsPatternAndPredicate = () =>
                pattern.Where(predicate, description).Match(input).IsSuccessful ==
                (pattern.Match(input).IsSuccessful && predicate(input));
            return wherePatternMatchesSameAsPatternAndPredicate.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternWithDescriptionShouldHaveSameResultAsPatternWhenSuccessful(
            IPattern<string, string> pattern,
            Func<string, bool> predicate,
            string input,
            string description)
        {
            Func<bool> wherePatternHasSameResult = () =>
            {
                var wherePattern = pattern.Where(predicate, description);
                return wherePattern.Match(input).IsSuccessful.ImpliesThat(
                    wherePattern.Match(input) == pattern.Match(input));
            };

            return wherePatternHasSameResult.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternShouldHaveSameDescriptionAsPattern(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Where(predicate).Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property WherePatternWithDescriptionShouldHaveCorrectDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate,
            string description)
        {
            Func<bool> wherePatternWithDescriptionHasCorrectDescription = () =>
                pattern.Where(predicate, description).Description == description;
            return wherePatternWithDescriptionHasCorrectDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WherePatternShouldThrowIfPatternIsNull(Func<string, bool> predicate)
        {
            Action action = () => ((IPattern<string, string>)null).Where(predicate);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WherePatternWithDescriptionShouldThrowIfPatternIsNull(
            Func<string, bool> predicate,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Where(predicate, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WherePatternShouldThrowIfPredicateIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Where(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WherePatternWithDescriptionShouldThrowIfPredicateIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Where(null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void WherePatternWithDescriptionShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
        {
            Action action = () => pattern.Where(predicate, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
