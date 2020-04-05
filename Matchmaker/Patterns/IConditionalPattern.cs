using System;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern, to which additional conditions may be added.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <typeparam name="TPattern">The actual type of the pattern.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="IDescriptivePattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    public interface IConditionalPattern<in TInput, TMatchResult, out TPattern> : IPattern<TInput, TMatchResult>
        where TPattern : IConditionalPattern<TInput, TMatchResult, TPattern>
    {
        /// <summary>
        /// Returns a new pattern which includes the specified condition.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        /// <returns>A new pattern which includes the specified condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> is <see langword="null" />.
        /// </exception>
        TPattern When(Func<TMatchResult, bool> condition);
    }
}
