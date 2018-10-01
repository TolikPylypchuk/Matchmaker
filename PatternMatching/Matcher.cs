﻿using System;

using LanguageExt;

namespace PatternMatching
{
	/// <summary>
	/// Represents a match expression.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <typeparam name="TOutput">The type of the output value of the expression.</typeparam>
	/// <seealso cref="Matcher{TInput}" />
	public sealed class Matcher<TInput, TOutput>
	{
		/// <summary>
		/// The list of patterns that will be matched in this expression.
		/// </summary>
		/// <remarks>
		/// This list has dynamic items in it because the matcher knows nothing about transformations
		/// of the patterns. If it did, then the information about each type of the pattern transformation
		/// would be required, and that would render the matcher either unusable, because of the many types
		/// which will have to be specified, or impossible, because there would always be a finite amount
		/// of matcher types (each with information about one more match result type than the previous).
		/// 
		/// This list contains value tuples which contain the pattern and the function which is to be executed
		/// if the pattern is matched successfully.
		///
		/// The type safety is not compromised this way, because the match result type is needed only between
		/// the execution of the pattern match and the execution of the function, and is not visible to the
		/// outside world.
		///
		/// This incurs a performance overhead, but it must be compromised in order for this to work.
		/// </remarks>
		private readonly Lst<dynamic> patterns;

		/// <summary>
		/// Initializes a new instance of the <see cref="Matcher{TInput, TOutput}" /> class.
		/// </summary>
		internal Matcher() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Matcher{TInput, TOutput}" /> class
		/// with the specified patterns.
		/// </summary>
		/// <param name="patterns">The patterns of this expression.</param>
		private Matcher(Lst<dynamic> patterns)
			=> this.patterns = patterns;

		/// <summary>
		/// Returns a new matcher which includes the specified pattern and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
		/// <param name="pattern">The pattern to match with.</param>
		/// <param name="func">The function to execute if the match is successful.</param>
		/// <returns></returns>
		public Matcher<TInput, TOutput> Case<TMatchResult>(
			IPattern<TInput, TMatchResult> pattern,
			Func<TMatchResult, TOutput> func)
			=> new Matcher<TInput, TOutput>(this.patterns.Add((pattern, func)));

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
	}

	/// <summary>
	/// Represents a match expression, which doesn't yield a value.
	/// </summary>
	/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
	/// <remarks>A match expression which doesn't yield a value is also known as a match statement.</remarks>
	/// <seealso cref="Matcher{TInput, TOutput}" />
	public sealed class Matcher<TInput>
	{
		/// <summary>
		/// The list of patterns that will be matched in this expression.
		/// </summary>
		/// <remarks>
		/// This list has dynamic items in it because the matcher knows nothing about transformations
		/// of the patterns. If it did, then the information about each type of the pattern transformation
		/// would be required, and that would render the matcher either unusable, because of the many types
		/// which will have to be specified, or impossible, because there would always be a finite amount
		/// of matcher types (each with information about one more match result type than the previous).
		/// 
		/// This list contains value tuples which contain the pattern and the function which is to be executed
		/// if the pattern is matched successfully.
		///
		/// The type safety is not compromised this way, because the match result type is needed only between
		/// the execution of the pattern match and the execution of the function, and is not visible to the
		/// outside world.
		///
		/// This incurs a performance overhead, but it must be compromised in order for this to work.
		/// </remarks>
		private readonly Lst<dynamic> patterns;

		/// <summary>
		/// Initializes a new instance of the <see cref="Matcher{TInput}" /> class.
		/// </summary>
		internal Matcher() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Matcher{TInput}" /> class
		/// with the specified patterns.
		/// </summary>
		/// <param name="patterns">The patterns of this expression.</param>
		private Matcher(Lst<dynamic> patterns)
			=> this.patterns = patterns;

		/// <summary>
		/// Returns a new matcher which includes the specified pattern and function to execute if this
		/// pattern is matched successfully.
		/// </summary>
		/// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
		/// <param name="pattern">The pattern to match with.</param>
		/// <param name="func">The function to execute if the match is successful.</param>
		/// <returns></returns>
		public Matcher<TInput> Case<TMatchResult>(IPattern<TInput, TMatchResult> pattern, Action<TMatchResult> func)
			=> new Matcher<TInput>(this.patterns.Add((pattern, func)));

		/// <summary>
		/// Executes the match expression on the specified input.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <returns>
		/// <see langword="true" />, if the match was successful.
		/// Otherwise, <see langword="false" />.
		/// </returns>
		/// <remarks>
		/// This method uses some non-conventional manipulation of the option type.
		/// It is used because the conventional approach uses lambda expressions,
		/// and those are forbidden to use with dynamic types.
		/// </remarks>
		/// <seealso cref="ExecuteOnStrict(TInput)" />
		public bool ExecuteOn(TInput input)
		{
			foreach (var pattern in this.patterns)
			{
				var matchResult = pattern.Item1.Match(input);
				if (matchResult.IsSome)
				{
					pattern.Item2(matchResult.ToList()[0]);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Executes the match expression strictly on the specified input.
		/// </summary>
		/// <param name="input">The input value of the expression.</param>
		/// <exception cref="MatchException">
		/// The match failed for all cases.
		/// </exception>
		/// <seealso cref="ExecuteOn(TInput)" />
		public void ExecuteOnStrict(TInput input)
		{
			bool isMatched = this.ExecuteOn(input);

			if (!isMatched)
			{
				throw new MatchException($"Cannot match {input}.");
			}
		}
	}
}