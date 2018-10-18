using System;
using LanguageExt;

namespace PatternMatching
{
	/// <summary>
	/// Represents a match expression.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TOutput">The type of the output value of the expression.</typeparam>
	/// <seealso cref="Match{TInput}" />
	/// <seealso cref="Match" />
	/// <seealso cref="MatchException" />
	public sealed class Match<TInput, TOutput>
	{
		/// <summary>
		/// The list of patterns that will be matched in this expression.
		/// </summary>
		/// <remarks>
		/// This list contains value tuples which contain the pattern and the function which is to be executed
		/// if the pattern is matched successfully.
		/// </remarks>
		private readonly Lst<dynamic> patterns;

		/// <summary>
		/// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class.
		/// </summary>
		internal Match() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class
		/// with the specified patterns.
		/// </summary>
		/// <param name="patterns">The patterns of this expression.</param>
		private Match(Lst<dynamic> patterns)
			=> this.patterns = patterns;

		/// <summary>
		/// Returns a new matcher which includes the specified pattern and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
		/// <param name="pattern">The pattern to match with.</param>
		/// <param name="func">The function to execute if the match is successful.</param>
		/// <returns>
		/// A new matcher which includes the specified pattern and function to execute if this
		/// pattern is matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pattern" /> or <paramref name="func" /> is <see langword="null" />.
		/// </exception>
		public Match<TInput, TOutput> Case<TMatchResult>(
			IPattern<TInput, TMatchResult> pattern,
			Func<TMatchResult, TOutput> func)
			=> pattern != null
				? func != null
					? new Match<TInput, TOutput>(this.patterns.Add((pattern, func)))
					: throw new ArgumentNullException(nameof(func))
				: throw new ArgumentNullException(nameof(pattern));

		/// <summary>
		/// Returns a new matcher which includes the pattern for the specified type and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
		/// <param name="func">The function to execute if the match is successful.</param>
		/// <returns>
		/// A new matcher which includes the type pattern and function to execute if this
		/// pattern is matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="func" /> is <see langword="null" />.
		/// </exception>
		public Match<TInput, TOutput> Case<TType>(Func<TType, TOutput> func)
			where TType : TInput
			=> this.Case(Pattern.Type<TInput, TType>(), func);

		/// <summary>
		/// Executes the match expression on the specified input and returns the result.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>The result of the match expression.</returns>
		/// <exception cref="MatchException">
		/// The match failed for all cases.
		/// </exception>
		/// <remarks>
		/// This method uses some non-conventional manipulation of the option type.
		/// It is used because the conventional approach uses lambda expressions,
		/// and those are forbidden to use with dynamic types.
		/// </remarks>
		public TOutput ExecuteOn(TInput input)
		{
			foreach (var pattern in this.patterns)
			{
				var matchResult = pattern.Item1.Match(input);
				if (matchResult.IsSome)
				{
					return pattern.Item2(matchResult.ToList()[0]);
				}
			}

			throw new MatchException($"Cannot match {input}.");
		}

		/// <summary>
		/// Compiles this matcher into a function which, when called, will match the specified value.
		/// </summary>
		/// <returns>A function which, when called, will match the specified value.</returns>
		public Func<TInput, TOutput> ToFunction()
			=> this.ExecuteOn;
	}
}
