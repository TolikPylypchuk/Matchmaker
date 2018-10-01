using System;

using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	/// <summary>
	/// Represents a general non-transforming pattern.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	public sealed class SimplePattern<TInput> : ConditionalPattern<TInput, TInput, SimplePattern<TInput>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
		/// with the specified condition.
		/// </summary>
		/// <param name="predicate">The condition of this pattern.</param>
		public SimplePattern(Func<TInput, bool> predicate)
			: base(List(predicate)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
		/// with the specified conditions.
		/// </summary>
		/// <param name="predicates">The conditions of this pattern.</param>
		private SimplePattern(Lst<Func<TInput, bool>> predicates)
			: base(predicates) { }

		/// <summary>
		/// Matches the input with this pattern, and returns the input value.
		/// </summary>
		/// <param name="input">The input value to match.</param>
		/// <returns>
		/// A non-empty optional value, which contains the input value,
		/// if this match is successful. Otherwise, an empty optional.
		/// </returns>
		public override Option<TInput> Match(TInput input)
			=> Some(input).Filter(result => this.Predicates.ForAll(predicate => predicate(result)));

		/// <summary>
		/// Returns a new pattern, which includes the specified condition.
		/// </summary>
		/// <param name="predicate">The condition to add.</param>
		/// <returns>A new pattern, which includes the specified condition.</returns>
		public override SimplePattern<TInput> When(Func<TInput, bool> predicate)
			=> new SimplePattern<TInput>(this.Predicates.Add(predicate));

		/// <summary>
		/// Returns a pattern, which is matched successfully only if both
		/// this and other pattern are matched successfully.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully only if both
		/// this and other pattern are matched successfully.
		/// </returns>
		public SimplePattern<TInput> And(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(this.Predicates.Append(other.Predicates));

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// this or other pattern is matched successfully.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// this or other pattern is matched successfully.
		/// </returns>
		public SimplePattern<TInput> Or(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome || other.Match(input).IsSome);

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// matches with this and other patterns yield different results.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// matches with this and other patterns yield different results.
		/// </returns>
		public SimplePattern<TInput> Xor(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome ^ other.Match(input).IsSome);
	}
}
