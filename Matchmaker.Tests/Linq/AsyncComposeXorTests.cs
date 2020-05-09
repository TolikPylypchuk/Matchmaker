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
    public class AsyncComposeXorTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Compose(pattern2, PatternComposition.Xor) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldBeSameAsExclusiveEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful ^ pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Xor).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternWithDescriptionShouldBeSameAsExclusiveEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x,
            NonNull<string> description)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful ^ pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveCorrectDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
                 String.Format(AsyncPattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty)
                .Compose(pattern, PatternComposition.Xor).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (pattern.Compose(AsyncPattern.CreatePattern(predicate, String.Empty), PatternComposition.Xor)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, Task<bool>> predicate1,
            Func<string, Task<bool>> predicate2)
            => (AsyncPattern.CreatePattern(predicate1, String.Empty)
                .Compose(AsyncPattern.CreatePattern(predicate2, String.Empty), PatternComposition.Xor)
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Compose(pattern2, PatternComposition.Xor, description.Get).Description == description.Get)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Compose(pattern, PatternComposition.Xor);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Compose(
                pattern, PatternComposition.Xor, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Xor);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Xor, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Compose(pattern2, PatternComposition.Xor, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Xor(pattern2) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternWithDescriptionShouldNeverReturnNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Xor(pattern2, description.Get) != null).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldBeSameAsExclusiveEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful ^ pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Xor(pattern2).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternWithDescriptionShouldBeSameAsExclusiveEitherPattern(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            string x,
            NonNull<string> description)
            => ((pattern1.MatchAsync(x).Result.IsSuccessful ^ pattern2.MatchAsync(x).Result.IsSuccessful) ==
                pattern1.Xor(pattern2, description.Get).MatchAsync(x).Result.IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveCorrectDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                    pattern1.Xor(pattern2).Description ==
                    String.Format(AsyncPattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (AsyncPattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IAsyncPattern<string, string> pattern,
            Func<string, Task<bool>> predicate)
            => (pattern.Xor(AsyncPattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, Task<bool>> predicate1,
            Func<string, Task<bool>> predicate2)
            => (AsyncPattern.CreatePattern(predicate1, String.Empty).Xor(
                    AsyncPattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveSpecifiedDescription(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2,
            NonNull<string> description)
            => (pattern1.Xor(pattern2, description.Get).Description == description.Get).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfLeftPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Xor(pattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => ((IAsyncPattern<string, string>)null).Xor(pattern, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfRightPatternIsNull(IAsyncPattern<string, string> pattern)
        {
            Action action = () => pattern.Xor(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IAsyncPattern<string, string> pattern,
            NonNull<string> description)
        {
            Action action = () => pattern.Xor(null, description.Get);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfDescriptionIsNull(
            IAsyncPattern<string, string> pattern1,
            IAsyncPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Xor(pattern2, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
