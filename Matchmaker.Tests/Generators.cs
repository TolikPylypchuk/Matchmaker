namespace Matchmaker;

public static class Generators
{
    public static Arbitrary<IPattern<string, string>> ArbPattern() =>
        new ArbitraryPattern();

    public static Arbitrary<IPattern<string, object>> ArbObjectPattern() =>
        new ArbitraryObjectPattern();

    public static Arbitrary<Func<string, bool>> ArbPredicate() =>
        new ArbitraryPredicate();

    public static Arbitrary<Func<string, MatchResult<string>>> ArbMatcher() =>
        new ArbitraryMatcher();

    public static Arbitrary<Func<string, int>> ArbMapper() =>
        new ArbitraryMapper();

    public static Arbitrary<Func<string, IPattern<string, string>>> ArbPatternBinder() =>
        new ArbitraryBinder();

    public static Arbitrary<MatchResult<string>> ArbResult() =>
        new ArbitraryMatchResult();

    public static Arbitrary<Func<string, MatchResult<int>>> ArbResultBinder() =>
        new ArbitraryResultBinder();

    public static Arbitrary<IAsyncPattern<string, string>> ArbAsyncPattern() =>
        new ArbitraryAsyncPattern();

    public static Arbitrary<IAsyncPattern<string, object>> ArbAsyncObjectPattern() =>
        new ArbitraryAsyncObjectPattern();

    public static Arbitrary<Func<string, Task<bool>>> ArbAsyncPredicate() =>
        new ArbitraryAsyncPredicate();

    public static Arbitrary<Func<string, Task<MatchResult<string>>>> ArbAsyncMatcher() =>
        new ArbitraryAsyncMatcher();

    public static Arbitrary<Func<string, IAsyncPattern<string, string>>> ArbAsyncPatternBinder() =>
        new ArbitraryAsyncBinder();

    public static Arbitrary<Task<MatchResult<string>>> ArbAsyncResult() =>
        new ArbitraryAsyncMatchResult();

    public static Arbitrary<Func<string, Task<MatchResult<int>>>> ArbAsyncResultBinder() =>
        new ArbitraryAsyncResultBinder();

    public static Arbitrary<Task<string>> ArbAsyncString() =>
        new ArbitraryAsyncString();

    private static IEnumerable<IPattern<string, string>> Patterns(string input) =>
        new List<IPattern<string, string>>
        {
                Pattern.EqualTo(input),
                Pattern.EqualTo(() => input),
                Pattern.LessThan(input),
                Pattern.LessThan(() => input),
                Pattern.LessOrEqual(input),
                Pattern.LessOrEqual(() => input),
                Pattern.GreaterThan(input),
                Pattern.GreaterThan(() => input),
                Pattern.GreaterOrEqual(() => input),
                Pattern.GreaterOrEqual(() => input),
                Pattern.Any<string>()
        };

    private static IEnumerable<IAsyncPattern<string, string>> AsyncPatterns(string input) =>
        new List<IAsyncPattern<string, string>>
        {
                AsyncPattern.EqualTo(Task.FromResult(input)),
                AsyncPattern.EqualTo(() => Task.FromResult(input)),
                AsyncPattern.LessThan(Task.FromResult(input)),
                AsyncPattern.LessThan(() => Task.FromResult(input)),
                AsyncPattern.LessOrEqual(Task.FromResult(input)),
                AsyncPattern.LessOrEqual(() => Task.FromResult(input)),
                AsyncPattern.GreaterThan(Task.FromResult(input)),
                AsyncPattern.GreaterThan(() => Task.FromResult(input)),
                AsyncPattern.GreaterOrEqual(() => Task.FromResult(input)),
                AsyncPattern.GreaterOrEqual(() => Task.FromResult(input)),
                AsyncPattern.Any<string>()
        };

    private class ArbitraryPattern : Arbitrary<IPattern<string, string>>
    {
        public override Gen<IPattern<string, string>> Generator =>
            from input in Arb.Default.String().Generator
               from item in Gen.Elements(Patterns(input))
               select item;
    }

    private class ArbitraryObjectPattern : Arbitrary<IPattern<string, object>>
    {
        public override Gen<IPattern<string, object>> Generator =>
            ArbPattern().Generator.Select(pattern => pattern.Select(input => (object)input));
    }

    private class ArbitraryPredicate : Arbitrary<Func<string, bool>>
    {
        public override Gen<Func<string, bool>> Generator =>
            Gen.Elements(
                    str => str == null,
                    String.IsNullOrEmpty,
                    str => str == "abc",
                    str => str != null && str == str.ToLower());
    }

    private class ArbitraryMatcher : Arbitrary<Func<string, MatchResult<string>>>
    {
        public override Gen<Func<string, MatchResult<string>>> Generator =>
            ArbPredicate().Generator.Select(this.ResultFromPredicate);

