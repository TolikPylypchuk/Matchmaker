using System;

using LanguageExt;

namespace PatternMatching
{
	public sealed class Matcher<TInput, TOutput>
	{
		private readonly Lst<dynamic> patterns;

		internal Matcher() { }

		private Matcher(Lst<dynamic> patterns)
		{
			this.patterns = patterns;
		}

		public Matcher<TInput, TOutput> Case<TMatchResult>(
			IPattern<TInput, TMatchResult> pattern,
			Func<TMatchResult, TOutput> body)
			=> new Matcher<TInput, TOutput>(this.patterns.Add((pattern, body)));

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

	public sealed class Matcher<TInput>
	{
		private readonly Lst<dynamic> patterns;

		internal Matcher() { }

		private Matcher(Lst<dynamic> patterns)
		{
			this.patterns = patterns;
		}

		public Matcher<TInput> Case<TMatchResult>(IPattern<TInput, TMatchResult> pattern, Action<TMatchResult> body)
			=> new Matcher<TInput>(this.patterns.Add((pattern, body)));

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
