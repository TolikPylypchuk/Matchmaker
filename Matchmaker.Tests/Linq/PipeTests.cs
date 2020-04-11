using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

using Xunit;

namespace Matchmaker.Linq
{
    public class PipeTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldMatchSameAsPatterns(
            IPattern<string, string> firstPattern,
            IPattern<string, string> secondPattern,
            string input)
        {
            var result = firstPattern.Match(input);
            return result.IsSuccessful.ImpliesThat(
                    firstPattern.Pipe(secondPattern).Match(input) == secondPattern.Match(result.Value))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithDescriptionShouldMatchSameAsPattern(
            IPattern<string, string> firstPattern,
            IPattern<string, string> secondPattern,
            string input,
            string description)
        {
            Func<bool> pipePatternMatchesSameAsPattern = () =>
            {
                var result = firstPattern.Match(input);
                return result.IsSuccessful.ImpliesThat(
                    firstPattern.Pipe(secondPattern, description).Match(input) == secondPattern.Match(result.Value));
            };

            return pipePatternMatchesSameAsPattern.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveCorectDescription(
            IPattern<string, string> firstPattern,
            IPattern<string, string> secondPattern)
            => (firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0).ImpliesThat(
                    firstPattern.Pipe(secondPattern).Description ==
                    String.Format(
                        Pattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Pipe(pattern).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Pipe(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).Pipe(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, string> firstPattern,
            IPattern<string, string> secondPattern,
            string description)
        {
            Func<bool> pipePatternHasDpecifiedDescription = () =>
                firstPattern.Pipe(secondPattern, description).Description == description;
            return pipePatternHasDpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternShouldThrowIfFirstPatternIsNull(IPattern<string, string> secondPattern)
        {
            Action action = () => ((IPattern<string, string>)null).Pipe(secondPattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfFirstPatternIsNull(
            IPattern<string, string> secondPattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Pipe(secondPattern, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternShouldThrowIfSecondPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Pipe((IPattern<string, int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfSecondPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Pipe((IPattern<string, int>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfDescriptionIsNull(
            IPattern<string, string> firstPattern,
            IPattern<string, string> secondPattern)
        {
            Action action = () => firstPattern.Pipe(secondPattern, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionShouldMatchSameAsPatterns(
            IPattern<string, string> pattern,
            Func<string, MatchResult<string>> matcher,
            string input)
        {
            var result = pattern.Match(input);
            return result.IsSuccessful.ImpliesThat(pattern.Pipe(matcher).Match(input) == matcher(result.Value))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionAndDescriptionShouldMatchSameAsPattern(
            IPattern<string, string> pattern,
            Func<string, MatchResult<string>> matcher,
            string input,
            string description)
        {
            Func<bool> pipePatternMatchesSameAsPattern = () =>
            {
                var result = pattern.Match(input);
                return result.IsSuccessful.ImpliesThat(
                    pattern.Pipe(matcher, description).Match(input) == matcher(result.Value));
            };

            return pipePatternMatchesSameAsPattern.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionShouldHaveSameDescriptionAsPattern(
            IPattern<string, string> pattern,
            Func<string, MatchResult<string>> matcher)
            => (pattern.Pipe(matcher).Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionAndDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern,
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            Func<bool> pipePatternHasDpecifiedDescription = () =>
                pattern.Pipe(matcher, description).Description == description;
            return pipePatternHasDpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionShouldThrowIfPatternIsNull(Func<string, MatchResult<string>> matcher)
        {
            Action action = () => ((IPattern<string, string>)null).Pipe(matcher);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfPatternIsNull(
            Func<string, MatchResult<string>> matcher,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Pipe(matcher, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionShouldThrowIfMatcherIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Pipe((Func<string, MatchResult<int>>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfMatcherIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Pipe((Func<string, MatchResult<int>>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern,
            Func<string, MatchResult<string>> matcher)
        {
            Action action = () => pattern.Pipe(matcher, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldMatchSameAsPatterns(
            IPattern<string, object> pattern,
            string input)
        {
            var result = pattern.Match(input);
            return result.IsSuccessful.ImpliesThat(
                    pattern.Cast<string, object, string>().Match(input) ==
                    Pattern.Type<object, string>().Match(result.Value))
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternWithDescriptionShouldMatchSameAsPattern(
            IPattern<string, object> pattern,
            string input,
            string description)
        {
            Func<bool> castPatternMatchesSameAsPattern = () =>
            {
                var result = pattern.Match(input);
                return result.IsSuccessful.ImpliesThat(
                    pattern.Cast<string, object, string>(description).Match(input) ==
                    Pattern.Type<object, string>().Match(result.Value));
            };

            return castPatternMatchesSameAsPattern.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldHaveCorectDescription(IPattern<string, object> pattern)
            => (pattern.Description.Length > 0).ImpliesThat(
                    pattern.Cast<string, object, string>().Description ==
                    String.Format(
                        Pattern.DefaultPipeDescriptionFormat,
                        pattern.Description,
                        String.Format(Pattern.DefaultTypeDescriptionFormat, typeof(string))))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty)
                    .Select(value => (object)value)
                    .Cast<string, object, string>()
                    .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternWithDescriptionShouldHaveSpecifiedDescription(
            IPattern<string, object> pattern,
            string description)
        {
            Func<bool> castPatternHasDpecifiedDescription = () =>
                pattern.Cast<string, object, string>(description).Description == description;
            return castPatternHasDpecifiedDescription.When(description != null);
        }

        [Fact]
        public void CastPatternShouldThrowIfPatternIsNull()
        {
            Action action = () => ((IPattern<string, object>)null).Cast<string, object, string>();
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CastPatternWithDescriptionShouldThrowIfPatternIsNull(string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, object>)null).Cast<string, object, string>(description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CastPatternWithDescriptionShouldThrowIfDescriptionIsNull(IPattern<string, object> pattern)
        {
            Action action = () => pattern.Cast<string, object, string>(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
