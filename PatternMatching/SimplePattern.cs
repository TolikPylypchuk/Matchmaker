using System;
using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	/// <summary>
	/// Represents a general non-transforming pattern.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <seealso cref="IPattern{TInput, TMatchResult}" />
	/// <seealso cref="ConditionalPattern{TInput, TMatchResult, TPattern}" />
	/// <seealso cref="Pattern{TInput, TMatchResult}" />
	/// <seealso cref="Pattern" />
	public sealed class SimplePattern<TInput> : ConditionalPattern<TInput, TInput, SimplePattern<TInput>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
		/// with the specified condition.
		/// </summary>
		/// <param name="predicate">The condition of this pattern.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="predicate" /> is <see langword="null" />.
		/// </exception>
		public SimplePattern(Func<TInput, bool> predicate)
			: base(List(predicate ?? throw new ArgumentNullException(nameof(predicate)))) { }

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
		/// <exception cref="ArgumentNullException">
		/// <paramref name="input" /> is <see langword="null" />.
		/// </exception>
		public override Option<TInput> Match(TInput input)
			=> input != null
				? Some(input).Filter(result => this.Predicates.ForAll(predicate => predicate(result)))
				: throw new ArgumentNullException(nameof(input));

		/// <summary>
		/// Returns a new pattern, which includes the specified condition.
		/// </summary>
		/// <param name="predicate">The condition to add.</param>
		/// <returns>A new pattern, which includes the specified condition.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="predicate" /> is <see langword="null" />.
		/// </exception>
		public override SimplePattern<TInput> When(Func<TInput, bool> predicate)
			=> predicate != null
				? new SimplePattern<TInput>(this.Predicates.Add(predicate))
				: throw new ArgumentNullException(nameof(predicate));

		/// <summary>
		/// Returns a pattern, which is matched successfully only if both
		/// this and other pattern are matched successfully.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully only if both
		/// this and other pattern are matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other" /> is <see langword="null" />.
		/// </exception>
		public SimplePattern<TInput> And(SimplePattern<TInput> other)
			=> other != null
				? new SimplePattern<TInput>(this.Predicates.Append(other.Predicates))
				: throw new ArgumentNullException(nameof(other));

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// this or other pattern is matched successfully.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// this or other pattern is matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other" /> is <see langword="null" />.
		/// </exception>
		public SimplePattern<TInput> Or(SimplePattern<TInput> other)
			=> other != null
				? new SimplePattern<TInput>(input => this.Match(input).IsSome || other.Match(input).IsSome)
				: throw new ArgumentNullException(nameof(other));

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// matches with this and other patterns yield different results.
		/// </summary>
		/// <param name="other">The other pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// matches with this and other patterns yield different results.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other" /> is <see langword="null" />.
		/// </exception>
		public SimplePattern<TInput> Xor(SimplePattern<TInput> other)
			=> other != null
				? new SimplePattern<TInput>(input => this.Match(input).IsSome ^ other.Match(input).IsSome)
				: throw new ArgumentNullException(nameof(other));

		/// <summary>
		/// Returns a pattern, which is matched successfully only if both
		/// the first and second patterns are matched successfully.
		/// </summary>
		/// <param name="pattern1">The first pattern.</param>
		/// <param name="pattern2">The second pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully only if both
		/// the first and second patterns are matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
		/// </exception>
		public static SimplePattern<TInput> operator &(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
			=> pattern1 != null
				? pattern2 != null
					? pattern1.And(pattern2)
					: throw new ArgumentNullException(nameof(pattern2))
				: throw new ArgumentNullException(nameof(pattern1));

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// the first or the second pattern is matched successfully.
		/// </summary>
		/// <param name="pattern1">The first pattern.</param>
		/// <param name="pattern2">The second pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// the first or the second pattern is matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
		/// </exception>
		public static SimplePattern<TInput> operator |(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
			=> pattern1 != null
				? pattern2 != null
					? pattern1.Or(pattern2)
					: throw new ArgumentNullException(nameof(pattern2))
				: throw new ArgumentNullException(nameof(pattern1));

		/// <summary>
		/// Returns a pattern, which is matched successfully if
		/// matches with the first and second patterns yield different results.
		/// </summary>
		/// <param name="pattern1">The first pattern.</param>
		/// <param name="pattern2">The second pattern.</param>
		/// <returns>
		/// A pattern, which is matched successfully if
		/// matches with the first and second patterns yield different results.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
		/// </exception>
		public static SimplePattern<TInput> operator ^(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
			=> pattern1 != null
				? pattern2 != null
					? pattern1.Xor(pattern2)
					: throw new ArgumentNullException(nameof(pattern2))
				: throw new ArgumentNullException(nameof(pattern1));

		/// <summary>
		/// Returns a pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </summary>
		/// <param name="pattern">The pattern to invert.</param>
		/// <returns>
		/// A pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public static SimplePattern<TInput> operator ~(SimplePattern<TInput> pattern)
			=> pattern != null
				? Pattern.Not(pattern)
				: throw new ArgumentNullException(nameof(pattern));
	}
}
