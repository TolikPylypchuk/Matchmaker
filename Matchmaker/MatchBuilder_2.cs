using System;
using System.Collections.Generic;

using Matchmaker.Linq;
using Matchmaker.Patterns;

namespace Matchmaker
{
    /// <summary>
    /// Represents a match expression builder.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TOutput">The type of the output value of the expression.</typeparam>
    /// <seealso cref="Match{TInput, TOutput}" />
    public class MatchBuilder<TInput, TOutput>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchBuilder{TInput, TOutput}" /> class.
        /// </summary>
        internal MatchBuilder()
        {
            this.Cases = new List<Match<TInput, TOutput>.CaseData>();
            this.FallthroughByDefault = false;
        }

        /// <summary>
        /// Gets the collection of cases that will be matched in this expression.
        /// </summary>
        internal ICollection<Match<TInput, TOutput>.CaseData> Cases { get; }

        /// <summary>
        /// Gets the default fallthrough behaviour.
        /// </summary>
        internal bool FallthroughByDefault { get; private set; }

        /// <summary>
        /// Sets the default fallthrough behavior for the match expression.
        /// </summary>
        /// <param name="fallthrough">The default fallthrough behavior.</param>
        /// <returns>The calling builder.</returns>
        public MatchBuilder<TInput, TOutput> Fallthrough(bool fallthrough)
        {
            this.FallthroughByDefault = fallthrough;
            return this;
        }

        /// <summary>
        /// Add the specified case to the match expression.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput, TOutput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, TOutput> func)
            => this.Case(pattern, this.FallthroughByDefault, func);

        /// <summary>
        /// Add the specified case to the match expression.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="fallthrough">The fallthrough behavior.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput, TOutput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Func<TMatchResult, TOutput> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            this.Cases.Add(new Match<TInput, TOutput>.CaseData(
                pattern.Select(result => (object?)result),
                fallthrough,
                value => func((TMatchResult)value!)));

            return this;
        }

        /// <summary>
        /// Add the specified case to the match expression.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// builder.Case(Pattern.Type&lt;TInput, TType&gt;(), func)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput, TOutput> Case<TType>(Func<TType, TOutput> func)
            where TType : TInput
            => this.Case(this.FallthroughByDefault, func);

        /// <summary>
        /// Add the specified case to the match expression.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="fallthrough">The fallthrough behavior.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>The calling builder.</returns>
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// builder.Case(Pattern.Type&lt;TInput, TType&gt;(), fallthrough, func)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public MatchBuilder<TInput, TOutput> Case<TType>(bool fallthrough, Func<TType, TOutput> func)
            where TType : TInput
            => this.Case(Pattern.Type<TInput, TType>(), fallthrough, func);

        /// <summary>
        /// Constructs a match expression from this builder.
        /// </summary>
        /// <returns></returns>
        internal Match<TInput, TOutput> Build()
            => new Match<TInput, TOutput>(this);
    }
}
