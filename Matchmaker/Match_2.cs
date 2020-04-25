using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Matchmaker.Linq;
using Matchmaker.Patterns;

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
        /// The collection of cases that will be matched in this expression.
        /// </summary>
        private readonly IReadOnlyCollection<CaseData> cases;

        /// <summary>
        /// The default fallthrough behaviour.
        /// </summary>
        private readonly bool fallthroughByDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class.
        /// </summary>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        internal Match(bool fallthroughByDefault)
        {
            this.cases = new List<CaseData>().AsReadOnly();
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class.
        /// </summary>
        /// <param name="builder"></param>
        internal Match(MatchBuilder<TInput, TOutput> builder)
        {
            this.cases = new List<CaseData>(builder.Cases).AsReadOnly();
            this.fallthroughByDefault = builder.FallthroughByDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput, TOutput}" /> class with the specified cases.
        /// </summary>
        /// <param name="cases">The cases of this expression.</param>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        private Match(IReadOnlyCollection<CaseData> cases, bool fallthroughByDefault)
        {
            this.cases = cases;
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Gets the global cache of static match expressions.
        /// </summary>
        internal static Dictionary<string, Match<TInput, TOutput>> Cache { get; } =
            new Dictionary<string, Match<TInput, TOutput>>();

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
                        new List<CaseData>(this.cases)
                        {
                            new CaseData(
                                Pattern<TInput>.FromActualPattern(pattern),
                                fallthrough,
                                value => func((TMatchResult)value!))
                        }.AsReadOnly(),
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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(Pattern.Type&lt;TInput, TType&gt;(), func)
        /// </code>
        /// </remarks>
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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(Pattern.Type&lt;TInput, TType&gt;(), fallthrough, func)
        /// </code>
        /// </remarks>
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
        /// <seealso cref="ExecuteNonStrict(TInput)" />
        /// <seealso cref="ExecuteWithFallthrough(TInput)" />
        /// <seealso cref="ToFunction" />
        [return: MaybeNull]
        public TOutput ExecuteOn(TInput input)
            => this.ExecuteNonStrict(input).GetValueOrThrow(() => new MatchException($"Could not match {input}."));

        /// <summary>
        /// Executes the match expression on the specified input and returns the result.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>
        /// The result of the match expression, or a failed result if no pattern was matched successfully.
        /// </returns>
        /// <seealso cref="ExecuteOn(TInput)" />
        /// <seealso cref="ExecuteWithFallthrough(TInput)" />
        /// <seealso cref="ToNonStrictFunction" />
        public MatchResult<TOutput> ExecuteNonStrict(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSuccessful)
                {
                    return MatchResult.Success(@case.Function(matchResult.Value));
                }
            }

            return MatchResult.Failure<TOutput>();
        }

        /// <summary>
        /// Executes the match expression on the specified input with fallthrough and lazily returns the results.
        /// </summary>
        /// <param name="input">The input value of the expression.</param>
        /// <returns>
        /// The results of the match expression, empty if no pattern is matched successfully.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method returns a lazy enumerable - it will check only as many patterns,
        /// as are needed to return one result at a time.
        /// </para>
        /// <para>
        /// The enumerable may contain <see langword="null" /> values.
        /// </para>
        /// </remarks>
        /// <seealso cref="ExecuteOn(TInput)" />
        /// <seealso cref="ExecuteNonStrict(TInput)" />
        /// <seealso cref="ToFunctionWithFallthrough" />
        public IEnumerable<TOutput> ExecuteWithFallthrough(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSuccessful)
                {
                    yield return @case.Function(matchResult.Value);

                    if (!@case.Fallthrough)
                    {
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        /// <seealso cref="ExecuteOn(TInput)" />
        /// <seealso cref="ToNonStrictFunction" />
        /// <seealso cref="ToFunctionWithFallthrough" />
        public Func<TInput, TOutput> ToFunction()
            => this.ExecuteOn;

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        /// <seealso cref="ExecuteNonStrict(TInput)" />
        /// <seealso cref="ToFunction" />
        /// <seealso cref="ToFunctionWithFallthrough" />
        public Func<TInput, MatchResult<TOutput>> ToNonStrictFunction()
            => this.ExecuteNonStrict;

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        /// <remarks>
        /// <para>
        /// This method returns a lazy enumerable - it will check only as many patterns,
        /// as are needed to return one result at a time.
        /// </para>
        /// <para>
        /// The enumerable may contain <see langword="null" /> values.
        /// </para>
        /// </remarks>
        /// <seealso cref="ExecuteWithFallthrough(TInput)" />
        /// <seealso cref="ToFunction" />
        /// <seealso cref="ToNonStrictFunction" />
        public Func<TInput, IEnumerable<TOutput>> ToFunctionWithFallthrough()
            => this.ExecuteWithFallthrough;

        /// <summary>
        /// Represents the data of a single case in a match expression.
        /// </summary>
        internal class CaseData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CaseData" /> class.
            /// </summary>
            /// <param name="pattern">The pattern of the case.</param>
            /// <param name="fallthrough">The fallthrough behaviour of the case.</param>
            /// <param name="func">The function of the case.</param>
            public CaseData(Pattern<TInput> pattern, bool fallthrough, Func<object?, TOutput> func)
            {
                this.Pattern = pattern;
                this.Fallthrough = fallthrough;
                this.Function = func;
            }

            /// <summary>
            /// Gets the pattern of the case.
            /// </summary>
            public Pattern<TInput> Pattern { get; }

            /// <summary>
            /// Gets the fallthrough behaviour of the case.
            /// </summary>
            public bool Fallthrough { get; }

            /// <summary>
            /// Gets the function of the case.
            /// </summary>
            public Func<object?, TOutput> Function { get; }
        }
    }
}
