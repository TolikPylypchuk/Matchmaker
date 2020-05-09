using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns.Async;

using Xunit;

namespace Matchmaker.Linq
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class AsyncPipeTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldNeverReturnNull(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern)
            => (firstPattern.Pipe(secondPattern) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern,
            NonNull<string> description)
            => (firstPattern.Pipe(secondPattern, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldMatchSameAsPatterns(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern,
            string x)
        {
            var result = firstPattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                    firstPattern.Pipe(secondPattern).MatchAsync(x).Result ==
                    secondPattern.MatchAsync(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithDescriptionShouldMatchSameAsPattern(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern,
            string x,
            NonNull<string> description)
        {
            var result = firstPattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                firstPattern.Pipe(secondPattern, description.Get).MatchAsync(x).Result ==
                secondPattern.MatchAsync(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveCorrectDescription(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern)
            => (firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0).ImpliesThat(() =>
                    firstPattern.Pipe(secondPattern).Description ==
                    String.Format(
                        AsyncPattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty).Pipe(pattern).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (pattern.Pipe(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, Task<bool>> predicate1,
            Func<string, Task<bool>> predicate2)
            => (AsyncPattern.CreatePattern(predicate1, String.Empty)
                .Pipe(AsyncPattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithDescriptionShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern,
            NonNull<string> description)
            => (firstPattern.Pipe(secondPattern, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternShouldThrowIfFirstPatternIsNull(IAsyncPattern<string, string> secondPattern)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Pipe(secondPattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfFirstPatternIsNull(
            IAsyncPattern<string, string> secondPattern,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Pipe(secondPattern, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternShouldThrowIfSecondPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Pipe((IAsyncPattern<string, int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfSecondPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Pipe((IAsyncPattern<string, int>)null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithDescriptionShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> firstPattern,
            IAsyncPattern<string, string> secondPattern)
        {
            Action action = () => firstPattern.Pipe(secondPattern, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher)
            => (pattern.Pipe(matcher) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionAndDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (pattern.Pipe(matcher, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionShouldMatchSameAsPatterns(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher,
            string x)
        {
            var result = pattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                    pattern.Pipe(matcher).MatchAsync(x).Result ==
                    matcher(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionAndDescriptionShouldMatchSameAsPattern(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher,
            string x,
            NonNull<string> description)
        {
            var result = pattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                pattern.Pipe(matcher, description.Get).MatchAsync(x).Result == matcher(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionShouldHaveSameDescriptionAsPattern(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher)
            => (pattern.Pipe(matcher).Description == pattern.Description).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property PipePatternWithFunctionAndDescriptionShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
            => (pattern.Pipe(matcher, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionShouldThrowIfPatternIsNull(Func<string, Task<MatchResult<string>>> matcher)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Pipe(matcher);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfPatternIsNull(
            Func<string, Task<MatchResult<string>>> matcher,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Pipe(matcher, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionShouldThrowIfMatcherIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Pipe((Func<string, Task<MatchResult<int>>>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfMatcherIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Pipe((Func<string, Task<MatchResult<int>>>)null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void PipePatternWithFunctionAndDescriptionShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<MatchResult<string>>> matcher)
        {
            Action action = () => pattern.Pipe(matcher, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldNeverReturnNull(IAsyncPattern<string, object> pattern)
            => (pattern.Cast<string, object, string>() != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, object> pattern,
            NonNull<string> description)
            => (pattern.Cast<string, object, string>(description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldMatchSameAsPatterns(IAsyncPattern<string, object> pattern, string x)
        {
            var result = pattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                    pattern.Cast<string, object, string>().MatchAsync(x).Result ==
                    AsyncPattern.Type<object, string>().MatchAsync(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternWithDescriptionShouldMatchSameAsPattern(
            IAsyncPattern<string, object> pattern,
            string x,
            NonNull<string> description)
        {
            var result = pattern.MatchAsync(x);
            return result.Result.IsSuccessful.ImpliesThat(() =>
                pattern.Cast<string, object, string>(description.Get).MatchAsync(x).Result ==
                AsyncPattern.Type<object, string>().MatchAsync(result.Result.Value).Result)
                .ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldHaveCorrectDescription(IAsyncPattern<string, object> pattern)
            => (pattern.Description.Length > 0).ImpliesThat(() =>
                    pattern.Cast<string, object, string>().Description ==
                    String.Format(
                        AsyncPattern.DefaultPipeDescriptionFormat,
                        pattern.Description,
                        String.Format(AsyncPattern.DefaultTypeDescriptionFormat, typeof(string))))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty)
                    .Select(value => (object)value)
                    .Cast<string, object, string>()
                    .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property CastPatternWithDescriptionShouldHaveSpecifiedDescription(
            IAsyncPattern<string, object> pattern,
            NonNull<string> description)
            => (pattern.Cast<string, object, string>(description.Get).Description == description.Get).ToProperty();

        [Fact]
        public void CastPatternShouldThrowIfPatternIsNull()
        {
            Action action = () => ((IAsyncPattern<string, object>)null).Cast<string, object, string>();
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CastPatternWithDescriptionShouldThrowIfPatternIsNull(NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, object>)null).Cast<string, object, string>(description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void CastPatternWithDescriptionShouldThrowIfDescriptionIsNull(IAsyncPattern<string, object> pattern)
        {
            Action action = () => pattern.Cast<string, object, string>(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
