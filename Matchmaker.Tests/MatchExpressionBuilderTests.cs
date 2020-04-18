using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;

using Xunit;

namespace Matchmaker
{
    public class MatchExpressionBuilderTests
    {
        [Fact]
        public void MatchCreateStaticShouldNeverReturnNull()
            => Match.CreateStatic<int, string>(match => { })
                .Should()
                .NotBeNull();

        [Fact]
        public void MatchCreateStaticShouldCreateExpressionOnce()
        {
            int counter = 0;

            for (int i = 0; i < 5; i++)
            {
                Match.CreateStatic<int, string>(match => { counter++; });
            }

            counter.Should().Be(1);
        }

        [Fact]
        public void MatchClearCacheShouldForceStaticMatchCreation()
        {
            int counter = 0;

            void CreateMatchExression()
                => Match.CreateStatic<int, string>(match => { counter++; });

            CreateMatchExression();

            Match.ClearCache<int, string>();

            CreateMatchExression();

            counter.Should().Be(2);
        }

        [Fact]
        public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
        {
            Action action = () => Match.CreateStatic<int, string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void MatchCreateStaticShouldThrowIfFilePathIsNull()
        {
            Action action = () => Match.CreateStatic<int, string>(match => { }, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            bool matchSuccessful = Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteOn(value);

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteNonStrict(value);

            bool matchSuccessful = result.IsSuccessful && result.Value;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => null))
                .ExecuteNonStrict(value);

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteNonStrict(value);

            var matchSuccessful = result.IsSuccessful ? result.Value : null;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteNonStrict(value);

            return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteOn(value);

            if (pattern.Match(value).IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteNonStrict(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public Property MatchWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, int>();

            var pattern = Pattern.CreatePattern(predicate);

            int count = 0;

            Match.CreateStatic<string, int>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => count++)
                    .Case(Pattern.Any<string>(), _ => count++))
                .ExecuteWithFallthrough(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, int>();

            var pattern = Pattern.CreatePattern(predicate);

            int count = 0;

            Match.CreateStatic<string, int>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => count++)
                    .Case(Pattern.Any<string>(), fallthrough: true, _ => count++))
                .ExecuteWithFallthrough(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteWithFallthrough(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern<string>(_ => false);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true))
                .ExecuteWithFallthrough(value);

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            bool matchSuccessful = Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunction()(value);

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToNonStrictFunction()(value);

            bool matchSuccessful = result.IsSuccessful && result.Value;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => null))
                .ToNonStrictFunction()(value);

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == true == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToNonStrictFunction()(value);

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToNonStrictFunction()(value);

            return (result.IsSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToFunction()(value);

            if (pattern.Match(value).IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToNonStrictFunction()(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public Property MatchToFunctionWithFallthroughShouldBeLazy(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, int>();

            var pattern = Pattern.CreatePattern(predicate);

            int count = 0;

            Match.CreateStatic<string, int>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => count++)
                    .Case(Pattern.Any<string>(), _ => count++))
                .ToFunctionWithFallthrough()(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool?>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool?>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public Property MatchToFunctionWithFallthroughTrueShouldBeLazy(Func<string, bool> predicate, string value)
        {
            Match.ClearCache<string, int>();

            var pattern = Pattern.CreatePattern(predicate);

            int count = 0;

            Match.CreateStatic<string, int>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => count++)
                    .Case(Pattern.Any<string>(), fallthrough: true, _ => count++))
                .ToFunctionWithFallthrough()(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(
            Func<string, bool> predicate,
            string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            Match.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern<string>(_ => false);

            var result = Match.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true))
                .ToFunctionWithFallthrough()(value);

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Fact]
        public void MatchShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case<string>(null, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case(pattern, null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeFunctionIsNull()
        {
            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case<string>(null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case<string>(null, fallthrough, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case(pattern, fallthrough, null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.CreateStatic<string, bool>(match => match.Case<string>(fallthrough, null));

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
