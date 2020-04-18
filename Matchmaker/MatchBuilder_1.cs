using System;
using System.Collections.Generic;

using Matchmaker.Patterns;

namespace Matchmaker
{
    /// <summary>
    /// Represents a match statement builder.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <seealso cref="Match{TInput}" />
    public class MatchBuilder<TInput>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchBuilder{TInput}" /> class.
        /// </summary>
        internal MatchBuilder()
        {
            this.Cases = new List<Match<TInput>.CaseData>();
            this.FallthroughByDefault = false;
        }

        /// <summary>
        /// Gets the collection of cases that will be matched in this statement.
        /// </summary>
        internal ICollection<Match<TInput>.CaseData> Cases { get; }

        /// <summary>
        /// Gets the default fallthrough behaviour.
        /// </summary>
        internal bool FallthroughByDefault { get; private set; }

        /// <summary>
        /// Sets the default fallthrough behavior for the match statement.
        /// </summary>
        /// <param name="fallthrough">The default fallthrough behavior.</param>
        /// <returns>The calling builder.</returns>
        public MatchBuilder<TInput> Fallthrough(bool fallthrough)
        {
            this.FallthroughByDefault = fallthrough;
            return this;
        }

        /// <summary>
        /// Add the specified case to the match statement.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            Action<TMatchResult> action)
            => this.Case(pattern, this.FallthroughByDefault, action);

        /// <summary>
        /// Add the specified case to the match statement.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="fallthrough">The fallthrough behavior.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Action<TMatchResult> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.Cases.Add(new Match<TInput>.CaseData(
                pattern ?? throw new ArgumentNullException(nameof(pattern)),
                fallthrough,
                value => action((TMatchResult)value)));

            return this;
        }

        /// <summary>
        /// Add the specified case to the match statement.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// builder.Case(Pattern.Type&lt;TInput, TType&gt;(), action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput> Case<TType>(Action<TType> action)
            where TType : TInput
            => this.Case(this.FallthroughByDefault, action);

        /// <summary>
        /// Add the specified case to the match statement.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="fallthrough">The fallthrough behavior.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// builder.Case(Pattern.Type&lt;TInput, TType&gt;(), fallthrough, action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput> Case<TType>(bool fallthrough, Action<TType> action)
            where TType : TInput
            => this.Case(Pattern.Type<TInput, TType>(), fallthrough, action);

        /// <summary>
        /// Constructs a match statement from this builder.
        /// </summary>
        /// <returns></returns>
        internal Match<TInput> Build()
            => new Match<TInput>(this);
    }
}
