using System;
using System.Diagnostics.CodeAnalysis;

using FsCheck;

using LanguageExt;

using static LanguageExt.Prelude;

using static PatternMatching.Pattern;

namespace PatternMatching
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

        class ArbitrarySimplePattern : Arbitrary<SimplePattern<string>>
        {
            public override Gen<SimplePattern<string>> Generator
                => from input in Arb.Default.String().Generator
                    from index in Gen.Choose(0, 10)
                    select new[]
                    {
                        EqualTo(input), EqualTo(() => input),
                        LessThan(input), LessThan(() => input),
                        LessOrEqual(input), LessOrEqual(() => input),
                        GreaterThan(input), GreaterThan(() => input),
                        GreaterOrEqual(() => input), GreaterOrEqual(() => input),
                        Any<string>()
                    }[index];
        }

        class ArbitraryPredicate : Arbitrary<Func<string, bool>>
        {
            public override Gen<Func<string, bool>> Generator
                => Gen.Elements<Func<string, bool>>(
                        str => str == null,
                        String.IsNullOrEmpty,
                        str => str == "abc",
                        str => str != null && str == str.ToLower());
        }

        class ArbitraryMatcher : Arbitrary<Func<string, OptionUnsafe<string>>>
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
