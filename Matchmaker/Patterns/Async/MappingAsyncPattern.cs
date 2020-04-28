using System;
using System.Threading.Tasks;

using Matchmaker.Linq;

namespace Matchmaker.Patterns.Async
{
    /// <summary>
    /// Represents a pattern which maps another pattern's result.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the provided pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IAsyncPattern{TInput, TMatchResult}" />
    /// <seealso cref="AsyncPattern" />
    internal sealed class MappingAsyncPattern<TInput, TIntermediateResult, TMatchResult>
        : AsyncPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// The pattern whose result should be mapped.
        /// </summary>
        private readonly IAsyncPattern<TInput, TIntermediateResult> pattern;

        /// <summary>
        /// The result mapping function.
        /// </summary>
        private readonly Func<TIntermediateResult, TMatchResult> mapper;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MappingAsyncPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        internal MappingAsyncPattern(
            IAsyncPattern<TInput, TIntermediateResult> pattern,
            Func<TIntermediateResult, TMatchResult> mapper)
            : this(pattern, mapper, pattern.Description)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MappingAsyncPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal MappingAsyncPattern(
            IAsyncPattern<TInput, TIntermediateResult> pattern,
            Func<TIntermediateResult, TMatchResult> mapper,
            string description)
            : base(description)
        {
            this.pattern = pattern;
            this.mapper = mapper;
        }

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result asynchronously.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input)
            => this.pattern.MatchAsync(input)
                .Select(this.mapper);
    }
}
