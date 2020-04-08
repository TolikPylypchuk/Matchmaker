using System;
using System.Collections.Immutable;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a general transforming pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="PatternBase{TInput, TMatchResult, TPattern}" />
    /// <seealso cref="SimplePattern{TInput}" />
    /// <seealso cref="Pattern" />
    public sealed class Pattern<TInput, TMatchResult> : PatternBase<TInput, TMatchResult, Pattern<TInput, TMatchResult>>
    {
        /// <summary>
        /// The matcher function.
        /// </summary>
        private readonly Func<TInput, MatchResult<TMatchResult>> matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class
        /// with the specified matcher function.
        /// </summary>
        /// <param name="matcher">The matcher function.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matcher" /> is <see langword="null" />.
        /// </exception>
        internal Pattern(Func<TInput, MatchResult<TMatchResult>> matcher)
            => this.matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class
        /// with the specified matcher function.
        /// </summary>
        /// <param name="matcher">The matcher function.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matcher" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal Pattern(Func<TInput, MatchResult<TMatchResult>> matcher, string description)
            : base(description)
            => this.matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class
        /// with the specified matcher function and additional conditions.
        /// </summary>
        /// <param name="matcher">The matcher function.</param>
        /// <param name="conditions">The additional conditions.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matcher" />, <paramref name="conditions" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        private Pattern(
            Func<TInput, MatchResult<TMatchResult>> matcher,
            IImmutableList<Func<TMatchResult, bool>> conditions,
            string description)
            : base(conditions, description)
            => this.matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result if successful.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override MatchResult<TMatchResult> Match(TInput input)
            => this.CheckConditions(this.matcher(input));

        /// <summary>
        /// Returns a new pattern which includes the specified condition.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        /// <returns>A new pattern which includes the specified condition.</returns>
        public override Pattern<TInput, TMatchResult> When(Func<TMatchResult, bool> condition)
            => new Pattern<TInput, TMatchResult>(this.matcher, this.Conditions.Add(condition), this.Description);

        /// <summary>
        /// Returns a pattern which is matched successfully
        /// when the specified pattern is not matched successfully.
        /// </summary>
        /// <param name="pattern">The pattern to invert.</param>
        /// <returns>
        /// A pattern which is matched successfully
        /// when the specified pattern is not matched successfully.
        /// </returns>
        /// <remarks>
        /// This pattern ignores the specified pattern's transformation
        /// and returns the input value if matched successfully.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> is <see langword="null" />.
        /// </exception>
        public static SimplePattern<TInput> operator ~(Pattern<TInput, TMatchResult> pattern)
            => Pattern.Not(pattern);
    }
}