        private Func<string, MatchResult<string>> ResultFromPredicate(Func<string, bool> predicate) =>
            str => predicate(str) ? MatchResult.Success(str) : MatchResult.Failure<string>();
    }

    private class ArbitraryMapper : Arbitrary<Func<string, int>>
    {
        public override Gen<Func<string, int>> Generator =>
            Gen.Elements<Func<string, int>>(
                str => str?.Length ?? -1,
                str => Int32.TryParse(str, out int result) ? result : 0,
                str => (str?.Length ?? -1) % 5);
    }

    private class ArbitraryBinder : Arbitrary<Func<string, IPattern<string, string>>>
    {
        public override Gen<Func<string, IPattern<string, string>>> Generator =>
            Gen.Choose(0, 10).Select<int, Func<string, IPattern<string, string>>>(
                index => str => Patterns(str).Skip(index).First());
    }

    private class ArbitraryMatchResult : Arbitrary<MatchResult<string>>
    {
        public override Gen<MatchResult<string>> Generator =>
            Gen.Frequency(
                Tuple.Create(9, Arb.Default.String().Generator.Select(MatchResult.Success)),
                Tuple.Create(1, Gen.Constant(MatchResult.Failure<string>())));
    }

    private class ArbitraryResultBinder : Arbitrary<Func<string, MatchResult<int>>>
    {
        public override Gen<Func<string, MatchResult<int>>> Generator =>
            Gen.Elements<Func<string, MatchResult<int>>>(
                str => MatchResult.Failure<int>(),
                str => MatchResult.Success(str?.Length ?? -1),
                str => MatchResult.Success(Int32.TryParse(str, out int result) ? result : 0),
                str => MatchResult.Success((str?.Length ?? -1) % 5));
    }

    private class ArbitraryAsyncPattern : Arbitrary<IAsyncPattern<string, string>>
    {
        public override Gen<IAsyncPattern<string, string>> Generator =>
            from input in Arb.Default.String().Generator
               from item in Gen.Elements(AsyncPatterns(input))
               select item;
    }

    private class ArbitraryAsyncObjectPattern : Arbitrary<IAsyncPattern<string, object>>
    {
        public override Gen<IAsyncPattern<string, object>> Generator =>
            ArbAsyncPattern().Generator.Select(pattern => pattern.Select(input => (object)input));
    }

    private class ArbitraryAsyncPredicate : Arbitrary<Func<string, Task<bool>>>
    {
        public override Gen<Func<string, Task<bool>>> Generator =>
            Gen.Elements<Func<string, Task<bool>>>(
                    str => Task.FromResult(str == null),
                    str => Task.FromResult(String.IsNullOrEmpty(str)),
                    str => Task.FromResult(str == "abc"),
                    str => Task.FromResult(str != null && str == str.ToLower()));
    }

    private class ArbitraryAsyncMatcher : Arbitrary<Func<string, Task<MatchResult<string>>>>
    {
        public override Gen<Func<string, Task<MatchResult<string>>>> Generator =>
            ArbAsyncPredicate().Generator.Select(this.ResultFromPredicate);

        private Func<string, Task<MatchResult<string>>> ResultFromPredicate(Func<string, Task<bool>> predicate) =>
            async str => await predicate(str) ? MatchResult.Success(str) : MatchResult.Failure<string>();
    }

    private class ArbitraryAsyncBinder : Arbitrary<Func<string, IAsyncPattern<string, string>>>
    {
        public override Gen<Func<string, IAsyncPattern<string, string>>> Generator =>
            Gen.Choose(0, 10).Select<int, Func<string, IAsyncPattern<string, string>>>(
                index => str => AsyncPatterns(str).Skip(index).First());
    }

    private class ArbitraryAsyncMatchResult : Arbitrary<Task<MatchResult<string>>>
    {
        public override Gen<Task<MatchResult<string>>> Generator =>
            Gen.Frequency(
                Tuple.Create(9, Arb.Default.String().Generator.Select(
                    str => Task.FromResult(MatchResult.Success(str)))),
                Tuple.Create(1, Gen.Constant(Task.FromResult(MatchResult.Failure<string>()))));
    }

    private class ArbitraryAsyncResultBinder : Arbitrary<Func<string, Task<MatchResult<int>>>>
    {
        public override Gen<Func<string, Task<MatchResult<int>>>> Generator =>
            Gen.Elements<Func<string, Task<MatchResult<int>>>>(
                str => Task.FromResult(MatchResult.Failure<int>()),
                str => Task.FromResult(MatchResult.Success(str?.Length ?? -1)),
                str => Task.FromResult(MatchResult.Success(Int32.TryParse(str, out int result) ? result : 0)),
                str => Task.FromResult(MatchResult.Success((str?.Length ?? -1) % 5)));
    }

    private class ArbitraryAsyncString : Arbitrary<Task<string>>
    {
        public override Gen<Task<string>> Generator =>
            Arb.Default.String().Generator.Select(Task.FromResult);
    }
}
