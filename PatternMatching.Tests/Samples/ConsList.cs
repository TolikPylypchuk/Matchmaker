namespace PatternMatching.Tests.Samples
{
    public abstract class ConsList
    {
        private protected ConsList() { }

        public static ConsList Cell(int head, ConsList tail)
            => new ConsCell(head, tail);

        public static ConsList Empty
            => new Empty();
    }
}
