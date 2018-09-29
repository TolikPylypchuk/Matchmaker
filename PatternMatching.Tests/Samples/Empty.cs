namespace PatternMatching.Tests.Samples
{
	public sealed class Empty : ConsList
	{
		public static readonly Pattern<ConsList, Empty> Pattern = Patterns.Type<ConsList, Empty>();

		internal Empty() { }
	}
}
