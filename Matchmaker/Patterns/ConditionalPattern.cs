namespace Matchmaker.Patterns;

using System;

using Matchmaker.Linq;

/// <summary>
/// Represents a pattern with an additional condition.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
internal sealed class ConditionalPattern<TInput, TMatchResult> : Pattern<TInput, TMatchResult>
{
    /// <summary>
    /// The pattern to which the condition should be added.
    /// </summary>
    private readonly IPattern<TInput, TMatchResult> pattern;

    /// <summary>
    /// The condition to add to the pattern.
    /// </summary>
    private readonly Func<TMatchResult, bool> condition;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern to which to add a condition.</param>
    /// <param name="condition">The condition to add.</param>
    internal ConditionalPattern(IPattern<TInput, TMatchResult> pattern, Func<TMatchResult, bool> condition)
        : this(pattern, condition, pattern.Description)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern to which to add a condition.</param>
    /// <param name="condition">The condition to add.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    internal ConditionalPattern(
        IPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, bool> condition,
        string description)
        : base(description)
    {
        this.pattern = pattern;
        this.condition = condition;
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
            .Where(this.condition);
}
