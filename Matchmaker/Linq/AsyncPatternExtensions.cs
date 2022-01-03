namespace Matchmaker.Linq;

using System;
using System.Threading.Tasks;

using Matchmaker.Patterns;
using Matchmaker.Patterns.Async;

using static Matchmaker.Patterns.Async.AsyncPattern;

/// <summary>
/// A container class for extension methods for <see cref="IAsyncPattern{TInput, TMatchResult}" />.
/// </summary>
/// <seealso cref="IAsyncPattern{TInput,TMatchResult}" />
public static class AsyncPatternExtensions
{
    /// <summary>
    /// Returns a pattern which maps the result of the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="mapper">The result mapping function.</param>
    /// <returns>
    /// A pattern which maps the result of the specified pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="mapper" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Select<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, TMatchResult> mapper) =>
        new MappingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            mapper ?? throw new ArgumentNullException(nameof(mapper)));

    /// <summary>
    /// Returns a pattern which maps the result of the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="mapper">The result mapping function.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which maps the result of the specified pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" />, <paramref name="mapper" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Select<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, TMatchResult> mapper,
        string description) =>
        new MappingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            mapper ?? throw new ArgumentNullException(nameof(mapper)),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which pipes the result of one pattern to the other pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="firstPattern">The pattern whose result should be piped.</param>
    /// <param name="secondPattern">The pattern whose input is the first pattern's output.</param>
    /// <returns>
    /// A pattern which pipes the result of one pattern to the other pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="firstPattern" /> or <paramref name="secondPattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Pipe<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> firstPattern,
        IAsyncPattern<TIntermediateResult, TMatchResult> secondPattern) =>
        new PipingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            firstPattern ?? throw new ArgumentNullException(nameof(firstPattern)),
            secondPattern ?? throw new ArgumentNullException(nameof(secondPattern)));

    /// <summary>
    /// Returns a pattern which pipes the result of one pattern to the other pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="firstPattern">The pattern whose result should be piped.</param>
    /// <param name="secondPattern">The pattern whose input is the first pattern's output.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which pipes the result of one pattern to the other pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="firstPattern" />, <paramref name="secondPattern" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Pipe<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> firstPattern,
        IAsyncPattern<TIntermediateResult, TMatchResult> secondPattern,
        string description) =>
        new PipingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            firstPattern ?? throw new ArgumentNullException(nameof(firstPattern)),
            secondPattern ?? throw new ArgumentNullException(nameof(secondPattern)),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which pipes the result of one pattern to the pattern
    /// created by the specified matcher function.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be piped.</param>
    /// <param name="matcher">The pattern whose input is the first pattern's output.</param>
    /// <returns>
    /// A pattern which pipes the result of one pattern to the pattern
    /// created by the specified matcher function.
    /// </returns>
    /// <remarks>
    /// This method is functionally equivalent to the following, except it reuses the pattern's description:
    /// <code>
    /// pattern.Pipe(Pattern.CreatePattern(matcher))
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="matcher" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Pipe<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, Task<MatchResult<TMatchResult>>> matcher) =>
        pattern.Pipe(matcher, pattern?.Description ?? throw new ArgumentNullException(nameof(pattern)));

    /// <summary>
    /// Returns a pattern which pipes the result of one pattern to the pattern
    /// created by the specified matcher function.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be piped.</param>
    /// <param name="matcher">The pattern whose input is the first pattern's output.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which pipes the result of one pattern to the pattern
    /// created by the specified matcher function.
    /// </returns>
    /// <remarks>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// pattern.Pipe(Pattern.CreatePattern(matcher), description)
    /// </code>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" />, <paramref name="matcher" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Pipe<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, Task<MatchResult<TMatchResult>>> matcher,
        string description) =>
        new PipingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            CreatePattern(matcher ?? throw new ArgumentNullException(nameof(matcher))),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which casts the result of the specified pattern to another type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be cast.</param>
    /// <returns>
    /// A pattern which casts the result of the specified pattern to another type.
    /// </returns>
    /// <remarks>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// pattern.Pipe(Pattern.Type&lt;TIntermediateResult, TMatchResult&gt;())
    /// </code>
    /// As such, this pattern fails if the input is <see langword="null" /> and <typeparamref name="TMatchResult"/>
    /// is a non-nullable value type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Cast<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern)
        where TMatchResult : TIntermediateResult =>
        pattern.Pipe(Type<TIntermediateResult, TMatchResult>());

    /// <summary>
    /// Returns a pattern which casts the result of the specified pattern to another type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be cast.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which pipes the result of one pattern to the other pattern when it's successful.
    /// </returns>
    /// <remarks>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// pattern.Pipe(Pattern.Type&lt;TIntermediateResult, TMatchResult&gt;(), description)
    /// </code>
    /// As such, this pattern fails if the input is <see langword="null" /> and <typeparamref name="TMatchResult"/>
    /// is a non-nullable value type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Cast<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        string description)
        where TMatchResult : TIntermediateResult =>
        pattern.Pipe(Type<TIntermediateResult, TMatchResult>(), description);

    /// <summary>
    /// Returns a pattern which binds (flat-maps) the result of the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="binder">The result binding function.</param>
    /// <returns>
    /// A pattern which binds (flat-maps) the result of the specified pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="binder" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Bind<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, IAsyncPattern<TInput, TMatchResult>> binder) =>
        new BindingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            binder ?? throw new ArgumentNullException(nameof(binder)));

    /// <summary>
    /// Returns a pattern which binds (flat-maps) the result of the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TIntermediateResult">The type of the result of the specified pattern's match.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="binder">The result binding function.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which binds (flat-maps) the result of the specified pattern when it's successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" />, <paramref name="binder" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Bind<TInput, TIntermediateResult, TMatchResult>(
        this IAsyncPattern<TInput, TIntermediateResult> pattern,
        Func<TIntermediateResult, IAsyncPattern<TInput, TMatchResult>> binder,
        string description) =>
        new BindingAsyncPattern<TInput, TIntermediateResult, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            binder ?? throw new ArgumentNullException(nameof(binder)),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which adds a condition to the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="predicate">The condition to add.</param>
    /// <returns>
    /// A pattern which adds a condition to the specified pattern.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Where<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, bool> predicate) =>
        predicate != null
            ? pattern.Where(result => Task.FromResult(predicate(result)))
            : throw new ArgumentNullException(nameof(predicate));

    /// <summary>
    /// Returns a pattern which adds a condition to the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="predicate">The condition to add.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which adds a condition to the specified pattern.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Where<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, bool> predicate,
        string description) =>
        predicate != null
            ? pattern.Where(result => Task.FromResult(predicate(result)), description)
            : throw new ArgumentNullException(nameof(predicate));

    /// <summary>
    /// Returns a pattern which adds a condition to the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="predicate">The condition to add.</param>
    /// <returns>
    /// A pattern which adds a condition to the specified pattern.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Where<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, Task<bool>> predicate) =>
        new ConditionalAsyncPattern<TInput, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            predicate ?? throw new ArgumentNullException(nameof(predicate)));

    /// <summary>
    /// Returns a pattern which adds a condition to the specified pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose result should be mapped.</param>
    /// <param name="predicate">The condition to add.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which adds a condition to the specified pattern.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> Where<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        Func<TMatchResult, Task<bool>> predicate,
        string description) =>
        new ConditionalAsyncPattern<TInput, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            predicate ?? throw new ArgumentNullException(nameof(predicate)),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <param name="composition">The composition which should be applied to the patterns.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns.
    /// </returns>
    /// <remarks>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" /> or <paramref name="rightPattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Compose<T>(
        this IAsyncPattern<T, T> leftPattern,
        IAsyncPattern<T, T> rightPattern,
        PatternComposition composition) =>
        new CompositeAsyncPattern<T>(
            leftPattern ?? throw new ArgumentNullException(nameof(leftPattern)),
            rightPattern ?? throw new ArgumentNullException(nameof(rightPattern)),
            composition);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <param name="composition">The composition which should be applied to the patterns.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns.
    /// </returns>
    /// <remarks>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" />, <paramref name="rightPattern" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Compose<T>(
        this IAsyncPattern<T, T> leftPattern,
        IAsyncPattern<T, T> rightPattern,
        PatternComposition composition,
        string description) =>
        new CompositeAsyncPattern<T>(
            leftPattern ?? throw new ArgumentNullException(nameof(leftPattern)),
            rightPattern ?? throw new ArgumentNullException(nameof(rightPattern)),
            composition,
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.And)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" /> or <paramref name="rightPattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> And<T>(this IAsyncPattern<T, T> leftPattern, IAsyncPattern<T, T> rightPattern) =>
        leftPattern.Compose(rightPattern, PatternComposition.And);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.And, description)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" />, <paramref name="rightPattern" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> And<T>(
        this IAsyncPattern<T, T> leftPattern,
        IAsyncPattern<T, T> rightPattern,
        string description) =>
        leftPattern.Compose(rightPattern, PatternComposition.And, description);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.Or)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" /> or <paramref name="rightPattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Or<T>(this IAsyncPattern<T, T> leftPattern, IAsyncPattern<T, T> rightPattern) =>
        leftPattern.Compose(rightPattern, PatternComposition.Or);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// when either pattern's result is successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// when either pattern's result is successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.Or, description)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" />, <paramref name="rightPattern" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Or<T>(
        this IAsyncPattern<T, T> leftPattern,
        IAsyncPattern<T, T> rightPattern,
        string description) =>
        leftPattern.Compose(rightPattern, PatternComposition.Or, description);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// only when both patterns' results are successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.Xor)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" /> or <paramref name="rightPattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Xor<T>(this IAsyncPattern<T, T> leftPattern, IAsyncPattern<T, T> rightPattern) =>
        leftPattern.Compose(rightPattern, PatternComposition.Xor);

    /// <summary>
    /// Returns a pattern which is composed of the two specified patterns such that the result is successful
    /// when only one pattern's result is successful.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    /// <param name="leftPattern">The left pattern to compose.</param>
    /// <param name="rightPattern">The right pattern to compose.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which is composed of the two specified patterns such that the result is successful
    /// when only one pattern's result is successful.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The resulting pattern ignores the patterns' transformations
    /// and returns the input value if matched successfully.
    /// </para>
    /// <para>
    /// This method is functionally equivalent to the following:
    /// <code>
    /// leftPattern.Compose(rightPattern, PatternComposition.Xor, description)
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="leftPattern" />, <paramref name="rightPattern" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<T, T> Xor<T>(
        this IAsyncPattern<T, T> leftPattern,
        IAsyncPattern<T, T> rightPattern,
        string description) =>
        leftPattern.Compose(rightPattern, PatternComposition.Xor, description);

    /// <summary>
    /// Returns a pattern which matches the same as the specified pattern, but caches its results.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose results should be cached.</param>
    /// <returns>
    /// A pattern which matches the same as the specified pattern, but caches its results.
    /// </returns>
    /// <remarks>
    /// The returned pattern's caching process is thread-safe.
    /// The cache itself is a simple null-safe hash table.
    /// </remarks>
    public static IAsyncPattern<TInput, TMatchResult> Cached<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern) =>
        new CachingAsyncPattern<TInput, TMatchResult>(pattern ?? throw new ArgumentNullException(nameof(pattern)));

    /// <summary>
    /// Returns a pattern which matches the same as the specified pattern, but caches its results.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern whose results should be cached.</param>
    /// <param name="description">The description of this pattern.</param>
    /// <returns>
    /// A pattern which matches the same as the specified pattern, but caches its results.
    /// </returns>
    /// <remarks>
    /// The returned pattern's caching process is thread-safe.
    /// The cache itself is a simple null-safe hash table.
    /// </remarks>
    public static IAsyncPattern<TInput, TMatchResult> Cached<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        string description) =>
        new CachingAsyncPattern<TInput, TMatchResult>(
            pattern ?? throw new ArgumentNullException(nameof(pattern)),
            description ?? throw new ArgumentNullException(nameof(description)));
}
