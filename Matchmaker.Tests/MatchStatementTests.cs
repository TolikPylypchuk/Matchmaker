using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker
{
    public class MatchStatementTests
    {
        [Fact]
        public void MatchCreateShouldNeverReturnNull()
            => Match.Create<int>()
                .Should()
                .NotBeNull();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault)
            => Match.Create<int>(fallthroughByDefault)
                .Should()
                .NotBeNull();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            bool matchSuccessful = false;

            Match.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
                .ExecuteOn(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            bool matchSuccessful = false;

            Match.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
                .ExecuteStrict(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchShouldReturnFalseIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matched = Match.Create<string>()
                .Case(pattern, _ => { })
                .ExecuteOn(value);

            return (matched == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, _ => { })
                    .ExecuteOn(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void StrictMatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, _ => { })
                    .ExecuteStrict(value);

            if (pattern.Match(value).IsSome)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthrough(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthrough(value);

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteWithFallthrough(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldReturn0IfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { })
                .ExecuteWithFallthrough(value);

            return (result == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteStrictWithFallthrough(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteStrictWithFallthrough(value);

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ExecuteStrictWithFallthrough(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void StrictMatchWithFallthroughShouldThrowIfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            Action action = () =>
                Match.Create<string>(fallthroughByDefault: true)
                    .Case(pattern, _ => { })
                    .ExecuteStrictWithFallthrough(value);

            action.Should().Throw<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            bool matchSuccessful = false;

            Match.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
                .ToFunction()(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            bool matchSuccessful = false;

            Match.Create<string>()
                .Case(pattern, _ => matchSuccessful = true)
                .Case(Pattern.Any<string>(), _ => matchSuccessful = false)
                .ToStrictFunction()(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldReturnFalseIfNoMatchFound(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matched = Match.Create<string>()
                .Case(pattern, _ => { })
                .ToFunction()(value);

            return (matched == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchToFunctionShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, _ => { })
                    .ToFunction()(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void StrictMatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, _ => { })
                    .ToStrictFunction()(value);

            if (pattern.Match(value).IsSome)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value);

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToFunctionWithFallthrough()(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldReturn0IfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { })
                .ToFunctionWithFallthrough()(value);

            return (result == 0).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToStrictFunctionWithFallthrough()(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToStrictFunctionWithFallthrough()(value);

            return (result == 1 && matchCount == 1).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);
            int matchCount = 0;

            int result = Match.Create<string>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => { matchCount++; })
                .Case(Pattern.Any<string>(), _ => { matchCount++; })
                .ToStrictFunctionWithFallthrough()(value);

            return pattern.Match(value).IsSome
                ? (result == 2 && matchCount == 2).ToProperty()
                : (result == 1 && matchCount == 1).ToProperty();
        }

        [Fact]
        public void MatchShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(null, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeFunctionIsNull()
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(null, fallthrough, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
