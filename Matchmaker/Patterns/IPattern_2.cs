namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern to match with in a match expression.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the match expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    /// <seealso cref="Pattern" />
    /// <seealso cref="IConditionalPattern{TInput, TMatchResult, TPattern}" />
    /// <seealso cref="IDescriptivePattern{TInput, TMatchResult}" />
    public interface IPattern<in TInput, TMatchResult> : IPattern<TInput>
    {
        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        new MatchResult<TMatchResult> Match(TInput input);
    }
}
