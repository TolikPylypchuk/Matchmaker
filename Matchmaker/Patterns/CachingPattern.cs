namespace Matchmaker.Patterns;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a pattern which caches another pattern's results.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
internal sealed class CachingPattern<TInput, TMatchResult> : Pattern<TInput, TMatchResult>
{
    /// <summary>
    /// The pattern whose result should be cached.
    /// </summary>
    private readonly IPattern<TInput, TMatchResult> pattern;

    /// <summary>
    /// The dictionary which holds this pattern's cache.
    /// </summary>
#nullable disable
    private readonly Dictionary<TInput, MatchResult<TMatchResult>> cache = [];
#nullable enable

    /// <summary>
    /// The cached result for the <see langword="null" /> input.
    /// </summary>
    private MatchResult<TMatchResult> nullResult;

    /// <summary>
    /// The value which indicates whether the result for the <see langword="null" /> input has been cached.
    /// </summary>
    private bool isNullResultDefined;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingPattern{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern whose result should be cached.</param>
    internal CachingPattern(IPattern<TInput, TMatchResult> pattern)
        : this(pattern, pattern.Description)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingPattern{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern whose result should be cached.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    internal CachingPattern(IPattern<TInput, TMatchResult> pattern, string description)
        : base(description) =>
        this.pattern = pattern;

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
        if (input == null)
        {
            if (!this.isNullResultDefined)
            {
                this.nullResult = this.pattern.Match(input);
                this.isNullResultDefined = true;
            }

            return this.nullResult;
        }

        if (!this.cache.TryGetValue(input, out var result))
        {
            result = this.pattern.Match(input);
            this.cache.Add(input, result);
        }

        return result;
    }
}
