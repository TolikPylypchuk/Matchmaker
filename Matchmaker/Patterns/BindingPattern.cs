namespace Matchmaker.Patterns;

using System;

using Matchmaker.Linq;

/// <summary>
/// Represents a pattern which binds (flat-maps) another pattern's result.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TIntermediateResult">The type of the result of the provided pattern's match.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
internal sealed class BindingPattern<TInput, TIntermediateResult, TMatchResult> : Pattern<TInput, TMatchResult>
{
    /// <summary>
    /// The pattern whose result should be bound.
    /// </summary>
    private readonly IPattern<TInput, TIntermediateResult> pattern;

    /// <summary>
    /// The result mapping function.
    /// </summary>
    private readonly Func<TIntermediateResult, IPattern<TInput, TMatchResult>> binder;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="BindingPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="binder">The result binding function.</param>
    internal BindingPattern(
        IPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, IPattern<TInput, TMatchResult>> binder)
        : this(pattern, binder, pattern.Description)
    { }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="BindingPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="binder">The result binding function.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    internal BindingPattern(
        IPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, IPattern<TInput, TMatchResult>> binder,
        string description)
        : base(description)
    {
        this.pattern = pattern;
        this.binder = binder;
    }

    /// <summary>
    /// Matches the input with this pattern, and returns a transformed result.
    /// </summary>
    /// <param name="input">The input value to match.</param>
    /// <returns>
    /// A successful match result which contains the transformed result of the match,
    /// if this match is successful. Otherwise, a failed match result.
    /// </returns>
    public override MatchResult<TMatchResult> Match(TInput input) =>
        this.pattern.Match(input)
            .Bind(result => this.binder(result).Match(input));
}
