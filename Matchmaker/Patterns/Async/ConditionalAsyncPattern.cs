using System;
using System.Threading.Tasks;

using Matchmaker.Linq;

namespace Matchmaker.Patterns.Async
{
    /// <summary>
    /// Represents an asynchronous pattern with an additional condition.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    internal sealed class ConditionalAsyncPattern<TInput, TMatchResult> : AsyncPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// The pattern to which the condition should be added.
        /// </summary>
        private readonly IAsyncPattern<TInput, TMatchResult> pattern;

        /// <summary>
        /// The condition to add to the pattern.
        /// </summary>
        private readonly Func<TMatchResult, Task<bool>> condition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalAsyncPattern{TInput, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern to which to add a condition.</param>
        /// <param name="condition">The condition to add.</param>
        internal ConditionalAsyncPattern(
            IAsyncPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, Task<bool>> condition)
            : this(pattern, condition, pattern.Description)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalAsyncPattern{TInput, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern to which to add a condition.</param>
        /// <param name="condition">The condition to add.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal ConditionalAsyncPattern(
            IAsyncPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, Task<bool>> condition,
            string description)
            : base(description)
        {
            this.pattern = pattern;
            this.condition = condition;
        }

        /// <summary>
        /// Matches the input with this pattern asynchronously, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input)
            => this.pattern.MatchAsync(input)
                .Where(this.condition);
    }
}
