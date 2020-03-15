using System;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace PatternMatching
{
    public class MatchExpressionTests
    {
        [Fact]
        public void MatchCreateShouldNeverReturnNull()
            => Match.Create<int, string>()
                .Should()
                .NotBeNull();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault)
            => Match.Create<int, string>(fallthroughByDefault)
                .Should()
                .NotBeNull();

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
        public Property NonStrictMatchExpressionShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteNonStrict(value);

            return (result.IsSome == pattern.Match(value).IsSome).ToProperty();
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
        public Property MatchExpressionToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matchSuccessful = Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunction()(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionToFunctionShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matchSuccessful = Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunction()(value)
                .IfNoneUnsafe(() => false);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionToFunctionShouldMatchPatternsCorrectlyWithNull(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var matchSuccessful = Match.Create<string, bool?>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => null)
                .ToNonStrictFunction()(value)
                .IfNoneUnsafe(() => false);

            return (matchSuccessful == true == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionToFunctionShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var matchSuccessful = Match.Create<string, bool?>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunction()(value)
                .IfNoneUnsafe(() => null);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchExpressionToFunctionShouldReturnNothingIfNoMatchFound(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ToNonStrictFunction()(value);

            return (result.IsSome == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchExpressionToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ToFunction()(value);

            if (pattern.Match(value).IsSome)
            {
                action.Should().NotThrow<MatchException>();
            } else
            {
                action.Should().Throw<MatchException>();
            }
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void NonStrictMatchExpressionToFunctionShouldNotThrowIfNoMatchFound(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ToNonStrictFunction()(value);

            action.Should().NotThrow<MatchException>();
        }

        [Fact]
        public void MatchExpressionShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null, _ => true);

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

        [Fact]
        public void MatchExpressionShouldThrowIfCaseTypeFunctionIsNull()
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null);

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
        public void MatchExpressionShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}