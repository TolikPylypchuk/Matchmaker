using System.Diagnostics.CodeAnalysis;

using FsCheck;
using FsCheck.Xunit;

namespace Matchmaker
{
    public class MatchResultTests
    {
        [Property]
        public Property SuccessConstructionShouldBeCorrect(string value)
        {
            var result = MatchResult.Success(value);
            return (result.IsSuccessful && result.Value == value).ToProperty();
        }

        [Property]
        public Property FailureConstructionShouldBeCorrect()
        {
            var result = MatchResult.Failure<string>();
            return (!result.IsSuccessful && result.Value == default).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right)
            => (left.Equals(right) == (left.IsSuccessful == right.IsSuccessful && Equals(left.Value, right.Value)))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ObjectEqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right)
            => (left.Equals((object)right) ==
                (left.IsSuccessful == right.IsSuccessful && Equals(left.Value, right.Value)))
                .ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualsShouldBeReflexive(MatchResult<string> result)
            => result.Equals(result).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualsShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right)
            => left.Equals(right).ImpliesThat(right.Equals(left)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualsShouldBeTransitive(MatchResult<string> x, MatchResult<string> y, MatchResult<string> z)
            => (x.Equals(y) && y.Equals(z)).ImpliesThat(x.Equals(z)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualsNullShouldBeFalse(MatchResult<string> result)
            => (!result.Equals(null)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public Property EqualsAnotherTypeShouldBeFalse(MatchResult<string> left, string right)
            => (!left.Equals(right)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property GetHashCodeShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right)
            => left.Equals(right).ImpliesThat(left.GetHashCode() == right.GetHashCode()).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right)
            => (left == right == left.Equals(right)).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property InequalityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right)
            => (left != right == !(left == right)).ToProperty();

#pragma warning disable CS1718 // Comparison made to same variable

        [Property(Arbitrary = new[] { typeof(Generators) })]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public Property EqualityOperatorShouldBeReflexive(MatchResult<string> result)
            => (result == result).ToProperty();

#pragma warning restore CS1718 // Comparison made to same variable

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right)
            => (left == right).ImpliesThat(right == left).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property InequalityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right)
            => (left != right).ImpliesThat(right != left).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property EqualityOperatorShouldBeTransitive(
            MatchResult<string> x,
            MatchResult<string> y,
            MatchResult<string> z)
            => (x == y && y == z).ImpliesThat(x == z).ToProperty();

        [Property(Arbitrary = new[] { typeof(Generators) })]
        public Property ToStringShouldRepresentResultCorrectly(MatchResult<string> result)
            => (result.ToString() == (result.IsSuccessful ? $"Success: {result.Value}" : "Failure")).ToProperty();
    }
}
