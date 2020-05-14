using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

using Xunit;

namespace Matchmaker
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
    [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
    public class AsyncMatchExpressionBuilderTests
    {
        [Fact]
        public void MatchCreateStaticShouldNeverReturnNull()
            => AsyncMatch.CreateStatic<int, string>(match => { })
                .Should()
                .NotBeNull();

        [Fact]
        public void MatchCreateStaticShouldCreateExpressionOnce()
        {
            int counter = 0;

            for (int i = 0; i < 5; i++)
            {
                AsyncMatch.CreateStatic<int, string>(match => { counter++; });
            }

            counter.Should().Be(1);
        }

        [Fact]
        public void MatchClearCacheShouldForceStaticMatchCreation()
        {
            int counter = 0;

            void CreateMatchExression()
                => AsyncMatch.CreateStatic<int, string>(match => { counter++; });

            CreateMatchExression();

            AsyncMatch.ClearCache<int, string>();

            CreateMatchExression();

            counter.Should().Be(2);
        }

        [Fact]
        public void MatchCreateStaticShouldThrowIfBuildActionIsNull()
        {
            Action action = () => AsyncMatch.CreateStatic<int, string>(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchCreateStaticShouldThrowIfFilePathIsNull()
        {
            Action action = () => AsyncMatch.CreateStatic<int, string>(match => { }, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            bool matchSuccessful = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => Task.FromResult(true))
                    .Case(AsyncPattern.Any<string>(), _ => Task.FromResult(false)))
                .ExecuteAsync(value)
                .Result;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            bool matchSuccessful = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteAsync(value)
                .Result;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
            Func<string, bool> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            bool matchSuccessful = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => Task.FromResult(true))
                    .Case(Pattern.Any<string>(), _ => Task.FromResult(false)))
                .ExecuteAsync(value)
                .Result;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
            Func<string, bool> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = Pattern.CreatePattern(predicate);

            bool matchSuccessful = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(Pattern.Any<string>(), _ => false))
                .ExecuteAsync(value)
                .Result;

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteNonStrictAsync(value)
                .Result;

            bool matchSuccessful = result.IsSuccessful && result.Value;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => Task.FromResult<bool?>(null)))
                .ExecuteNonStrictAsync(value)
                .Result;

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == true == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteNonStrictAsync(value)
                .Result;

            var matchSuccessful = result.IsSuccessful ? result.Value : null;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteNonStrictAsync(value)
                    .Result;

            return (result.IsSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteAsync(value)
                    .Result;
            };

            if (pattern.MatchAsync(value).Result.IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ExecuteNonStrictAsync(value)
                    .Result;
            };

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, int>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            _ = AsyncMatch.CreateStatic<string, int>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => count++)
                    .Case(AsyncPattern.Any<string>(), _ => count++))
                .ExecuteWithFallthroughAsync(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, int>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            _ = AsyncMatch.CreateStatic<string, int>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => count++)
                    .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++))
                .ExecuteWithFallthroughAsync(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ExecuteWithFallthroughAsync(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true))
                .ExecuteWithFallthroughAsync(value)
                .ToListAsync()
                .Result;

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            bool matchSuccessful = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunction()(value)
                .Result;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToNonStrictFunction()(value)
                .Result;

            bool matchSuccessful = result.IsSuccessful && result.Value;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => Task.FromResult<bool?>(null)))
                .ToNonStrictFunction()(value)
                .Result;

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == true == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToNonStrictFunction()(value)
                .Result;

            var matchSuccessful = result.IsSuccessful ? result.Value : false;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToNonStrictFunction()(value)
                    .Result;

            return (result.IsSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToFunction()(value)
                    .Result;
            };

            if (pattern.MatchAsync(value).Result.IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.CreateStatic<string, bool>(match => match
                        .Case(pattern, _ => true))
                    .ToNonStrictFunction()(value)
                    .Result;
            };

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, int>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            AsyncMatch.CreateStatic<string, int>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => count++)
                    .Case(AsyncPattern.Any<string>(), _ => count++))
                .ToFunctionWithFallthrough()(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(true)
                    .Case(pattern, fallthrough: false, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool?>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool?>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            AsyncMatch.ClearCache<string, int>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            AsyncMatch.CreateStatic<string, int>(match => match
                    .Fallthrough(false)
                    .Case(pattern, fallthrough: true, _ => count++)
                    .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++))
                .ToFunctionWithFallthrough()(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(
            Func<string, Task<bool>> predicate,
            string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern(predicate);

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true)
                    .Case(AsyncPattern.Any<string>(), _ => false))
                .ToFunctionWithFallthrough()(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            AsyncMatch.ClearCache<string, bool>();

            var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

            var result = AsyncMatch.CreateStatic<string, bool>(match => match
                    .Fallthrough(true)
                    .Case(pattern, _ => true))
                .ToFunctionWithFallthrough()(value)
                .ToListAsync()
                .Result;

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Fact]
        public void MatchShouldThrowIfAsyncPatternIsNull()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IAsyncPattern<string, string>)null, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IPattern<string, string>)null, _ => Task.FromResult(true)));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IAsyncPattern<string, string>)null, _ => Task.FromResult(true)));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IPattern<string, string>)null, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, (Func<string, Task<bool>>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((Func<string, Task<bool>>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((Func<string, bool>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.FromResult(true)));

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IAsyncPattern<string, string>)null, fallthrough, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IPattern<string, string>)null, fallthrough, _ => Task.FromResult(true)));

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case((IPattern<string, string>)null, fallthrough, _ => true));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
            Func<string, Task<bool>> predicate,
            bool fallthrough)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, fallthrough, (Func<string, Task<bool>>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
            Func<string, Task<bool>> predicate,
            bool fallthrough)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, fallthrough, (Func<string, bool>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, fallthrough, (Func<string, Task<bool>>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(pattern, fallthrough, (Func<string, bool>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(fallthrough, (Func<string, Task<bool>>)null));

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.CreateStatic<string, bool>(match => match
                    .Case(fallthrough, (Func<string, bool>)null));

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
