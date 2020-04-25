using System;

using Matchmaker.Linq;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern to match with in a match expression. This class is used to represent
    /// patterns internally in match expressions.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the match expression.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    internal class Pattern<TInput>
    {
        /// <summary>
        /// The function which matches the input.
        /// </summary>
        private readonly Func<TInput, MatchResult<object?>> matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{TInput}" /> class.
        /// </summary>
        /// <param name="matcher">The function which matches the input.</param>
        private Pattern(Func<TInput, MatchResult<object?>> matcher)
            => this.matcher = matcher;

        /// <summary>
        /// Creates an internal pattern from an actual pattern.
        /// </summary>
        /// <typeparam name="TMatchResult">The result of the actual pattern's match.</typeparam>
        /// <param name="pattern">The actual pattern.</param>
        /// <returns>An internal pattern based of an actual pattern.</returns>
        public static Pattern<TInput> FromActualPattern<TMatchResult>(IPattern<TInput, TMatchResult> pattern)
            => new Pattern<TInput>(input => pattern.Match(input).Select(result => (object?)result));

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public MatchResult<object?> Match(TInput input)
            => this.matcher(input);
    }
}
