using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace PatternMatching
{
    public class MatchTests
    {
        [Fact]
        public void MatchCreateShouldNeverReturnNull()
        {
            Match.Create<int, string>()
                .Should()
                .NotBeNull();

            Match.Create<int>()
                .Should()
                .NotBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault)
        {
            Match.Create<int, string>(fallthroughByDefault)
                .Should()
                .NotBeNull();

            Match.Create<int>(fallthroughByDefault)
                .Should()
                .NotBeNull();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchExpressionShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matchSuccessful = Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteOn(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchStatementShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
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
        public Property NonStrictMatchExpressionShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matchSuccessful = Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrict(value)
                .IfNoneUnsafe(() => false);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionShouldMatchPatternsCorrectlyWithNull(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var matchSuccessful = Match.Create<string, bool?>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => null)
                .ExecuteNonStrict(value)
                .IfNoneUnsafe(() => false);

            return (matchSuccessful == true == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var matchSuccessful = Match.Create<string, bool?>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrict(value)
                .IfNoneUnsafe(() => null);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property StrictMatchStatementShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
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
        public Property NonStrictMatchExpressionShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteNonStrict(value);

            return (result.IsSome == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchStatementShouldReturnFalseIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matched = Match.Create<string>()
                .Case(pattern, _ => { })
                .ExecuteOn(value);

            return (matched == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchExpressionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteOn(value);

            if (pattern.Match(value).IsSome)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchStatementShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, _ => { })
                    .ExecuteOn(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchExpressionShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteNonStrict(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void StrictMatchStatementShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
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

        [Fact]
        public void MatchExpressionShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null, _ => true);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchStatementShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(null, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchExpressionShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchStatementShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchExpressionShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null, fallthrough, _ => true);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchStatementShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string>()
                    .Case<string>(null, fallthrough, _ => { });

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchExpressionShouldThrowIfCaseFunctionWithFallthroughIsNull(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchStatementShouldThrowIfCaseFunctionWithFallthroughIsNull(
            Func<string, bool> predicate,
            bool fallthrough)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string>()
                    .Case(pattern, fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
