using System;

using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	public sealed class SimplePattern<TInput> : ConditionalPattern<TInput, TInput, SimplePattern<TInput>>
	{
		public SimplePattern(Func<TInput, bool> predicate)
			: base(List(predicate)) { }

		private SimplePattern(Lst<Func<TInput, bool>> predicates)
				: base(predicates) { }

		public override Option<TInput> Match(TInput input)
			=> Some(input).Filter(result => this.Predicates.ForAll(predicate => predicate(result)));

		public override SimplePattern<TInput> When(Func<TInput, bool> predicate)
			=> new SimplePattern<TInput>(this.Predicates.Add(predicate));

		public SimplePattern<TInput> And(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(this.Predicates.Append(other.Predicates));

		public SimplePattern<TInput> Or(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome || other.Match(input).IsSome);

		public SimplePattern<TInput> Xor(SimplePattern<TInput> other)
			=> new SimplePattern<TInput>(input => this.Match(input).IsSome ^ other.Match(input).IsSome);
	}
}
