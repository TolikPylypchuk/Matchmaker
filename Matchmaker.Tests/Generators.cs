using System;
using System.Diagnostics.CodeAnalysis;

using FsCheck;

using LanguageExt;

using static LanguageExt.Prelude;

using static Matchmaker.Pattern;

namespace Matchmaker
{
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public static class Generators
    {
        public static Arbitrary<SimplePattern<string>> SimplePattern()
            => new ArbitrarySimplePattern();

        public static Arbitrary<Func<string, bool>> Predicate()
            => new ArbitraryPredicate();

        public static Arbitrary<Func<string, OptionUnsafe<string>>> Matcher()
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

        private class ArbitraryMatcher : Arbitrary<Func<string, OptionUnsafe<string>>>
        {
            public override Gen<Func<string, OptionUnsafe<string>>> Generator
                => Gen.Elements<Func<string, OptionUnsafe<string>>>(
                    str => str == null ? SomeUnsafe(str) : None,
                    str => String.IsNullOrEmpty(str) ? SomeUnsafe(str) : None,
                    str => str == "abc" ? SomeUnsafe(str) : None,
                    str => str != null && str == str.ToLower() ? SomeUnsafe(str) : None);
        }
    }
}
