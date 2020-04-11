namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern to match with in a match expression. This inteface is for internal use and
    /// should never be used directly by the users of the library. Instead, users should use
    /// <see cref="IPattern{TInput, TMatchResult}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the match expression.</typeparam>
    /// <remarks>
    /// <para>
    /// This interface exists only for internal use in the <see cref="Match{TInput, TOutput}" />
    /// and <see cref="Match{TInput}" /> classes, because they can't know about the types of
    /// the transformation results. The public API never uses this interface - it always uses
    /// <see cref="IPattern{TInput, TMatchResult}" />, so type safety is not compromised.
    /// </para>
    /// <para>
    /// Classes shouldn't implement this interface directly - instead, they should implement
    /// <see cref="IPattern{TInput, TMatchResult}" /> and the <see cref="Match(TInput)" /> method
    /// should always be implemented in terms of the more generic
    /// <see cref="IPattern{TInput, TMatchResult}.Match(TInput)" />:
    /// <code>
    /// MatchResult&lt;object&gt; IPattern&lt;TInput&gt;.Match(TInput input)
    ///     =&gt; this.Match(input).Select(result => (object)result);
    /// </code>
    /// </para>
    /// </remarks>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    public interface IPattern<in TInput>
    {
        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        MatchResult<object> Match(TInput input);
    }
}
