using System;
using LanguageExt;

namespace PatternMatching
{
	/// <summary>
	/// Represents a general transforming pattern.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
	/// <seealso cref="IPattern{TInput, TMatchResult}" />
	/// <seealso cref="ConditionalPattern{TInput, TMatchResult, TPattern}" />
	/// <seealso cref="SimplePattern{TInput}" />
	/// <seealso cref="Pattern" />
	public sealed class Pattern<TInput, TMatchResult> : ConditionalPattern<TInput, TMatchResult, Pattern<TInput, TMatchResult>>
	{
		/// <summary>
		/// The matcher function.
		/// </summary>
		private readonly Func<TInput, Option<TMatchResult>> matcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class
		/// with the specified matcher function.
		/// </summary>
		/// <param name="matcher">The matcher function.</param>
		public Pattern(Func<TInput, Option<TMatchResult>> matcher)
			=> this.matcher = matcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pattern{TInput, TMatchResult}" /> class
		/// with the specified matcher function and additional conditions.
		/// </summary>
		/// <param name="matcher">The matcher function.</param>
		/// <param name="predicates">The additional conditions.</param>
		private Pattern(Func<TInput, Option<TMatchResult>> matcher, Lst<Func<TMatchResult, bool>> predicates)
			: base(predicates)
			=> this.matcher = matcher;

		/// <summary>
		/// Matches the input with this pattern, and returns a transformed result.
		/// </summary>
		/// <param name="input">The input value to match.</param>
		/// <returns>
		/// A non-empty optional value, which contains the transformed result of the match,
		/// if this match is successful. Otherwise, an empty optional.
		/// </returns>
		public override Option<TMatchResult> Match(TInput input)
			=> this.matcher(input).Filter(result => this.Predicates.ForAll(predicate => predicate(result)));

		/// <summary>
		/// Returns a new pattern, which includes the specified condition.
		/// </summary>
		/// <param name="predicate">The condition to add.</param>
		/// <returns>A new pattern, which includes the specified condition.</returns>
		public override Pattern<TInput, TMatchResult> When(Func<TMatchResult, bool> predicate)
			=> new Pattern<TInput, TMatchResult>(this.matcher, this.Predicates.Add(predicate));

		/// <summary>
		/// Returns a pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </summary>
		/// <param name="pattern">The pattern to invert.</param>
		/// <returns>
		/// A pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </returns>
		/// <remarks>
		/// This pattern ignores the specified pattern's transformation
		/// and returns the input value if matched successfully.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public static SimplePattern<TInput> operator ~(Pattern<TInput, TMatchResult> pattern)
			=> Pattern.Not(pattern);
	}
}
