using System;
using LanguageExt;

using static LanguageExt.Prelude;

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
		/// This list contains value tuples which contain the pattern, the fallthrough behaviour,
		/// and the action which is to be executed if the pattern is matched successfully.
		/// </remarks>
		private readonly Lst<(dynamic, bool, dynamic)> patterns;

		/// <summary>
		/// The default fallthrough behaviour.
		/// </summary>
		private readonly bool fallthroughByDefault;

		/// <summary>
		/// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class.
		/// </summary>
		/// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
		internal Match(bool fallthroughByDefault)
			=> this.fallthroughByDefault = fallthroughByDefault;

		/// <summary>
		/// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class
		/// with the specified patterns.
		/// </summary>
		/// <param name="patterns">The patterns of this expression.</param>
		/// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
		private Match(Lst<(dynamic, bool, dynamic)> patterns, bool fallthroughByDefault)
			=> (this.patterns, this.fallthroughByDefault) = (patterns, fallthroughByDefault);

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
			=> this.Case(pattern, fallthroughByDefault, func);

		/// <summary>
		/// Returns a new matcher which includes the specified pattern and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
		/// <param name="pattern">The pattern to match with.</param>
		/// <param name="fallthrough">The fallthrough behaviour.</param>
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
			bool fallthrough,
			Func<TMatchResult, TOutput> func)
			=> pattern != null
				? func != null
					? new Match<TInput, TOutput>(this.patterns.Add((pattern, fallthrough, func)), this.fallthroughByDefault)
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
			=> this.Case(this.fallthroughByDefault, func);

		/// <summary>
		/// Returns a new matcher which includes the pattern for the specified type and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
		/// <param name="fallthrough">The fallthrough behaviour.</param>
		/// <param name="func">The function to execute if the match is successful.</param>
		/// <returns>
		/// A new matcher which includes the type pattern and function to execute if this
		/// pattern is matched successfully.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="func" /> is <see langword="null" />.
		/// </exception>
		public Match<TInput, TOutput> Case<TType>(bool fallthrough, Func<TType, TOutput> func)
			where TType : TInput
			=> this.Case(Pattern.Type<TInput, TType>(), fallthrough, func);

		/// <summary>
		/// Executes the match expression on the specified input and returns the result.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>The result of the match expression.</returns>
		/// <exception cref="MatchException">
		/// The match failed for all cases.
		/// </exception>
		public TOutput ExecuteOn(TInput input)
			=> this.ExecuteNonStrict(input).IfNoneUnsafe(() => throw new MatchException($"Cannot match {input}."));

		/// <summary>
		/// Executes the match expression on the specified input and returns the result.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>The result of the match expression, or nothing if no pattern was matched successfully.</returns>
		public OptionUnsafe<TOutput> ExecuteNonStrict(TInput input)
		{
			foreach ((dynamic pattern, _, dynamic function) in this.patterns)
			{
				var matchResult = pattern.Match(input);
				if (matchResult.IsSome)
				{
					return SomeUnsafe(function(matchResult.ToList()[0]));
				}
			}

			return None;
		}

		/// <summary>
		/// Executes the match expression on the specified input with fallthrough and returns a list of results.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>The list of results of the match expression.</returns>
		/// <exception cref="MatchException">
		/// The match failed for all cases.
		/// </exception>
		public Lst<TOutput> ExecuteWithFallthrough(TInput input)
		{
			Lst<TOutput> results = this.ExecuteNonStrictWithFallthrough(input);

			if (results.Count == 0)
			{
				throw new MatchException($"Cannot match {input}.");
			}

			return results;
		}

		/// <summary>
		/// Executes the match expression on the specified input with fallthrough and returns a list of results.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>
		/// The list of results of the match expression, which is empty if no pattern is matched successfully.
		/// </returns>
		public Lst<TOutput> ExecuteNonStrictWithFallthrough(TInput input)
		{
			Lst<TOutput> results;
			foreach ((dynamic pattern, bool fallthrough, dynamic function) in this.patterns)
			{
				var matchResult = pattern.Match(input);
				if (matchResult.IsSome)
				{
					results = results.Add(function(matchResult.ToList()[0]));
					if (!fallthrough)
					{
						break;
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Returns a function which, when called, will match the specified value.
		/// </summary>
		/// <returns>A function which, when called, will match the specified value.</returns>
		public Func<TInput, TOutput> ToFunction()
			=> this.ExecuteOn;

		/// <summary>
		/// Returns a function which, when called, will match the specified value.
		/// </summary>
		/// <returns>A function which, when called, will match the specified value.</returns>
		public Func<TInput, OptionUnsafe<TOutput>> ToNonStrictFunction()
			=> this.ExecuteNonStrict;

		/// <summary>
		/// Returns a function which, when called, will match the specified value.
		/// </summary>
		/// <returns>A function which, when called, will match the specified value.</returns>
		public Func<TInput, Lst<TOutput>> ToFunctionWithFallthrough()
			=> this.ExecuteWithFallthrough;

		/// <summary>
		/// Returns a function which, when called, will match the specified value.
		/// </summary>
		/// <returns>A function which, when called, will match the specified value.</returns>
		public Func<TInput, Lst<TOutput>> ToNonStrictFunctionWithFallthrough()
			=> this.ExecuteNonStrictWithFallthrough;
	}
}
