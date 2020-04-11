using System;

using FsCheck;

using Matchmaker.Linq;
using Matchmaker.Patterns;

using static Matchmaker.Patterns.Pattern;

namespace Matchmaker
{
    public static class Generators
    {
        public static Arbitrary<IPattern<string, string>> Pattern()
            => new ArbitraryPattern();

        public static Arbitrary<IPattern<string, object>> ObjectPattern()
            => new ArbitraryObjectPattern();

        public static Arbitrary<Func<string, bool>> Predicate()
            => new ArbitraryPredicate();

        public static Arbitrary<Func<string, MatchResult<string>>> Matcher()
            => new ArbitraryMatcher();

        public static Arbitrary<Func<string, int>> Mapper()
            => new ArbitraryMapper();

        public static Arbitrary<MatchResult<string>> Result()
            => new ArbitraryMatchResult();

        private class ArbitraryPattern : Arbitrary<IPattern<string, string>>
        {
            public override Gen<IPattern<string, string>> Generator
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

        private class ArbitraryObjectPattern : Arbitrary<IPattern<string, object>>
        {
            public override Gen<IPattern<string, object>> Generator
                => from input in Arb.Default.String().Generator
                    from item in Gen.Elements(
                        EqualTo(input).Select(value => (object)value),
                        EqualTo(() => input).Select(value => (object)value),
                        LessThan(input).Select(value => (object)value),
                        LessThan(() => input).Select(value => (object)value),
                        LessOrEqual(input).Select(value => (object)value),
                        LessOrEqual(() => input).Select(value => (object)value),
                        GreaterThan(input).Select(value => (object)value),
                        GreaterThan(() => input).Select(value => (object)value),
                        GreaterOrEqual(() => input).Select(value => (object)value),
                        GreaterOrEqual(() => input).Select(value => (object)value),
                        Any<string>().Select(value => (object)value))
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

        private class ArbitraryMapper : Arbitrary<Func<string, int>>
        {
            public override Gen<Func<string, int>> Generator
                => Gen.Elements<Func<string, int>>(
                    str => str?.Length ?? -1,
                    str => Int32.TryParse(str, out int result) ? result : 0,
                    str => (str?.Length ?? -1) % 5);
        }

        private class ArbitraryMatchResult : Arbitrary<MatchResult<string>>
        {
            public override Gen<MatchResult<string>> Generator
                => Gen.Frequency(
                    Tuple.Create(9, Arb.Default.String().Generator.Select(MatchResult.Success)),
                    Tuple.Create(1, Gen.Constant(MatchResult.Failure<string>())));
        }
    }
}
