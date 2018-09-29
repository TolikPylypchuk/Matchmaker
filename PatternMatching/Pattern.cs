using System;

using LanguageExt;

namespace PatternMatching
{
	public sealed class Pattern<TInput, TMatchResult> : IPattern<TInput, TMatchResult>
	{
		private readonly Func<TInput, Option<TMatchResult>> matcher;
		private readonly Lst<Func<TMatchResult, bool>> additionalPredicates;

		public Pattern(Func<TInput, Option<TMatchResult>> matcher)
		{
			this.matcher = matcher;
		}

		private Pattern(Func<TInput, Option<TMatchResult>> matcher, Lst<Func<TMatchResult, bool>> additionalPredicates)
		{
			this.matcher = matcher;
			this.additionalPredicates = additionalPredicates;
		}

		public Option<TMatchResult> Match(TInput input)
			=> this.matcher(input).Filter(result => this.additionalPredicates.ForAll(predicate => predicate(result)));

		public Pattern<TInput, TMatchResult> When(Func<TMatchResult, bool> predicate)
			=> new Pattern<TInput, TMatchResult>(this.matcher, this.additionalPredicates.Add(predicate));
	}
}
