using FsCheck;

using static PatternMatching.Pattern;

using Random = System.Random;

namespace PatternMatching
{
    public static class Generators
    {
        private static readonly Random Random = new Random();

        public static Arbitrary<SimplePattern<string>> SimplePattern()
            => new ArbitrarySimplePattern();

        class ArbitrarySimplePattern : Arbitrary<SimplePattern<string>>
        {
            public override Gen<SimplePattern<string>> Generator
                => from input in Arb.Default.String().Generator
                    select new[]
                    {
                        EqualTo(input), EqualTo(() => input),
                        LessThan(input), LessThan(() => input),
                        LessOrEqual(input), LessOrEqual(() => input),
                        GreaterThan(input), GreaterThan(() => input),
                        GreaterOrEqual(() => input), GreaterOrEqual(() => input),
                        Any<string>()
                    }[Random.Next(11)];
        }
    }
}
