using System;

using LanguageExt;

namespace PatternMatching
{
	public abstract class ConditionalPattern<TInput, TMatchResult, TPattern> : IPattern<TInput, TMatchResult>
		where TPattern : ConditionalPattern<TInput, TMatchResult, TPattern>
	{
		protected readonly Lst<Func<TMatchResult, bool>> Predicates;

		protected ConditionalPattern() { }

		protected ConditionalPattern(Lst<Func<TMatchResult, bool>> predicates)
		{
			this.Predicates = predicates;
		}

		public abstract Option<TMatchResult> Match(TInput input);

		public abstract TPattern When(Func<TMatchResult, bool> predicate);
	}
}
