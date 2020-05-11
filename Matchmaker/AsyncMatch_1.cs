using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Matchmaker.Linq;
using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

namespace Matchmaker
{
    /// <summary>
    /// Represents an asynchronous match statement - a match expression that doesn't yield a value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <seealso cref="AsyncMatch{TInput, TOutput}" />
    /// <seealso cref="AsyncMatch" />
    /// <seealso cref="MatchException" />
    public sealed class AsyncMatch<TInput>
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
        /// Initializes a new instance of the <see cref="AsyncMatch{TInput}" /> class.
        /// </summary>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        internal AsyncMatch(bool fallthroughByDefault)
        {
            this.cases = new List<CaseData>().AsReadOnly();
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncMatch{TInput}" /> class.
        /// </summary>
        /// <param name="builder"></param>
        internal AsyncMatch(AsyncMatchBuilder<TInput> builder)
        {
            this.cases = new List<CaseData>(builder.Cases).AsReadOnly();
            this.fallthroughByDefault = builder.FallthroughByDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Match{TInput}" /> class with the specified cases.
        /// </summary>
        /// <param name="cases">The cases of this statement.</param>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        private AsyncMatch(IReadOnlyCollection<CaseData> cases, bool fallthroughByDefault)
        {
            this.cases = cases;
            this.fallthroughByDefault = fallthroughByDefault;
        }

        /// <summary>
        /// Gets the global cache of static match statements.
        /// </summary>
        internal static ConcurrentDictionary<string, AsyncMatch<TInput>> Cache { get; } =
            new ConcurrentDictionary<string, AsyncMatch<TInput>>();

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IAsyncPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, Task> action)
            => this.Case(pattern, this.fallthroughByDefault, action);

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IAsyncPattern<TInput, TMatchResult> pattern,
            Action<TMatchResult> action)
            => this.Case(pattern, this.fallthroughByDefault, action.AsAsync());

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            Func<TMatchResult, Task> action)
            => this.Case(pattern.AsAsync(), this.fallthroughByDefault, action);

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            Action<TMatchResult> action)
            => this.Case(pattern.AsAsync(), this.fallthroughByDefault, action.AsAsync());

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IAsyncPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Func<TMatchResult, Task> action)
            => pattern != null
                ? action != null
                    ? new AsyncMatch<TInput>(
                        new List<CaseData>(this.cases)
                        {
                            new CaseData(
                                pattern.Select(result => (object?)result),
                                fallthrough,
                                value => action((TMatchResult)value!))
                        }.AsReadOnly(),
                        this.fallthroughByDefault)
                    : throw new ArgumentNullException(nameof(action))
                : throw new ArgumentNullException(nameof(pattern));

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IAsyncPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Action<TMatchResult> action)
            => this.Case(pattern, fallthrough, action.AsAsync());

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Func<TMatchResult, Task> action)
            => this.Case(pattern.AsAsync(), fallthrough, action);

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
        public AsyncMatch<TInput> Case<TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            bool fallthrough,
            Action<TMatchResult> action)
            => this.Case(pattern.AsAsync(), fallthrough, action.AsAsync());

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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public AsyncMatch<TInput> Case<TType>(Func<TType, Task> action)
            where TType : TInput
            => this.Case(this.fallthroughByDefault, action);

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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public AsyncMatch<TInput> Case<TType>(Action<TType> action)
            where TType : TInput
            => this.Case(action.AsAsync());

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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), fallthrough, action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public AsyncMatch<TInput> Case<TType>(bool fallthrough, Func<TType, Task> action)
            where TType : TInput
            => this.Case(AsyncPattern.Type<TInput, TType>(), fallthrough, action);

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
        /// <remarks>
        /// This method is functionally equivalent to the following:
        /// <code>
        /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), fallthrough, action)
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public AsyncMatch<TInput> Case<TType>(bool fallthrough, Action<TType> action)
            where TType : TInput
            => this.Case(fallthrough, action.AsAsync());

        /// <summary>
        /// Asynchronously executes the match statement strictly on the specified input.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <exception cref="MatchException">
        /// The match failed for all cases.
        /// </exception>
        public async Task ExecuteAsync(TInput input)
        {
            bool isMatched = await this.ExecuteNonStrictAsync(input);

            if (!isMatched)
            {
                throw new MatchException($"Could not match {input}");
            }
        }

        /// <summary>
        /// Asynchronously executes the match statement non-strictly on the specified input.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <returns>
        /// <see langword="true" />, if the match was successful.
        /// Otherwise, <see langword="false" />.
        /// </returns>
        public async Task<bool> ExecuteNonStrictAsync(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = await @case.Pattern.MatchAsync(input);
                if (matchResult.IsSuccessful)
                {
                    await @case.Action(matchResult.Value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Asynchronously executes the match statement on the specified input with fallthrough lazily.
        /// </summary>
        /// <param name="input">The input value of the statement.</param>
        /// <returns>
        /// An enumerable of <see langword="null" /> objects which enables the execution to be lazy.
        /// The number of items in this enumerable equals the number of successful cases.
        /// </returns>
        public async IAsyncEnumerable<object?> ExecuteWithFallthroughAsync(TInput input)
        {
            foreach (var @case in this.cases)
            {
                var matchResult = await @case.Pattern.MatchAsync(input);
                if (matchResult.IsSuccessful)
                {
                    await @case.Action(matchResult.Value);
                    yield return null;

                    if (!@case.Fallthrough)
                    {
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an action which, when called, will match the specified value.
        /// </summary>
        /// <returns>An action which, when called, will match the specified value.</returns>
        public Func<TInput, Task> ToFunction()
            => this.ExecuteAsync;

        /// <summary>
        /// Returns a function which, when called, will match the specified value non-strictly.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value non-strictly.</returns>
        public Func<TInput, Task<bool>> ToNonStrictFunction()
            => this.ExecuteNonStrictAsync;

        /// <summary>
        /// Returns a function which, when called, will match the specified value with fallthrough.
        /// </summary>
        /// <returns>A function which, when called, will match the specified value with fallthrough.</returns>
        public Func<TInput, IAsyncEnumerable<object?>> ToFunctionWithFallthrough()
            => this.ExecuteWithFallthroughAsync;

        /// <summary>
        /// Represents the data of a single case in a match statement.
        /// </summary>
        internal class CaseData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CaseData" /> class.
            /// </summary>
            /// <param name="pattern">The pattern of the case.</param>
            /// <param name="fallthrough">The fallthrough behaviour of the case.</param>
            /// <param name="action">The action of the case.</param>
            public CaseData(IAsyncPattern<TInput, object?> pattern, bool fallthrough, Func<object?, Task> action)
            {
                this.Pattern = pattern;
                this.Fallthrough = fallthrough;
                this.Action = action;
            }

            /// <summary>
            /// Gets the pattern of the case.
            /// </summary>
            public IAsyncPattern<TInput, object?> Pattern { get; }

            /// <summary>
            /// Gets the fallthrough behaviour of the case.
            /// </summary>
            public bool Fallthrough { get; }

            /// <summary>
            /// Gets the action of the case.
            /// </summary>
            public Func<object?, Task> Action { get; }
        }
    }
}
