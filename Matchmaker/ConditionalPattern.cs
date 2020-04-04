using System;
using System.Collections.Immutable;

namespace Matchmaker
{
    /// <summary>
    /// Represents a pattern, to which additional conditions may be added.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <typeparam name="TPattern">The type of the pattern.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    public abstract class ConditionalPattern<TInput, TMatchResult, TPattern> : IPattern<TInput, TMatchResult>
        where TPattern : ConditionalPattern<TInput, TMatchResult, TPattern>
    {
        /// <summary>
        /// A list of predicates, which specify the conditions of this pattern.
        /// </summary>
        protected readonly IImmutableList<Func<TMatchResult, bool>> Conditions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult, TPattern}" /> class
        /// without any conditions.
        /// </summary>
        protected ConditionalPattern()
            => this.Conditions = ImmutableList<Func<TMatchResult, bool>>.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult, TPattern}" /> class
        /// with the specified conditions.
        /// </summary>
        /// <param name="conditions">The conditions of this pattern.</param>
        protected ConditionalPattern(IImmutableList<Func<TMatchResult, bool>> conditions)
            => this.Conditions = conditions;

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public abstract MatchResult<TMatchResult> Match(TInput input);

        /// <summary>
        /// Returns a new pattern, which includes the specified condition.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        /// <returns>A new pattern, which includes the specified condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> is <see langword="null" />.
        /// </exception>
        public abstract TPattern When(Func<TMatchResult, bool> condition);

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        MatchResult<object> IPattern<TInput>.Match(TInput input)
        {
            var result = this.Match(input);
            return result.IsSuccessful
                ? MatchResult.Success<object>(result.Value)
                : MatchResult.Failure<object>();
        }
    }
}
