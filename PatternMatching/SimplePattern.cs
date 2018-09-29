using System;

using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	public sealed class SimplePattern<TInput> : IPattern<TInput, TInput>
	{
		private readonly Lst<Func<TInput, bool>> predicates;

		public SimplePattern(Func<TInput, bool> predicate)
		{
			this.predicates = List(predicate);
		}

		private SimplePattern(Lst<Func<TInput, bool>> predicates)
		{
			this.predicates = predicates;
		}

		public Option<TInput> Match(TInput input)
			=> Some(input).Filter(result => this.predicates.ForAll(predicate => predicate(result)));

		public SimplePattern<TInput> When(Func<TInput, bool> predicate)
			=> new SimplePattern<TInput>(this.predicates.Add(predicate));

		public SimplePattern<TInput> And(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(this.predicates.Append(other.predicates));

		public SimplePattern<TInput> Or(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome || other.Match(input).IsSome);

		public SimplePattern<TInput> Xor(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome ^ other.Match(input).IsSome);
	}
}
