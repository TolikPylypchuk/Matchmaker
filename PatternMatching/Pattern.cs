using System;

using LanguageExt;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	public sealed class Pattern<TInput, TMatchResult> : ConditionalPattern<TInput, TMatchResult, Pattern<TInput, TMatchResult>>
	{
		private readonly Func<TInput, Option<TMatchResult>> matcher;

		public Pattern(Func<TInput, Option<TMatchResult>> matcher)
			=> this.matcher = matcher;

		private Pattern(Func<TInput, Option<TMatchResult>> matcher, Lst<Func<TMatchResult, bool>> predicates)
			: base(predicates)
			=> this.matcher = matcher;

		public override Option<TMatchResult> Match(TInput input)
			=> this.matcher(input).Filter(result => this.Predicates.ForAll(predicate => predicate(result)));

		public override Pattern<TInput, TMatchResult> When(Func<TMatchResult, bool> predicate)
			=> new Pattern<TInput, TMatchResult>(this.matcher, this.Predicates.Add(predicate));
	}

	public static class Pattern
	{
		public static SimplePattern<TInput> Any<TInput>()
			=> new SimplePattern<TInput>(_ => true);

		public static SimplePattern<TInput> EqualTo<TInput>(TInput value)
			where TInput : IEquatable<TInput>
			=> new SimplePattern<TInput>(input => input.Equals(value));

		public static SimplePattern<TInput> LessThan<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) < 0);

		public static SimplePattern<TInput> LessOrEqual<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) <= 0);

		public static SimplePattern<TInput> GreaterThan<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) > 0);

		public static SimplePattern<TInput> GreaterOrEqual<TInput>(TInput value)
			where TInput : IComparable<TInput>
			=> new SimplePattern<TInput>(input => input.CompareTo(value) >= 0);

		public static Pattern<TInput, TType> Type<TInput, TType>()
			where TType : TInput
			=> new Pattern<TInput, TType>(input => input is TType result ? Some(result) : None);

		public static SimplePattern<TInput> Not<TInput, TMatchResult>(IPattern<TInput, TMatchResult> pattern)
			=> new SimplePattern<TInput>(input => !pattern.Match(input).IsSome);
	}
}
