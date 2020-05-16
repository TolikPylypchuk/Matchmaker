using System;
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
    [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
    public class AsyncMatchStatementTests
    {
        [Fact]
        public void MatchCreateShouldNeverReturnNull()
            => AsyncMatch.Create<int>()
                .Should()
                .NotBeNull();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault)
            => AsyncMatch.Create<int>(fallthroughByDefault)
                .Should()
                .NotBeNull();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndAsyncAction(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            AsyncMatch.Create<string>()
                .Case(pattern, _ =>
                {
                    matchSuccessful = true;
                    return Task.CompletedTask;
                })
                .Case(AsyncPattern.Any<string>(), _ =>
                {
                    matchSuccessful = false;
                    return Task.CompletedTask;
                })
                .ExecuteAsync(value)
                .Wait();

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithAsyncPatternAndSyncAction(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            AsyncMatch.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
                .ExecuteAsync(value)
                .Wait();

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithSyncPatternAndAsyncAction(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = Pattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            AsyncMatch.Create<string>()
                .Case(pattern, _ =>
                {
                    matchSuccessful = true;
                    return Task.CompletedTask;
                })
                .Case(Pattern.Any<string>(), _ =>
                {
                    matchSuccessful = false;
                    return Task.CompletedTask;
                })
                .ExecuteAsync(value)
                .Wait();

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectlyWithSyncPatternAndSyncAction(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = Pattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            AsyncMatch.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
                .ExecuteAsync(value)
                .Wait();

            return (matchSuccessful == pattern.Match(value).IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            _ = AsyncMatch.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
                .ExecuteNonStrictAsync(value)
                .Result;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, _ => { })
                    .ExecuteAsync(value)
                    .Wait();

            if (pattern.MatchAsync(value).Result.IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldReturnFalseIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            bool matched = AsyncMatch.Create<string>()
                .Case(pattern, _ => { })
                .ExecuteNonStrictAsync(value)
                .Result;

            return (matched == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.Create<string>()
                    .Case(pattern, _ => { })
                    .ExecuteNonStrictAsync(value)
                    .Result;
            };

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthroughAsync(value)
                .CountAsync()
                .Result;

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            _ = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => count++)
                .Case(AsyncPattern.Any<string>(), _ => count++)
                .ExecuteWithFallthroughAsync(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthroughAsync(value)
                .CountAsync()
                .Result;

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthroughAsync(value)
                .CountAsync()
                .Result;

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldBeLazy(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            int count = 0;

            _ = AsyncMatch.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => count++)
                .Case(AsyncPattern.Any<string>(), fallthrough: true, _ => count++)
                .ExecuteWithFallthroughAsync(value);

            return (count == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { })
                .ExecuteWithFallthroughAsync(value)
                .CountAsync()
                .Result;

            return (result == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            AsyncMatch.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
                .ToFunction()(value)
                .Wait();

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            bool matchSuccessful = false;

            _ = AsyncMatch.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(AsyncPattern.Any<string>(), _ => matchSuccessful = false)
                .ToNonStrictFunction()(value)
                .Result;

            return (matchSuccessful == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, _ => { })
                    .ToFunction()(value)
                    .Wait();

            if (pattern.MatchAsync(value).Result.IsSuccessful)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionShouldReturnFalseIfNoMatchFound(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            bool matched = AsyncMatch.Create<string>()
                .Case(pattern, _ => { })
                .ToNonStrictFunction()(value)
                .Result;

            return (matched == pattern.MatchAsync(value).Result.IsSuccessful).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(Func<string, Task<bool>> predicate, string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
            {
                _ = AsyncMatch.Create<string>()
                    .Case(pattern, _ => { })
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
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value)
                .CountAsync()
                .Result;

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value)
                .CountAsync()
                .Result;

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, Task<bool>> predicate,
            string value)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);
            int matchCount = 0;

            int result = AsyncMatch.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(AsyncPattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value)
                .CountAsync()
                .Result;

            return pattern.MatchAsync(value).Result.IsSuccessful
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldReturnEmptyEnumerableIfNoMatchFound(string value)
        {
            var pattern = AsyncPattern.CreatePattern<string>(_ => Task.FromResult(false));

            int result = AsyncMatch.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { })
                .ToFunctionWithFallthrough()(value)
                .CountAsync()
                .Result;

            return (result == 0).ToProperty();
        }

        [Fact]
        public void MatchShouldThrowIfAsyncPatternIsNull()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IAsyncPattern<string, string>)null, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfSyncPatternIsNullWithAsyncAction()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IPattern<string, string>)null, _ => Task.CompletedTask);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfAsyncPatternIsNullWithAsyncAction()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IAsyncPattern<string, string>)null, _ => Task.CompletedTask);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfSyncPatternIsNullWithSyncAction()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IPattern<string, string>)null, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionIsNullWithSyncAction(Func<string, Task<bool>> predicate)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeAsyncFunctionIsNull()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((Func<string, Task>)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeSyncFunctionIsNull()
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((Action<string>)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IAsyncPattern<string, string>)null, fallthrough, _ => Task.CompletedTask);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfAsyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IAsyncPattern<string, string>)null, fallthrough, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithAsyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IPattern<string, string>)null, fallthrough, _ => Task.CompletedTask);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfSyncPatternWithFallthroughIsNullWithSyncAction(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case((IPattern<string, string>)null, fallthrough, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithAsyncPattern(
            Func<string, Task<bool>> predicate,
            bool fallthrough)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithAsyncPattern(
            Func<string, Task<bool>> predicate,
            bool fallthrough)
        {
            var pattern = AsyncPattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, fallthrough, (Action<string>)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseAsyncFunctionWithFallthroughIsNullWithSyncPattern(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseSyncFunctionWithFallthroughIsNullWithSyncPattern(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = Pattern.CreatePattern(predicate);

            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(pattern, fallthrough, (Action<string>)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeAsyncFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(fallthrough, (Func<string, Task>)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeSyncFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                AsyncMatch.Create<string>()
                    .Case(fallthrough, (Action<string>)null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
