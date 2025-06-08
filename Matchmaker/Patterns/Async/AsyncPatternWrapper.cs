namespace Matchmaker.Patterns.Async;

/// <summary>
/// Represents an asynchronous pattern which wraps a synchronous pattern.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
internal sealed class AsyncPatternWrapper<TInput, TMatchResult> : AsyncPattern<TInput, TMatchResult>
{
    /// <summary>
    /// The pattern which should be wrapped.
    /// </summary>
    private readonly IPattern<TInput, TMatchResult> pattern;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncPatternWrapper{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern which should be wrapped.</param>
    internal AsyncPatternWrapper(IPattern<TInput, TMatchResult> pattern)
        : this(pattern, pattern.Description)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncPatternWrapper{TInput, TMatchResult}" /> class.
    /// </summary>
    /// <param name="pattern">The pattern which should be wrapped.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    internal AsyncPatternWrapper(IPattern<TInput, TMatchResult> pattern, string description)
        : base(description) =>
        this.pattern = pattern;

    /// <summary>
    /// Matches the input with this pattern, and returns a transformed result asynchronously.
    /// </summary>
    /// <param name="input">The input value to match.</param>
    /// <returns>
    /// A successful match result which contains the transformed result of the match, if this match is successful.
    /// Otherwise, a failed match result.
    /// </returns>
    public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input) =>
        Task.FromResult(this.pattern.Match(input));
}
