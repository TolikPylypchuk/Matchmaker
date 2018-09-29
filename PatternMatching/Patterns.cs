using System;

using static LanguageExt.Prelude;

namespace PatternMatching
{
	public static class Patterns
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
