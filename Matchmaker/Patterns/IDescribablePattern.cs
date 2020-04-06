namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern which has a description.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <remarks>
    /// Description is not terribly important for patterns, but can be useful for debugging.
    /// </remarks>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="IConditionalPattern{TInput, TMatchResult, TPattern}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    public interface IDescribablePattern<in TInput, TMatchResult> : IPattern<TInput, TMatchResult>
    {
        /// <summary>
        /// Gets the description of this pattern.
        /// </summary>
        string Description { get; }
    }
}
