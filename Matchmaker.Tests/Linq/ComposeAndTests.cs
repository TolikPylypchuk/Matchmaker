using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class ComposeAndTests
    {
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.And).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> andPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                pattern1.Compose(pattern2, PatternComposition.And, description).Match(x).IsSuccessful;
            return andPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                pattern1.Compose(pattern2, PatternComposition.And).Description ==
                 String.Format(Pattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty)
                .Compose(pattern, PatternComposition.And).Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.Compose(Pattern.CreatePattern(predicate, String.Empty), PatternComposition.And)
                .Description.Length == 0)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty)
                .Compose(Pattern.CreatePattern(predicate2, String.Empty), PatternComposition.And)
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ComposeAndPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> andPatternHasSpecifiedDescription = () =>
                pattern1.Compose(pattern2, PatternComposition.And, description).Description == description;
            return andPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeAndPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).Compose(pattern, PatternComposition.And);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeAndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).Compose(
                    pattern, PatternComposition.And, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeAndPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.Compose(null, PatternComposition.And);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeAndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.Compose(null, PatternComposition.And, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void ComposeAndPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.Compose(pattern2, PatternComposition.And, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x)
            => ((pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                pattern1.And(pattern2).Match(x).IsSuccessful)
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternWithDescriptionShouldBeSameAsBothPatterns(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string x,
            string description)
        {
            Func<bool> andPatternIsSameAsBothPatterns = () =>
                (pattern1.Match(x).IsSuccessful && pattern2.Match(x).IsSuccessful) ==
                pattern1.And(pattern2, description).Match(x).IsSuccessful;
            return andPatternIsSameAsBothPatterns.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Description.Length > 0 && pattern2.Description.Length > 0).ImpliesThat(() =>
                    pattern1.And(pattern2).Description ==
                    String.Format(Pattern.DefaultAndDescriptionFormat, pattern1.Description, pattern2.Description))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfFirstPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (Pattern.CreatePattern(predicate, String.Empty).And(pattern).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfSecondPatternHasEmptyDescription(
            IPattern<string, string> pattern,
            Func<string, bool> predicate)
            => (pattern.And(Pattern.CreatePattern(predicate, String.Empty)).Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveEmptyDescriptionIfBothPatternsHaveEmptyDescription(
            Func<string, bool> predicate1,
            Func<string, bool> predicate2)
            => (Pattern.CreatePattern(predicate1, String.Empty).And(Pattern.CreatePattern(predicate2, String.Empty))
                .Description.Length == 0).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property AndPatternShouldHaveSpecifiedDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2,
            string description)
        {
            Func<bool> andPatternHasSpecifiedDescription = () =>
                pattern1.And(pattern2, description).Description == description;
            return andPatternHasSpecifiedDescription.When(description != null);
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AndPatternShouldThrowIfLeftPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => ((IPattern<string, string>)null).And(pattern);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AndPatternWithDescriptionShouldThrowIfLeftPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => ((IPattern<string, string>)null).And(pattern, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AndPatternShouldThrowIfRightPatternIsNull(IPattern<string, string> pattern)
        {
            Action action = () => pattern.And(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AndPatternWithDescriptionShouldThrowIfRightPatternIsNull(
            IPattern<string, string> pattern,
            string description)
        {
            if (description != null)
            {
                Action action = () => pattern.And(null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void AndPatternShouldThrowIfDescriptionIsNull(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
        {
            Action action = () => pattern1.And(pattern2, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
