namespace Matchmaker.Patterns.Async;

using System;
using System.Threading.Tasks;

/// <summary>
/// Represents a pattern to match with in an asynchronous match expression.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the match expression.</typeparam>
/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
/// <seealso cref="AsyncPattern{TInput, TMatchResult}" />
/// <seealso cref="AsyncPattern" />
public interface IAsyncPattern<in TInput, TMatchResult>
{
    /// <summary>
    /// Gets the description of this pattern.
    /// </summary>
    /// <remarks>
    /// Description is not terribly important, but can be used for debugging.
    /// The description may never be <see langword="null" />. An empty description
    /// implies that a pattern doesn't have a description.
    /// </remarks>
    string Description { get; }

    /// <summary>
    /// Matches the input with this pattern, and returns a transformed result asynchronously.
    /// </summary>
    /// <param name="input">The input value to match.</param>
    /// <returns>
    /// A successful match result which contains the transformed result of the match,
    /// if this match is successful. Otherwise, a failed match result.
    /// </returns>
    /// <remarks>
    /// Implementations of this method may throw <see cref="InvalidOperationException" />
    /// if they combine instances of <see cref="Task" /> or <see cref="Task{TResult}" /> and one
    /// of those instances is <see langword="null" />.
    /// </remarks>
    Task<MatchResult<TMatchResult>> MatchAsync(TInput input);
}
