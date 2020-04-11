using System;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class ComposeOrTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldBeSameAsEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Or).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> orPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.Or, description).Match(x).IsSuccessful;
            return orPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(
                    pattern1.Compose(pattern2, PatternComposition.Or).Description ==
                    String.Format(Pattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Compose(pattern, PatternComposition.Or)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.Or)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty)
                .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.Or)
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeOrPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> orPatternHasSpecifiedDescription = () =>
                pattern1.Compose(pattern2, PatternComposition.Or, description).Description == description;
            return orPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).Compose(pattern, PatternComposition.Or);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Compose(
                    pattern, PatternComposition.Or, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Compose(null, PatternComposition.Or);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Compose(null, PatternComposition.Or, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeOrPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Compose(pattern2, PatternComposition.Or, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldBeSameAsEitherPattern(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Or(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> orPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful || pattern2.Match(x).IsSuccessful) ==
                pattern1.Or(pattern2, description).Match(x).IsSuccessful;
            return orPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(
                    pattern1.Or(pattern2).Description ==
                    String.Format(Pattern.DefaultOrDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).Or(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Or(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).Or(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property OrPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> orPatternHasSpecifiedDescription = () =>
                pattern1.Or(pattern2, description).Description == description;
            return orPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).Or(pattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Or(pattern, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Or(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Or(null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void OrPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Or(pattern2, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
