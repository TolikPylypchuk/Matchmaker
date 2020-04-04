using System;

using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace Matchmaker
{
    /// <summary>
    /// Represents a match statement - a match expression that doesn't yield a value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <seealso cref="Match{TInput, TOutput}" />
    /// <seealso cref="Match" />
    /// <seealso cref="MatchException" />
    public sealed class Match<TInput>
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
        /// Initializes a new instance of the <see cref="Match{TInput}" /> class.
        /// </summary>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        internal Match(bool fallthroughByDefault)
            => this.fallthroughByDefault = fallthroughByDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput}" /> class with the specified cases.
        /// </summary>
        /// <param name="cases">The cases of this statement.</param>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        private Match(Lst<CaseData> cases, bool fallthroughByDefault)
        {
            this.cases = cases;
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Returns a new match statement which includes the specified pattern and action to execute if this
        /// pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>
        /// A new match statement which includes the specified pattern and action to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput> Case<TMatchResult>(IPattern<TInput, TMatchResult> pattern, Action<TMatchResult> action)
            => this.Case(pattern, this.fallthroughByDefault, action);

        /// <summary>
        /// Returns a new match statement which includes the specified pattern and action to execute if this
        /// pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TMatchResult">The type of the result of the pattern's match.</typeparam>
        /// <param name="pattern">The pattern to match with.</param>
        /// <param name="fallthrough">The fallthrough behaviour.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>
        /// A new match statement which includes the specified pattern and action to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Action<TMatchResult> action)
            => pattern != null
                ? action != null
                    ? new Match<TInput>(
                        this.cases.Add(new CaseData(pattern, fallthrough, value => action((TMatchResult)value))),
                        this.fallthroughByDefault)
                    : throw new ArgumentNullException(nameof(action))
                : throw new ArgumentNullException(nameof(pattern));

        /// <summary>
        /// Returns a new match statement which includes the pattern for the specified type
        /// and action to execute if this pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>
        /// A new match statement which includes the type pattern and action to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput> Case<TType>(Action<TType> action)
            where TType : TInput
            => this.Case(this.fallthroughByDefault, action);

        /// <summary>
        /// Returns a new match statement which includes the pattern for the specified type
        /// and action to execute if this pattern is matched successfully.
        /// </summary>
        /// <typeparam name="TType">The type of the result of the pattern's match.</typeparam>
        /// <param name="fallthrough">The fallthrough behaviour.</param>
        /// <param name="action">The action to execute if the match is successful.</param>
        /// <returns>
        /// A new match statement which includes the type pattern and action to execute if this
        /// pattern is matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public Match<TInput> Case<TType>(bool fallthrough, Action<TType> action)
            where TType : TInput
            => this.Case(Pattern.Type<TInput, TType>(), fallthrough, action);

        /// <summary>
        /// Executes the match statement on the specified input.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <returns>
        /// <see langword="true" />, if the match was successful.
        /// Otherwise, <see langword="false" />.
        /// </returns>
        public bool ExecuteOn(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSome)
                {
                    @case.Action(matchResult.ValueUnsafe());
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Executes the match statement strictly on the specified input.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <exception cref="MatchException">
        /// The match failed for all cases.
        /// </exception>
        public void ExecuteStrict(TInput input)
        {
            bool isMatched = this.ExecuteOn(input);

            if (!isMatched)
            {
                throw new MatchException($"Could not match {input}.");
            }
        }

        /// <summary>
        /// Executes the match statement on the specified input with fallthrough.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <returns>
        /// The number of patterns that were matched successfully.
        /// </returns>
        public int ExecuteWithFallthrough(TInput input)
        {
            int numberOfMatches = 0;

            foreach (var @case in this.cases)
            {
                var matchResult = @case.Pattern.Match(input);
                if (matchResult.IsSome)
                {
                    @case.Action(matchResult.ValueUnsafe());
                    numberOfMatches++;

                    if (!@case.Fallthrough)
                    {
                        break;
                    }
                }
            }

            return numberOfMatches;
        }

        /// <summary>
        /// Executes the match statement strictly on the specified input with fallthrough.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <returns>
        /// The number of patterns that were matched successfully.
        /// </returns>
        /// <exception cref="MatchException">
        /// The match failed for all cases.
        /// </exception>
        public int ExecuteStrictWithFallthrough(TInput input)
        {
            int numberOfMatches = this.ExecuteWithFallthrough(input);

            if (numberOfMatches == 0)
            {
                throw new MatchException($"Could not match {input}.");
            }

            return numberOfMatches;
        }

        /// <summary>
        /// Returns a function which, when called, will match the specified value.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value.</returns>
        public Func<TInput, bool> ToFunction()
            => this.ExecuteOn;

        /// <summary>
        /// Returns an action which, when called, will match the specified value strictly.
        /// </summary>
        /// <returns>An action which, when called, will match the specified value strictly.</returns>
        public Action<TInput> ToStrictFunction()
            => this.ExecuteStrict;

        /// <summary>
        /// Returns a function which, when called, will match the specified value with fallthrough.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value with fallthrough.</returns>
        public Func<TInput, int> ToFunctionWithFallthrough()
            => this.ExecuteWithFallthrough;

        /// <summary>
        /// Returns a function which, when called, will match the specified value strictly with fallthrough.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value strictly with fallthrough.</returns>
        public Func<TInput, int> ToStrictFunctionWithFallthrough()
            => this.ExecuteStrictWithFallthrough;

        /// <summary>
        /// Represents the data of a single case in a match statement.
        /// </summary>
        private class CaseData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CaseData" /> class.
            /// </summary>
            /// <param name="pattern">The pattern of the case.</param>
            /// <param name="fallthrough">The fallthrough behaviour of the case.</param>
            /// <param name="action">The action of the case.</param>
            public CaseData(IPattern<TInput> pattern, bool fallthrough, Action<object> action)
            {
                this.Pattern = pattern;
                this.Fallthrough = fallthrough;
                this.Action = action;
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
            /// The action of the case.
            /// </summary>
            public Action<object> Action { get; }
        }
    }
}
