using System;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

namespace Matchmaker.Linq
{
    public class MatchExtensionsTests
    {
        [Property]
        public Property EnumerableEnumerateForcesEnumeration(int count)
        {
            count = Math.Abs(count) % 100;

            int result = 0;

            Enumerable.Range(0, count)
                .Select(_ => result++)
                .Enumerate();

            return (result == count).ToProperty();
        }

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
        public Property SelectPatternWithDescriptionShouldHaveCorrectDescription(
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
        public Property ComposeAndPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Compose(pattern2, PatternComposition.And).Description ==
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
        public Property ComposeOrPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Compose(pattern2, PatternComposition.Or).Description ==
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
        public Property ComposeXorPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Compose(pattern2, PatternComposition.Xor).Description ==
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
        public Property AndPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.And(pattern2).Description ==
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
        public Property OrPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Or(pattern2).Description ==
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
        public Property XorPatternShouldHaveCorrectDescription(
            IPattern<string, string> pattern1,
            IPattern<string, string> pattern2)
            => (pattern1.Xor(pattern2).Description ==
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
    }
}
