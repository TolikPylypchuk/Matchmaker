namespace Matchmaker;

public class MatchResultTests
{
    [Property(DisplayName = "Success construction should be correct")]
    public Property SuccessConstructionShouldBeCorrect(string value)
    {
        var result = MatchResult.Success(value);
        return (result.IsSuccessful && result.Value == value).ToProperty();
    }

    [Fact(DisplayName = "Failure construction should be correct")]
    public void FailureConstructionShouldBeCorrect()
    {
        var result = MatchResult.Failure<string>();

        Assert.False(result.IsSuccessful);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Property(DisplayName = "Equals should work correctly")]
    public Property EqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left.Equals(right) ==
            (left.IsSuccessful == right.IsSuccessful &&
             left.IsSuccessful.ImpliesThat(() => Equals(left.Value, right.Value))))
            .ToProperty();

    [Property(DisplayName = "Object equals should work correctly")]
    public Property ObjectEqualsShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left.Equals((object)right) ==
            (left.IsSuccessful == right.IsSuccessful &&
             left.IsSuccessful.ImpliesThat(() => Equals(left.Value, right.Value))))
            .ToProperty();

    [Property(DisplayName = "Equals should be reflexive")]
    public Property EqualsShouldBeReflexive(MatchResult<string> result) =>
        result.Equals(result).ToProperty();

    [Property(DisplayName = "Equals should be symmetric")]
    public Property EqualsShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        left.Equals(right).ImpliesThat(right.Equals(left)).ToProperty();

    [Property(DisplayName = "Equals should be transitive")]
    public Property EqualsShouldBeTransitive(MatchResult<string> x, MatchResult<string> y, MatchResult<string> z) =>
        (x.Equals(y) && y.Equals(z)).ImpliesThat(x.Equals(z)).ToProperty();

    [Property(DisplayName = "Equals to null should be false")]
    public Property EqualsNullShouldBeFalse(MatchResult<string> result) =>
        (!result.Equals(null)).ToProperty();

    [Property(DisplayName = "Equals to another type should be false")]
    public Property EqualsAnotherTypeShouldBeFalse(MatchResult<string> left, string right) =>
        (!left.Equals(right)).ToProperty();

    [Property(DisplayName = "GetHashCode should work correctly")]
    public Property GetHashCodeShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        left.Equals(right).ImpliesThat(left.GetHashCode() == right.GetHashCode()).ToProperty();

    [Property(DisplayName = "Equality operator should work correctly")]
    public Property EqualityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left == right == left.Equals(right)).ToProperty();

    [Property(DisplayName = "Inequality operator should work correctly")]
    public Property InequalityOperatorShouldWorkCorrectly(MatchResult<string> left, MatchResult<string> right) =>
        (left != right == !(left == right)).ToProperty();

    [Property(DisplayName = "Equality operator should be reflexive")]
    public Property EqualityOperatorShouldBeReflexive(MatchResult<string> result) =>
        (result == result).ToProperty();

    [Property(DisplayName = "Equality operator should be symmetric")]
    public Property EqualityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        (left == right).ImpliesThat(right == left).ToProperty();

    [Property(DisplayName = "Inequality operator should be symmetric")]
    public Property InequalityOperatorShouldBeSymmetric(MatchResult<string> left, MatchResult<string> right) =>
        (left != right).ImpliesThat(right != left).ToProperty();

    [Property(DisplayName = "Equality operator should be transitive")]
    public Property EqualityOperatorShouldBeTransitive(
        MatchResult<string> x,
        MatchResult<string> y,
        MatchResult<string> z) =>
        (x == y && y == z).ImpliesThat(x == z).ToProperty();

    [Property(DisplayName = "ToString should represent the result correctly")]
    public Property ToStringShouldRepresentResultCorrectly(MatchResult<string> result) =>
        (result.ToString() == (result.IsSuccessful ? $"Success: {result.Value}" : "Failure")).ToProperty();
}
