using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

namespace Matchmaker.Linq
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class AsyncComposeOrTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Compose(pattern2, PatternComposition.Or) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Compose(pattern2, PatternComposition.Or, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldBeSameAsEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful || pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Or).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternWithDescriptionShouldBeSameAsEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x,
            NonNull<string> description)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful || pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Or, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveCorrectDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.Or).Description ==
                 String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty)
                .Compose(pattern, PatternComposition.Or).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.Or)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, Task<bool>> predicate1,
            Func<string, Task<bool>> predicate2)
            => (AsyncPattern.CreatePattern(predicate1, String.Empty)
                .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.Or)
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Compose(pattern2, PatternComposition.Or, description.Get).Description == description.Get)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Compose(
                pattern, PatternComposition.Or, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Or);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Or, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Compose(pattern2, PatternComposition.Or, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Or(pattern2) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Or(pattern2, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldBeSameAsEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful || pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Or(pattern2).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternWithDescriptionShouldBeSameAsEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x,
            NonNull<string> description)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful || pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Or(pattern2, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveCorrectDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                    pattern1.Or(pattern2).Description ==
                    String.Format(AsyncPattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (pattern.Or(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, Task<bool>> predicate1,
            Func<string, Task<bool>> predicate2)
            => (AsyncPattern.CreatePattern(predicate1, String.Empty).Or(
                    AsyncPattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Or(pattern2, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Or(pattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Or(pattern, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Or(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Or(null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Or(pattern2, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
