using System;
using System.Collections.Immutable;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents an abstract pattern with conditions and a description.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <typeparam name="TPattern">The actual type of the pattern.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="IConditionalPattern{TInput, TMatchResult, TPattern}" />
    /// <seealso cref="IDescribablePattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    public abstract class PatternBase<TInput, TMatchResult, TPattern>
        : IConditionalPattern<TInput, TMatchResult, TPattern>, IDescribablePattern<TInput, TMatchResult>
        where TPattern : PatternBase<TInput, TMatchResult, TPattern>
    {
        /// <summary>
        /// A list of conditions of this pattern.
        /// </summary>
        protected readonly IImmutableList<Func<TMatchResult, bool>> Conditions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternBase{TInput, TMatchResult, TPattern}" /> class
        /// without any conditions.
        /// </summary>
        protected PatternBase()
            : this(ImmutableList<Func<TMatchResult, bool>>.Empty, String.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternBase{TInput, TMatchResult, TPattern}" /> class
        /// with the specified conditions.
        /// </summary>
        /// <param name="conditions">The conditions of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="conditions" /> is <see langword="null" />.
        /// </exception>
        protected PatternBase(IImmutableList<Func<TMatchResult, bool>> conditions)
            : this(conditions, String.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternBase{TInput, TMatchResult, TPattern}" /> class
        /// with the specified description.
        /// </summary>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        protected PatternBase(string description)
            : this(ImmutableList<Func<TMatchResult, bool>>.Empty, description)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternBase{TInput, TMatchResult, TPattern}" /> class
        /// with the specified conditions and description.
        /// </summary>
        /// <param name="conditions">The conditions of this pattern.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="conditions" /> of <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        protected PatternBase(IImmutableList<Func<TMatchResult, bool>> conditions, string description)
        {
            this.Conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Gets the description of this pattern.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public abstract MatchResult<TMatchResult> Match(TInput input);

        /// <summary>
        /// Returns a new pattern which includes the specified condition.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        /// <returns>A new pattern which includes the specified condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> is <see langword="null" />.
        /// </exception>
        public abstract TPattern When(Func<TMatchResult, bool> condition);

        /// <summary>
        /// Returns the description of this pattern, if it has one.
        /// </summary>
        /// <returns>
        /// The description of this pattern, if it has one.
        /// If it doesn't, then returns the name of this pattern's type.
        /// </returns>
        public override string ToString()
            => String.IsNullOrEmpty(this.Description) ? base.ToString() : this.Description;

        /// <summary>
        /// Matches the input with this pattern, and returns a transformed result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        MatchResult<object> IPattern<TInput>.Match(TInput input)
        {
            var result = this.Match(input);
            return result.IsSuccessful
                ? MatchResult.Success<object>(result.Value)
                : MatchResult.Failure<object>();
        }
    }
}
