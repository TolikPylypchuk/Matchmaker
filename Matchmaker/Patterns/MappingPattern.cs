using System;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern which maps another pattern's result.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the provided pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    internal sealed class MappingPattern<TInput, TIntermediateResult, TMatchResult> : Pattern<TInput, TMatchResult>
    {
        /// <summary>
        /// The pattern whose result should be mapped.
        /// </summary>
        private readonly IPattern<TInput, TIntermediateResult> pattern;

        /// <summary>
        /// The result mapping function.
        /// </summary>
        private readonly Func<TIntermediateResult, TMatchResult> mapper;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MappingPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        internal MappingPattern(
            IPattern<TInput, TIntermediateResult> pattern,
            Func<TIntermediateResult, TMatchResult> mapper)
            : this(pattern, mapper, pattern.Description)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MappingPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
        /// </summary>
        /// <param name="pattern">The pattern whose result should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal MappingPattern(
            IPattern<TInput, TIntermediateResult> pattern,
            Func<TIntermediateResult, TMatchResult> mapper,
            string description)
            : base(description)
        {
            this.pattern = pattern;
            this.mapper = mapper;
        }

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override MatchResult<TMatchResult> Match(TInput input)
        {
            var result = this.pattern.Match(input);
            return result.IsSuccessful
                ? MatchResult.Success(this.mapper(result.Value))
                : MatchResult.Failure<TMatchResult>();
        }
    }
}
