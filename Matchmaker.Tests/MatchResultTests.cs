namespace Matchmaker;

public class MatchResultTests
{
    [Property]
    public Property SuccessConstructionShouldBeCorrect(string value)
    {
        var result = MatchResult.Success(value);
        return (result.IsSuccessful && result.Value == value).ToProperty();
    }

    [Fact]
    public void FailureConstructionShouldBeCorrect()
    {
        var result = MatchResult.Failure<string>();
        var getValue = () => { string _ = result.Value; };

        result.IsSuccessful.Should().BeFalse();
        getValue.Should().Throw<InvalidOperationException>();
    }

    [Property]
    public Property EqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left.Equals(right) ==
            (left.IsSuccessful == right.IsSuccessful &&
             left.IsSuccessful.ImpliesThat(() => Equals(left.Value, right.Value))))
            .ToProperty();

    [Property]
    public Property ObjectEqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left.Equals((object)right) ==
            (left.IsSuccessful == right.IsSuccessful &&
             left.IsSuccessful.ImpliesThat(() => Equals(left.Value, right.Value))))
            .ToProperty();

    [Property]
    public Property EqualsShouldBeReflexive(MatchResult<string> result) =>
        result.Equals(result).ToProperty();

    [Property]
    public Property EqualsShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        left.Equals(right).ImpliesThat(right.Equals(left)).ToProperty();

    [Property]
    public Property EqualsShouldBeTransitive(MatchResult<string> x, MatchResult<string> y, MatchResult<string> z) =>
        (x.Equals(y) && y.Equals(z)).ImpliesThat(x.Equals(z)).ToProperty();

    [Property]
    public Property EqualsNullShouldBeFalse(MatchResult<string> result) =>
        (!result.Equals(null)).ToProperty();

    [Property]
    public Property EqualsAnotherTypeShouldBeFalse(MatchResult<string> left, string right) =>
        (!left.Equals(right)).ToProperty();

    [Property]
    public Property GetHashCodeShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        left.Equals(right).ImpliesThat(left.GetHashCode() == right.GetHashCode()).ToProperty();

    [Property]
    public Property EqualityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left == right == left.Equals(right)).ToProperty();

    [Property]
    public Property InequalityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left != right == !(left == right)).ToProperty();

    [Property]
    public Property EqualityOperatorShouldBeReflexive(MatchResult<string> result) =>
        (result == result).ToProperty();

    [Property]
    public Property EqualityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        (left == right).ImpliesThat(right == left).ToProperty();

    [Property]
    public Property InequalityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        (left != right).ImpliesThat(right != left).ToProperty();

    [Property]
    public Property EqualityOperatorShouldBeTransitive(
        MatchResult<string> x,
        MatchResult<string> y,
        MatchResult<string> z) =>
        (x == y && y == z).ImpliesThat(x == z).ToProperty();

    [Property]
    public Property ToStringShouldRepresentResultCorrectly(MatchResult<string> result) =>
        (result.ToString() == (result.IsSuccessful ? $"Success: {result.Value}" : "Failure")).ToProperty();
}
