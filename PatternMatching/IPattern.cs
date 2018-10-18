using LanguageExt;

namespace PatternMatching
{
	/// <summary>
	/// Represents a pattern to match with in a match expression.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
	/// <seealso cref="IPattern{TInput, TMatchResult}" />
	/// <seealso cref="Pattern{TInput, TMatchResult}" />
	/// <seealso cref="SimplePattern{TInput}" />
	/// <seealso cref="Pattern" />
	public interface IPattern<in TInput, TMatchResult>
	{
		/// <summary>
		/// Matches the input with this pattern, and returns a transformed result.
		/// </summary>
		/// <param name="input">The input value to match.</param>
		/// <returns>
		/// A non-empty optional value, which contains the transformed result of the match,
		/// if this match is successful. Otherwise, an empty optional.
		/// </returns>
		Option<TMatchResult> Match(TInput input);
	}
}
