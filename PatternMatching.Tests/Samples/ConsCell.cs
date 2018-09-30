namespace PatternMatching.Tests.Samples
{
	public sealed class ConsCell : ConsList
	{
		public static readonly Pattern<ConsList, ConsCell> Pattern = PatternMatching.Pattern.Type<ConsList, ConsCell>();

		public int Head { get; }
		public ConsList Tail { get; }

		internal ConsCell(int head, ConsList tail)
		{
			this.Head = head;
			this.Tail = tail;
		}
	}
}
