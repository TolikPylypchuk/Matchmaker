namespace PatternMatching.Tests.Samples
{
	public sealed class Empty : ConsList
	{
		public static readonly Pattern<ConsList, Empty> Pattern = PatternMatching.Pattern.Type<ConsList, Empty>();

		internal Empty() { }
	}
}
