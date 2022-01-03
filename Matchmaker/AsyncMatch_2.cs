namespace Matchmaker;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Matchmaker.Linq;
using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

/// <summary>
/// Represents an asynchronous match expression.
/// </summary>
/// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
/// <typeparam name="TOutput">The type of the output value of the expression.</typeparam>
/// <seealso cref="AsyncMatch{TInput}" />
/// <seealso cref="AsyncMatch" />
/// <seealso cref="MatchException" />
public sealed class AsyncMatch<TInput, TOutput>
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
    /// Initializes a new instance of the <see cref="AsyncMatch{TInput, TOutput}" /> class.
    /// </summary>
    /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
    internal AsyncMatch(bool fallthroughByDefault)
    {
        this.cases = new List<CaseData>().AsReadOnly();
        this.fallthroughByDefault = fallthroughByDefault;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncMatch{TInput, TOutput}" /> class.
    /// </summary>
    /// <param name="builder"></param>
    internal AsyncMatch(AsyncMatchBuilder<TInput, TOutput> builder)
    {
        this.cases = new List<CaseData>(builder.Cases).AsReadOnly();
        this.fallthroughByDefault = builder.FallthroughByDefault;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncMatch{TInput, TOutput}" /> class with the specified cases.
    /// </summary>
    /// <param name="cases">The cases of this expression.</param>
    /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
    private AsyncMatch(IReadOnlyCollection<CaseData> cases, bool fallthroughByDefault)
    {
        this.cases = cases;
        this.fallthroughByDefault = fallthroughByDefault;
    }

    /// <summary>
    /// Gets the global cache of static asynchronous match expressions.
    /// </summary>
    internal static ConcurrentDictionary<string, AsyncMatch<TInput, TOutput>> Cache { get; } = new();

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, Task<TOutput>> func) =>
        this.Case(pattern, this.fallthroughByDefault, func);

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, TOutput> func) =>
        this.Case(pattern, func.AsAsync());

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, Task<TOutput>> func) =>
        this.Case(pattern.AsAsync(), this.fallthroughByDefault, func);

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, TOutput> func) =>
        this.Case(pattern.AsAsync(), func.AsAsync());

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern,
        bool fallthrough,
        Func<TMatchResult, Task<TOutput>> func) =>
        pattern != null
            ? func != null
                ? new AsyncMatch<TInput, TOutput>(
                    new List<CaseData>(this.cases)
                    {
                            new CaseData(
                                pattern.Select(result => (object?)result),
                                fallthrough,
                                value => func((TMatchResult)value!))
                    }.AsReadOnly(),
                    this.fallthroughByDefault)
                : throw new ArgumentNullException(nameof(func))
            : throw new ArgumentNullException(nameof(pattern));

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern,
        bool fallthrough,
        Func<TMatchResult, TOutput> func) =>
        this.Case(pattern, fallthrough, func.AsAsync());

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IPattern<TInput, TMatchResult> pattern,
        bool fallthrough,
        Func<TMatchResult, Task<TOutput>> func) =>
        this.Case(pattern.AsAsync(), fallthrough, func);

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
    public AsyncMatch<TInput, TOutput> Case<TMatchResult>(
        IPattern<TInput, TMatchResult> pattern,
        bool fallthrough,
        Func<TMatchResult, TOutput> func) =>
        this.Case(pattern, fallthrough, func.AsAsync());

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
    /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), func)
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="func" /> is <see langword="null" />.
    /// </exception>
    public AsyncMatch<TInput, TOutput> Case<TType>(Func<TType, Task<TOutput>> func)
        where TType : TInput =>
        this.Case(this.fallthroughByDefault, func);

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
    /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), func)
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="func" /> is <see langword="null" />.
    /// </exception>
    public AsyncMatch<TInput, TOutput> Case<TType>(Func<TType, TOutput> func)
        where TType : TInput =>
        this.Case(this.fallthroughByDefault, func.AsAsync());

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
    /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), fallthrough, func)
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="func" /> is <see langword="null" />.
    /// </exception>
    public AsyncMatch<TInput, TOutput> Case<TType>(bool fallthrough, Func<TType, Task<TOutput>> func)
        where TType : TInput =>
        this.Case(AsyncPattern.Type<TInput, TType>(), fallthrough, func);

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
    /// match.Case(AsyncPattern.Type&lt;TInput, TType&gt;(), fallthrough, func)
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="func" /> is <see langword="null" />.
    /// </exception>
    public AsyncMatch<TInput, TOutput> Case<TType>(bool fallthrough, Func<TType, TOutput> func)
        where TType : TInput =>
        this.Case(fallthrough, func.AsAsync());

    /// <summary>
    /// Asynchronously executes the match expression on the specified input and returns the result.
    /// </summary>
    /// <param name="input">The input value of the expression.</param>
    /// <returns>The result of the match expression.</returns>
    /// <exception cref="MatchException">
    /// The match failed for all cases.
    /// </exception>
    public Task<TOutput> ExecuteAsync(TInput input) =>
        this.ExecuteNonStrictAsync(input).GetValueOrThrow(() => new MatchException($"Could not match {input}"));

    /// <summary>
    /// Asynchronously executes the match expression on the specified input and returns the result.
    /// </summary>
    /// <param name="input">The input value of the expression.</param>
    /// <returns>
    /// The result of the match expression, or a failed result if no pattern was matched successfully.
    /// </returns>
    public async Task<MatchResult<TOutput>> ExecuteNonStrictAsync(TInput input)
    {
        foreach (var @case in this.cases)
        {
            var matchResult = await @case.Pattern.MatchAsync(input);
            if (matchResult.IsSuccessful)
            {
                return MatchResult.Success(await @case.Function(matchResult.Value));
            }
        }

        return MatchResult.Failure<TOutput>();
    }

    /// <summary>
    /// Asynchronously executes the match expression on the specified input with fallthrough
    /// and lazily returns the results.
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
    public async IAsyncEnumerable<TOutput> ExecuteWithFallthroughAsync(TInput input)
    {
        foreach (var @case in this.cases)
        {
            var matchResult = await @case.Pattern.MatchAsync(input);
            if (matchResult.IsSuccessful)
            {
                yield return await @case.Function(matchResult.Value);

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
    public Func<TInput, Task<TOutput>> ToFunction() =>
        this.ExecuteAsync;

    /// <summary>
    /// Returns a function which, when called, will match the specified value.
    /// </summary>
    /// <returns>A function which, when called, will match the specified value.</returns>
    public Func<TInput, Task<MatchResult<TOutput>>> ToNonStrictFunction() =>
        this.ExecuteNonStrictAsync;

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
    public Func<TInput, IAsyncEnumerable<TOutput>> ToFunctionWithFallthrough() =>
        this.ExecuteWithFallthroughAsync;

    /// <summary>
    /// Represents the data of a single case in an asynchronous match expression.
    /// </summary>
    internal class CaseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseData" /> class.
        /// </summary>
        /// <param name="pattern">The pattern of the case.</param>
        /// <param name="fallthrough">The fallthrough behaviour of the case.</param>
        /// <param name="func">The function of the case.</param>
        public CaseData(IAsyncPattern<TInput, object?> pattern, bool fallthrough, Func<object?, Task<TOutput>> func)
        {
            this.Pattern = pattern;
            this.Fallthrough = fallthrough;
            this.Function = func;
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
        /// Gets the function of the case.
        /// </summary>
        public Func<object?, Task<TOutput>> Function { get; }
    }
}
