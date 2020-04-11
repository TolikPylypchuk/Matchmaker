using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class ComposeXorTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldBeSameAsExcusiveEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Xor).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> xorPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Xor, description).Match(x).IsSuccessful;
            return xorPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                    pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
                    String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Compose(pattern, PatternComposition.Xor)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.Xor)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty)
                .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.Xor)
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeXorPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> xorPatternHasSpecifiedDescription = () =>
                pattern1.Compose(pattern2, PatternComposition.Xor, description).Description == description;
            return xorPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Compose(
                    pattern, PatternComposition.Xor, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Xor);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Compose(null, PatternComposition.Xor, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeXorPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Compose(pattern2, PatternComposition.Xor, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldBeSameAsExcusiveEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Xor(pattern2).Match(x).IsSuccessful).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> xorPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful ^ pattern2.Match(x).IsSuccessful) ==
                pattern1.Xor(pattern2, description).Match(x).IsSuccessful;
            return xorPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                    pattern1.Xor(pattern2).Description ==
                    String.Format(Pattern.DefaultXorDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Xor(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Xor(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).Xor(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property XorPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> xorPatternHasSpecifiedDescription = () =>
                pattern1.Xor(pattern2, description).Description == description;
            return xorPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).Xor(pattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Xor(pattern, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Xor(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Xor(null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void XorPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Xor(pattern2, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
