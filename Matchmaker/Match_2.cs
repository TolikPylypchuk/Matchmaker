using System;

using LanguageExt;
using LanguageExt.UnsafeValueAccess;

using static LanguageExt.Prelude;

namespace Matchmaker
{
    /// <summary>
    /// Represents a match expression.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TOutput">The type of the output value of the expression.</typeparam>
    /// <seealso cref="Match{TInput}" />
    /// <seealso cref="Match" />
    /// <seealso cref="MatchException" />
    public sealed class Match<TInput, TOutput>
    {
        /// <summary>
        /// The list of cases that will be matched in this expression.
        /// </summary>
        private readonly Lst<CaseData> cases;

        /// <summary>
        /// The default fallthrough behaviour.
        /// </summary>
        private readonly bool fallthroughByDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class.
        /// </summary>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        internal Match(bool fallthroughByDefault)
            => this.fallthroughByDefault = fallthroughByDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class with the specified cases.
        /// </summary>
        /// <param name="cases">The cases of this expression.</param>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        private Match(Lst<CaseData> cases, bool fallthroughByDefault)
        {
            this.cases = cases;
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Returns a new match expression which includes the specified pattern and function to execute if this
        /// pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>
        /// A new match expression which includes the specified pattern and function to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput, TOutput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, TOutput> func)
            => this.Case(pattern, this.fallthroughByDefault, func);

        /// <summary>
        /// Returns a new match expression which includes the specified pattern and function to execute if this
        /// pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="fallthrough">The fallthrough behaviour.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>
        /// A new match expression which includes the specified pattern and function to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput, TOutput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Func<TMatchResult, TOutput> func)
            => pattern != null
                ? func != null
                    ? new Match<TInput, TOutput>(
                        this.cases.Add(new CaseData(pattern, fallthrough, value => func((TMatchResult)value))),
                        this.fallthroughByDefault)
                    : throw new ArgumentNullException(nameof(func))
                : throw new ArgumentNullException(nameof(pattern));

        /// <summary>
        /// Returns a new match expression which includes the pattern for the specified type
        /// and function to execute if this pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>
        /// A new match expression which includes the type pattern and function to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput, TOutput> Case<TType>(Func<TType, TOutput> func)
            where TType : TInput
            => this.Case(this.fallthroughByDefault, func);

        /// <summary>
        /// Returns a new match expression which includes the pattern for the specified type
        /// and function to execute if this pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="fallthrough">The fallthrough behaviour.</param>
        /// <param name="func">The function to execute if the match is successful.</param>
        /// <returns>
        /// A new match expression which includes the type pattern and function to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput, TOutput> Case<TType>(bool fallthrough, Func<TType, TOutput> func)
            where TType : TInput
            => this.Case(Pattern.Type<TInput, TType>(), fallthrough, func);

        /// <summary>
        /// Executes the match expression on the specified input and returns the result.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>The result of the match expression.</returns>
        /// <exception cref="MatchException">
        /// The match failed for all cases.
        /// </exception>
        public TOutput ExecuteOn(TInput input)
            => this.ExecuteNonStrict(input).IfNoneUnsafe(() => throw new MatchException($"Could not match {input}."));

        /// <summary>
        /// Executes the match expression on the specified input and returns the result.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>The result of the match expression, or nothing if no pattern was matched successfully.</returns>
        public OptionUnsafe<TOutput> ExecuteNonStrict(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSome)
                {
                    return SomeUnsafe(@case.Function(matchResult.ValueUnsafe()));
                }
            }

            return None;
        }

        /// <summary>
        /// Executes the match expression on the specified input with fallthrough and returns the results.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>The results of the match expression.</returns>
        /// <exception cref="MatchException">
        /// The match failed for all cases.
        /// </exception>
        public Lst<TOutput> ExecuteWithFallthrough(TInput input)
        {
            var results = this.ExecuteNonStrictWithFallthrough(input);

            if (results.Count == 0)
            {
                throw new MatchException($"Could not match {input}.");
            }

            return results;
        }

        /// <summary>
        /// Executes the match expression on the specified input with fallthrough and returns the results.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>
        /// The results of the match expression, which is empty if no pattern is matched successfully.
        /// </returns>
        public Lst<TOutput> ExecuteNonStrictWithFallthrough(TInput input)
        {
            Lst<TOutput> results;
            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSome)
                {
                    results = results.Add(@case.Function(matchResult.ToList()[0]));

                    if (!@case.Fallthrough)
                    {
                        break;
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        public Func<TInput, TOutput> ToFunction()
            => this.ExecuteOn;

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        public Func<TInput, OptionUnsafe<TOutput>> ToNonStrictFunction()
            => this.ExecuteNonStrict;

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        public Func<TInput, Lst<TOutput>> ToFunctionWithFallthrough()
            => this.ExecuteWithFallthrough;

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        public Func<TInput, Lst<TOutput>> ToNonStrictFunctionWithFallthrough()
            => this.ExecuteNonStrictWithFallthrough;

        /// <summary>
        /// Represents the data of a single case in a match expression.
        /// </summary>
        private class CaseData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CaseData" /> class.
            /// </summary>
            /// <param name="pattern">The pattern of the case.</param>
            /// <param name="fallthrough">The fallthrough behaviour of the case.</param>
            /// <param name="func">The function of the case.</param>
            public CaseData(IPattern<TInput> pattern, bool fallthrough, Func<object, TOutput> func)
            {
                this.Pattern = pattern;
                this.Fallthrough = fallthrough;
                this.Function = func;
            }

            /// <summary>
            /// The pattern of the case.
            /// </summary>
            public IPattern<TInput> Pattern { get; }

            /// <summary>
            /// The fallthrough behaviour of the case.
            /// </summary>
            public bool Fallthrough { get; }

            /// <summary>
            /// The function of the case.
            /// </summary>
            public Func<object, TOutput> Function { get; }
        }
    }
}
