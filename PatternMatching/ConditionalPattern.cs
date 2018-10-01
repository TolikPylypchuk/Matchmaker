using System;

using LanguageExt;

namespace PatternMatching
{
	/// <summary>
	/// Represents a pattern, to which additional conditions may be added.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
	/// <typeparam name="TPattern">The type of the pattern.</typeparam>
	public abstract class ConditionalPattern<TInput, TMatchResult, TPattern> : IPattern<TInput, TMatchResult>
		where TPattern : ConditionalPattern<TInput, TMatchResult, TPattern>
	{
		/// <summary>
		/// A list of predicates, which specify the conditions of this pattern.
		/// </summary>
		protected readonly Lst<Func<TMatchResult, bool>> Predicates;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult, TPattern}" /> class
		/// without any conditions.
		/// </summary>
		protected ConditionalPattern() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionalPattern{TInput, TMatchResult, TPattern}" /> class
		/// with the specified conditions.
		/// </summary>
		/// <param name="predicates">The conditions of this pattern.</param>
		protected ConditionalPattern(Lst<Func<TMatchResult, bool>> predicates)
			=> this.Predicates = predicates;

		/// <summary>
		/// Matches the input with this pattern, and returns a transformed result.
		/// </summary>
		/// <param name="input">The input value to match.</param>
		/// <returns>
		/// A non-empty optional value, which contains the transformed result of the match,
		/// if this match is successful. Otherwise, an empty optional.
		/// </returns>
		public abstract Option<TMatchResult> Match(TInput input);

		/// <summary>
		/// Returns a new pattern, which includes the specified condition.
		/// </summary>
		/// <param name="predicate">The condition to add.</param>
		/// <returns>A new pattern, which includes the specified condition.</returns>
		public abstract TPattern When(Func<TMatchResult, bool> predicate);
	}
}
