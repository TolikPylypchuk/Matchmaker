namespace PatternMatching.Tests.Samples
{
	public sealed class ConsCell : ConsList
	{
		public int Head { get; }
		public ConsList Tail { get; }

		internal ConsCell(int head, ConsList tail)
		{
			this.Head = head;
			this.Tail = tail;
		}
	}
}
