using System;
using System.Threading.Tasks;

namespace Matchmaker.Patterns.Async
{
    /// <summary>
    /// Represents a base class for asynchronous patterns.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IAsyncPattern{TInput, TMatchResult}" />
    /// <seealso cref="AsyncPattern" />
    public abstract class AsyncPattern<TInput, TMatchResult> : IAsyncPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class.
        /// </summary>
        protected AsyncPattern()
            : this(String.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncPattern{TInput, TMatchResult}" /> class
        /// with the specified description.
        /// </summary>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        protected AsyncPattern(string description)
            => this.Description = description ?? throw new ArgumentNullException(nameof(description));

        /// <summary>
        /// Gets the description of this pattern.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result asynchronously.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public abstract Task<MatchResult<TMatchResult>> MatchAsync(TInput input);

        /// <summary>
        /// Returns the description of this pattern, if it has one.
        /// </summary>
        /// <returns>
        /// The description of this pattern, if it has one.
        /// If it doesn't, then the name of this pattern's type.
        /// </returns>
        public override string ToString()
            => String.IsNullOrEmpty(this.Description) ? base.ToString() : this.Description;
    }
}
