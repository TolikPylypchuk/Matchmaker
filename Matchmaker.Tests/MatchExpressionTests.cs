using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker
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
        public Property MatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            bool matchSuccessful = Match.Create<string, bool>()
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteOn(value);

            return (matchSuccessful == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
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
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNull(
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
        public Property NonStrictMatchShouldMatchPatternsCorrectlyWithNullable(
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
        public Property NonStrictMatchShouldReturnNothingIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteNonStrict(value);

            return (result.IsSome == pattern.Match(value).IsSome).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
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
        public void NonStrictMatchShouldNotThrowIfNoMatchFound(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, _ => true)
                    .ExecuteNonStrict(value);

            action.Should().NotThrow<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectly(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property MatchWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteWithFallthrough(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchWithFallthroughShouldThrowIfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            Action action = () =>
                Match.Create<string, bool>(fallthroughByDefault: true)
                    .Case(pattern, _ => true)
                    .ExecuteWithFallthrough(value);

            action.Should().Throw<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property NonStrictMatchWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ExecuteNonStrictWithFallthrough(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchWithFallthroughShouldReturnEmptyListIfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .ExecuteNonStrictWithFallthrough(value);

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionShouldMatchPatternsCorrectly(
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
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectly(
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
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNull(
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
        public Property NonStrictMatchToFunctionShouldMatchPatternsCorrectlyWithNullable(
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
        public Property NonStrictMatchToFunctionShouldReturnNothingIfNoMatchFound(
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
        public void MatchToFunctionShouldThrowIfNoMatchFound(Func<string, bool> predicate, string value)
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
        public void NonStrictMatchToFunctionShouldNotThrowIfNoMatchFound(
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
        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property MatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property MatchToFunctionWithFallthroughShouldNeverReturnNull(Func<string, bool> predicate, string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToFunctionWithFallthrough()(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchToFunctionWithFallthroughShouldThrowIfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            Action action = () =>
                Match.Create<string, bool>(fallthroughByDefault: true)
                    .Case(pattern, _ => true)
                    .ToFunctionWithFallthrough()(value);

            action.Should().Throw<MatchException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool> { true };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughFalseShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: true)
                .Case(pattern, fallthrough: false, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool?> { true };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectly(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool> { true, false };
            var failure = new List<bool> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughTrueShouldMatchPatternsCorrectlyWithNullable(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool?>(fallthroughByDefault: false)
                .Case(pattern, fallthrough: true, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            var success = new List<bool?> { true, false };
            var failure = new List<bool?> { false };

            return pattern.Match(value).IsSome
                ? result.SequenceEqual(success).ToProperty()
                : result.SequenceEqual(failure).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Property NonStrictMatchToFunctionWithFallthroughShouldNeverReturnNull(
            Func<string, bool> predicate,
            string value)
        {
            var pattern = new SimplePattern<string>(predicate);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .Case(Pattern.Any<string>(), _ => false)
                .ToNonStrictFunctionWithFallthrough()(value);

            return (result != null).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property NonStrictMatchToFunctionWithFallthroughShouldReturnEmptyListIfNoMatchFound(string value)
        {
            var pattern = new SimplePattern<string>(_ => false);

            var result = Match.Create<string, bool>(fallthroughByDefault: true)
                .Case(pattern, _ => true)
                .ToNonStrictFunctionWithFallthrough()(value);

            return result.SequenceEqual(Enumerable.Empty<bool>()).ToProperty();
        }

        [Fact]
        public void MatchShouldThrowIfPatternIsNull()
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null, _ => true);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionIsNull(Func<string, bool> predicate)
        {
            var pattern = new SimplePattern<string>(predicate);

            Action action = () =>
                Match.Create<string, bool>()
                    .Case(pattern, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MatchShouldThrowIfCaseTypeFunctionIsNull()
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchShouldThrowIfPatternWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(null, fallthrough, _ => true);

            action.Should().Throw<ArgumentNullException>();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public void MatchShouldThrowIfCaseFunctionWithFallthroughIsNull(
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
        public void MatchShouldThrowIfCaseTypeFunctionWithFallthroughIsNull(bool fallthrough)
        {
            Action action = () =>
                Match.Create<string, bool>()
                    .Case<string>(fallthrough, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
