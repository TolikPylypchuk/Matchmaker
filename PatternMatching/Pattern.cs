using System;
using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	/// <summary>
	/// Represents a general transforming pattern.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
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
			=> pattern != null
				? Pattern.Not(pattern)
				: throw new ArgumentNullException(nameof(pattern));
	}

	/// <summary>
	/// Contains some frequently used patterns.
	/// </summary>
	public static class Pattern
	{
		/// <summary>
		/// Returns a pattern which is always matched successfully.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <returns>A pattern which is always matched successfully.</returns>
		/// <remarks>
		/// This pattern should be used as the default case of the match expression, if one is needed.
		/// </remarks>
		public static SimplePattern<TInput> Any<TInput>()
			=> new SimplePattern<TInput>(_ => true);

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is equal to the specified value.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <param name="value">The value to check for equality.</param>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is equal to the specified value.
		/// </returns>
		public static SimplePattern<TInput> EqualTo<TInput>(TInput value)
			where TInput : IEquatable<TInput>
			=> new SimplePattern<TInput>(input => input.Equals(value));

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is less than the specified value.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <param name="value">The value to compare with.</param>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is less than the specified value.
		/// </returns>
		public static SimplePattern<TInput> LessThan<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) < 0);

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is less than or equal to the specified value.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <param name="value">The value to compare with.</param>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is less than or equal to the specified value.
		/// </returns>
		public static SimplePattern<TInput> LessOrEqual<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) <= 0);

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is greater than the specified value.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <param name="value">The value to compare with.</param>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is greater than the specified value.
		/// </returns>
		public static SimplePattern<TInput> GreaterThan<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) > 0);

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is greater than or equal to the specified value.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <param name="value">The value to compare with.</param>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is greater than or equal to the specified value.
		/// </returns>
		public static SimplePattern<TInput> GreaterOrEqual<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) >= 0);

		/// <summary>
		/// Returns a pattern which is matched successfully when the input value
		/// is of the specified type.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <typeparam name="TType">The type to check for.</typeparam>
		/// <returns>
		/// A pattern which is matched successfully when the input value
		/// is of the specified type.
		/// </returns>
		/// <remarks>
		/// This pattern should be used to match discriminated unions, which are implemented
		/// as class hierarchies.
		/// </remarks>
		public static Pattern<TInput, TType> Type<TInput, TType>()
			where TType : TInput
			=> new Pattern<TInput, TType>(input => input is TType result ? Some(result) : None);

		/// <summary>
		/// Returns a pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
		/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
		/// <param name="pattern">The pattern to invert.</param>
		/// <returns>
		/// A pattern which is matched successfully
		/// when the specified pattern is not matched successfully.
		/// </returns>
		/// <remarks>
		/// This pattern ignores the specified pattern's transformation
		/// and returns the input value if matched successfully.
		/// </remarks>
		public static SimplePattern<TInput> Not<TInput, TMatchResult>(IPattern<TInput, TMatchResult> pattern)
			=> new SimplePattern<TInput>(input => !pattern.Match(input).IsSome);
	}
}
