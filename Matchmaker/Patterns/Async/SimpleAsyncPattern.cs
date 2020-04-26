using System;
using System.Threading.Tasks;

namespace Matchmaker.Patterns.Async
{
    /// <summary>
    /// Represents a pattern which uses a function to match its inputs asynchronously.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IAsyncPattern{TInput, TMatchResult}" />
    /// <seealso cref="AsyncPattern{TInput, TMatchResult}" />
    /// <seealso cref="AsyncPattern" />
    internal sealed class SimpleAsyncPattern<TInput, TMatchResult> : AsyncPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// The matcher function.
        /// </summary>
        private readonly Func<TInput, Task<MatchResult<TMatchResult>>> matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleAsyncPattern{TInput, TMatchResult}" /> class
        /// with the specified matcher function.
        /// </summary>
        /// <param name="matcher">The matcher function.</param>
        internal SimpleAsyncPattern(Func<TInput, Task<MatchResult<TMatchResult>>> matcher)
            => this.matcher = matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleAsyncPattern{TInput, TMatchResult}" /> class
        /// with the specified matcher function and description.
        /// </summary>
        /// <param name="matcher">The matcher function.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal SimpleAsyncPattern(Func<TInput, Task<MatchResult<TMatchResult>>> matcher, string description)
            : base(description)
            => this.matcher = matcher;

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result asynchronously.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input)
            => this.matcher(input);
    }
}
