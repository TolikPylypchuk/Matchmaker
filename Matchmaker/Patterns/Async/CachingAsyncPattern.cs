using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Matchmaker.Patterns.Async
{
    /// <summary>
    /// Represents a pattern which caches another pattern's results.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    internal sealed class CachingAsyncPattern<TInput, TMatchResult> : AsyncPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// The pattern whose result should be cached.
        /// </summary>
        private readonly IAsyncPattern<TInput, TMatchResult> pattern;

        /// <summary>
        /// The dictionary which holds this pattern's cache.
        /// </summary>
        private readonly ConcurrentDictionary<TInput, Task<MatchResult<TMatchResult>>> cache =
            new ConcurrentDictionary<TInput, Task<MatchResult<TMatchResult>>>();

        /// <summary>
        /// The cached result for the <see langword="null" /> input.
        /// </summary>
        private Task<MatchResult<TMatchResult>>? nullResult;

        /// <summary>
        /// The value which indicates whether the result for the <see langword="null" /> input has been cached.
        /// </summary>
        private bool isNullResultDefined;

        /// <summary>
        /// The object on which to lock the caching process of the <see langword="null" /> input.
        /// </summary>
        private readonly object nullResultLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAsyncPattern{TInput, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be cached.</param>
        internal CachingAsyncPattern(IAsyncPattern<TInput, TMatchResult> pattern)
            : this(pattern, pattern.Description)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAsyncPattern{TInput, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be cached.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal CachingAsyncPattern(IAsyncPattern<TInput, TMatchResult> pattern, string description)
            : base(description)
            => this.pattern = pattern;

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result asynchronously.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input)
        {
            if (input == null)
            {
                if (!this.isNullResultDefined)
                {
                    lock (this.nullResultLock)
                    {
                        if (!this.isNullResultDefined)
                        {
                            this.nullResult = this.pattern.MatchAsync(input);
                            this.isNullResultDefined = true;
                        }
                    }
                }

                return this.nullResult!;
            }

            return this.cache.GetOrAdd(input, this.pattern.MatchAsync);
        }
    }
}
