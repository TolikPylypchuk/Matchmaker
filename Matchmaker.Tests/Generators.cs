using System;

using FsCheck;

using static Matchmaker.Pattern;

namespace Matchmaker
{
    public static class Generators
    {
        public static Arbitrary<SimplePattern<string>> SimplePattern()
            => new ArbitrarySimplePattern();

        public static Arbitrary<Func<string, bool>> Predicate()
            => new ArbitraryPredicate();

        public static Arbitrary<Func<string, MatchResult<string>>> Matcher()
            => new ArbitraryMatcher();

        private class ArbitrarySimplePattern : Arbitrary<SimplePattern<string>>
        {
            public override Gen<SimplePattern<string>> Generator
                => from input in Arb.Default.String().Generator
                    from item in Gen.Elements(
                        EqualTo(input),
                        EqualTo(() => input),
                        LessThan(input),
                        LessThan(() => input),
                        LessOrEqual(input),
                        LessOrEqual(() => input),
                        GreaterThan(input),
                        GreaterThan(() => input),
                        GreaterOrEqual(() => input),
                        GreaterOrEqual(() => input),
                        Any<string>())
                   select item;
        }

        private class ArbitraryPredicate : Arbitrary<Func<string, bool>>
        {
            public override Gen<Func<string, bool>> Generator
                => Gen.Elements<Func<string, bool>>(
                        str => str == null,
                        String.IsNullOrEmpty,
                        str => str == "abc",
                        str => str != null && str == str.ToLower());
        }

        private class ArbitraryMatcher : Arbitrary<Func<string, MatchResult<string>>>
        {
            public override Gen<Func<string, MatchResult<string>>> Generator
                => Gen.Elements(
                    this.ResultFromPredicate(str => str == null),
                    this.ResultFromPredicate(String.IsNullOrEmpty),
                    this.ResultFromPredicate(str => str == "abc"),
                    this.ResultFromPredicate(str => str != null && str == str.ToLower()));

            private Func<string, MatchResult<string>> ResultFromPredicate(Func<string, bool> predicate)
                => str => predicate(str) ? MatchResult.Success(str) : MatchResult.Failure<string>();
        }
    }
}
