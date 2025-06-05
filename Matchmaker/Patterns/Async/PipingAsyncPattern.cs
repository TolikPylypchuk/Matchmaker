namespace Matchmaker.Patterns.Async;

/// <summary>
/// Represents a pattern which pipes another pattern's result to a different pattern.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TIntermediateResult">The type of the result of the first pattern's match.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
internal sealed class PipingAsyncPattern<TInput, TIntermediateResult, TMatchResult>
    : AsyncPattern<TInput, TMatchResult>
{
    /// <summary>
    /// The pattern whose result should be piped.
    /// </summary>
    private readonly IAsyncPattern<TInput, TIntermediateResult> firstPattern;

    /// <summary>
    /// The pattern whose input is the first pattern's output.
    /// </summary>
    private readonly IAsyncPattern<TIntermediateResult, TMatchResult> secondPattern;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PipingAsyncPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
    /// </summary>
    /// <param name="firstPattern">The pattern whose result should be piped.</param>
    /// <param name="secondPattern">The pattern whose input is the first pattern's output.</param>
    internal PipingAsyncPattern(
        IAsyncPattern<TInput, TIntermediateResult> firstPattern,
        IAsyncPattern<TIntermediateResult, TMatchResult> secondPattern)
        : this(
            firstPattern,
            secondPattern,
            firstPattern.Description.Length > 0 && secondPattern.Description.Length > 0
                ? String.Format(
                    AsyncPattern.DefaultPipeDescriptionFormat, firstPattern.Description, secondPattern.Description)
                : String.Empty)
    { }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PipingAsyncPattern{TInput, TIntermediateResult, TMatchResult}" /> class.
    /// </summary>
    /// <param name="firstPattern">The pattern whose result should be piped.</param>
    /// <param name="secondPattern">The pattern whose input is the first pattern's output.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    internal PipingAsyncPattern(
        IAsyncPattern<TInput, TIntermediateResult> firstPattern,
        IAsyncPattern<TIntermediateResult, TMatchResult> secondPattern,
        string description)
        : base(description)
    {
        this.firstPattern = firstPattern;
        this.secondPattern = secondPattern;
    }

    /// <summary>
    /// Matches the input with this pattern, and returns a transformed result asynchronously.
    /// </summary>
    /// <param name="input">The input value to match.</param>
    /// <returns>
    /// A successful match result which contains the transformed result of the match,
    /// if this match is successful. Otherwise, a failed match result.
    /// </returns>
    public override Task<MatchResult<TMatchResult>> MatchAsync(TInput input) =>
        this.firstPattern.MatchAsync(input)
            .Bind(result => this.secondPattern.MatchAsync(result));
}
