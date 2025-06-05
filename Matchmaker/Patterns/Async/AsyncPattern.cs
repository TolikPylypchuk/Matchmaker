namespace Matchmaker.Patterns.Async;

/// <summary>
/// Contains factory methods for creating asynchronous patterns.
/// </summary>
/// <seealso cref="IAsyncPattern{TInput, TMatchResult}" />
/// <seealso cref="AsyncPattern{TInput, TMatchResult}" />
public static class AsyncPattern
{
    /// <summary>
    /// The default description of the 'any' pattern.
    /// </summary>
    public static readonly string DefaultAnyDescription = "any x";

    /// <summary>
    /// The default description of the 'return' pattern.
    /// </summary>
    public static readonly string DefaultReturnDescription = "return <provided value>";

    /// <summary>
    /// The default description of 'null' patterns.
    /// </summary>
    public static readonly string DefaultNullDescription = "x is null";

    /// <summary>
    /// The default description of equality patterns.
    /// </summary>
    public static readonly string DefaultEqualToDescription = "x = <provided value>";

    /// <summary>
    /// The default description of less-than patterns.
    /// </summary>
    public static readonly string DefaultLessThanDescription = "x < <provided value>";

    /// <summary>
    /// The default description of less-or-equal patterns.
    /// </summary>
    public static readonly string DefaultLessOrEqualDescription = "x <= <provided value>";

    /// <summary>
    /// The default description of greater-than patterns.
    /// </summary>
    public static readonly string DefaultGreaterThanDescription = "x > <provided value>";

    /// <summary>
    /// The default description of greater-or-equal patterns.
    /// </summary>
    public static readonly string DefaultGreaterOrEqualDescription = "x >= <provided value>";

    /// <summary>
    /// The default description of type patterns.
    /// </summary>
    public static readonly string DefaultTypeDescriptionFormat = "x is {0}";

    /// <summary>
    /// The default description of piping patterns.
    /// </summary>
    public static readonly string DefaultPipeDescriptionFormat = "({0}) => ({1})";

    /// <summary>
    /// The default description of the 'and' pattern combinator.
    /// </summary>
    public static readonly string DefaultAndDescriptionFormat = "({0}) and ({1})";

    /// <summary>
    /// The default description of the 'or' pattern combinator.
    /// </summary>
    public static readonly string DefaultOrDescriptionFormat = "({0}) or ({1})";

    /// <summary>
    /// The default description of the 'xor' pattern combinator.
    /// </summary>
    public static readonly string DefaultXorDescriptionFormat = "({0}) xor ({1})";

    /// <summary>
    /// The default description of the 'not' pattern combinators.
    /// </summary>
    public static readonly string DefaultNotDescriptionFormat = "not ({0})";

    /// <summary>
    /// Creates a pattern which uses a specified function to match its inputs.
    /// </summary>
    /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
    /// <typeparam name="TMatchResult">The type of the pattern's results.</typeparam>
    /// <param name="matcher">The function which matches the inputs.</param>
    /// <returns>
    /// A pattern which matches its inputs according to the specified matcher function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="matcher" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> CreatePattern<TInput, TMatchResult>(
        Func<TInput, Task<MatchResult<TMatchResult>>> matcher) =>
        matcher != null
            ? new SimpleAsyncPattern<TInput, TMatchResult>(matcher)
            : throw new ArgumentNullException(nameof(matcher));

    /// <summary>
    /// Creates a pattern which uses a specified function to match its inputs and has a specified description.
    /// </summary>
    /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
    /// <typeparam name="TMatchResult">The type of the pattern's results.</typeparam>
    /// <param name="matcher">The function which matches the inputs.</param>
    /// <param name="description">The pattern's description.</param>
    /// <returns>
    /// A pattern which matches its inputs according to the specified matcher function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="matcher" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TMatchResult> CreatePattern<TInput, TMatchResult>(
        Func<TInput, Task<MatchResult<TMatchResult>>> matcher,
        string description) =>
        matcher != null
            ? new SimpleAsyncPattern<TInput, TMatchResult>(
                matcher,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(matcher));

    /// <summary>
    /// Creates a pattern which uses a specified predicate to match its inputs.
    /// </summary>
    /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
    /// <param name="predicate">The predicate which matches the inputs.</param>
    /// <returns>
    /// A pattern which matches its inputs according to the specified predicate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> CreatePattern<TInput>(Func<TInput, Task<bool>> predicate) =>
        predicate != null
            ? new SimpleAsyncPattern<TInput, TInput>(
                async input => await predicate(input)
                    ? MatchResult.Success(input)
                    : MatchResult.Failure<TInput>())
            : throw new ArgumentNullException(nameof(predicate));

    /// <summary>
    /// Creates a pattern which uses a specified predicate to match its inputs and has a specified description.
    /// </summary>
    /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
    /// <param name="predicate">The predicate which matches the inputs.</param>
    /// <param name="description">The pattern's description.</param>
    /// <returns>
    /// A pattern which matches its inputs according to the specified predicate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="predicate" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> CreatePattern<TInput>(
        Func<TInput, Task<bool>> predicate,
        string description) =>
        predicate != null
            ? new SimpleAsyncPattern<TInput, TInput>(
                async input => await predicate(input)
                    ? MatchResult.Success(input)
                    : MatchResult.Failure<TInput>(),
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(predicate));

    /// <summary>
    /// Returns a pattern which is always matched successfully.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <returns>A pattern which is always matched successfully.</returns>
    /// <remarks>
    /// This pattern should be used as the default case of a match expression, if one is needed.
    /// </remarks>
    public static IAsyncPattern<TInput, TInput> Any<TInput>() =>
        Any<TInput>(DefaultAnyDescription);

    /// <summary>
    /// Returns a pattern which is always matched successfully.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>A pattern which is always matched successfully.</returns>
    /// <remarks>
    /// This pattern should be used as the default case of a match expression, if one is needed.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> Any<TInput>(string description) =>
        CreatePattern<TInput>(
            _ => Task.FromResult(true),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which always successfully returns the specified value, discarding its input value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TValue">The type of the value to return.</typeparam>
    /// <param name="value">The value to return.</param>
    /// <returns>A pattern which always successfully returns the specified value.</returns>
    /// <remarks>
    /// This pattern is much like the <see cref="Any{TInput}()" /> pattern,
    /// except it returns the specified value instead of the pattern's input.
    /// </remarks>
    public static IAsyncPattern<TInput, TValue> Return<TInput, TValue>(Task<TValue> value) =>
        Return<TInput, TValue>(value, DefaultReturnDescription);

    /// <summary>
    /// Returns a pattern which always successfully returns the specified value, discarding its input value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TValue">The type of the value to return.</typeparam>
    /// <param name="value">The value to return.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>A pattern which always successfully returns the specified value.</returns>
    /// <remarks>
    /// This pattern is much like the <see cref="Any{TInput}(string)" /> pattern,
    /// except it returns the specified value instead of the pattern's input.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TValue> Return<TInput, TValue>(Task<TValue> value, string description) =>
        CreatePattern<TInput, TValue>(
            async _ => MatchResult.Success(await value),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which always successfully returns the provided value, discarding its input value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TValue">The type of the value to return.</typeparam>
    /// <param name="valueProvider">The provider of the value to return.</param>
    /// <returns>A pattern which always successfully returns the provided value.</returns>
    /// <remarks>
    /// This pattern is much like the <see cref="Any{TInput}()" /> pattern,
    /// except it returns the provided value instead of the pattern's input.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider"/> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TValue> Return<TInput, TValue>(Func<Task<TValue>> valueProvider) =>
        Return<TInput, TValue>(valueProvider, DefaultReturnDescription);

    /// <summary>
    /// Returns a pattern which always successfully returns the provided value, discarding its input value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TValue">The type of the value to return.</typeparam>
    /// <param name="valueProvider">The provider of the value to return.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>A pattern which always successfully returns the provided value.</returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="Pattern{TInput, TMatchResult}.Match(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// This pattern is much like the <see cref="Any{TInput}(string)" /> pattern,
    /// except it returns the provided value instead of the pattern's input.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TValue> Return<TInput, TValue>(
        Func<Task<TValue>> valueProvider,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));
        return CreatePattern<TInput, TValue>(
            async _ => MatchResult.Success(await memoizedProvider()),
            description ?? throw new ArgumentNullException(nameof(description)));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
    public static IAsyncPattern<TInput, TInput> Null<TInput>()
        where TInput : class =>
        Null<TInput>(DefaultNullDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> Null<TInput>(string description)
        where TInput : class =>
        CreatePattern<TInput>(
            input => Task.FromResult(input == null),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
    public static IAsyncPattern<TInput?, TInput?> ValueNull<TInput>()
        where TInput : struct =>
        ValueNull<TInput>(DefaultNullDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput?, TInput?> ValueNull<TInput>(string description)
        where TInput : struct =>
        CreatePattern<TInput?>(
            input => Task.FromResult(input == null),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to check for equality.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(Task<TInput> value) =>
        EqualTo(value, DefaultEqualToDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to check for equality.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(Func<Task<TInput>> valueProvider) =>
        EqualTo(valueProvider, DefaultEqualToDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the specified value
    /// according to the specified equality comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to check for equality.</param>
    /// <param name="comparer">The equality comparer to use for checking equality.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(
        Task<TInput> value,
        IEqualityComparer<TInput> comparer) =>
        EqualTo(value, comparer, DefaultEqualToDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the provided value
    /// according to the specified equality comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to check for equality.</param>
    /// <param name="comparer">The equality comparer to use for checking equality.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(
        Func<Task<TInput>> valueProvider,
        IEqualityComparer<TInput> comparer) =>
        EqualTo(valueProvider, comparer, DefaultEqualToDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to check for equality.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(Task<TInput> value, string description) =>
        EqualTo(value, EqualityComparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to check for equality.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(
        Func<Task<TInput>> valueProvider,
        string description) =>
        EqualTo(valueProvider, EqualityComparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the specified value
    /// according to the specified equality comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to check for equality.</param>
    /// <param name="comparer">The equality comparer to use for checking equality.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" />, <paramref name="comparer" /> or <paramref name="description" /> is
    /// <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(
        Task<TInput> value,
        IEqualityComparer<TInput> comparer,
        string description) =>
        value != null
            ? comparer != null
                ? CreatePattern<TInput>(
                    async input => comparer.Equals(input, await value),
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer))
            : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is equal to the provided value
    /// according to the specified equality comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to check for equality.</param>
    /// <param name="comparer">The equality comparer to use for checking equality.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> EqualTo<TInput>(
        Func<Task<TInput>> valueProvider,
        IEqualityComparer<TInput> comparer,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));

        return comparer != null
            ? CreatePattern<TInput>(
                async input => comparer.Equals(input, await memoizedProvider()),
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(Task<TInput> value)
        where TInput : IComparable<TInput> =>
        LessThan(value, DefaultLessThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(Func<Task<TInput>> valueProvider)
        where TInput : IComparable<TInput> =>
        LessThan(valueProvider, DefaultLessThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the specified value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(Task<TInput> value, IComparer<TInput> comparer) =>
        LessThan(value, comparer, DefaultLessThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the provided value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer) =>
        LessThan(valueProvider, comparer, DefaultLessThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(Task<TInput> value, string description)
        where TInput : IComparable<TInput> =>
        LessThan(value, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(
        Func<Task<TInput>> valueProvider,
        string description)
        where TInput : IComparable<TInput> =>
        LessThan(valueProvider, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the specified value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" />, <paramref name="comparer" /> or <paramref name="description" /> is
    /// <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(
        Task<TInput> value,
        IComparer<TInput> comparer,
        string description) =>
        value != null
            ? comparer != null
                ? CreatePattern<TInput>(
                    async input => comparer.Compare(input, await value) < 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer))
            : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than the provided value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessThan<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));

        return comparer != null
            ? CreatePattern<TInput>(
                async input => comparer.Compare(input, await memoizedProvider()) < 0,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(Task<TInput> value)
        where TInput : IComparable<TInput> =>
        LessOrEqual(value, DefaultLessOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(Func<Task<TInput>> valueProvider)
        where TInput : IComparable<TInput> =>
        LessOrEqual(valueProvider, DefaultLessOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the specified value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(Task<TInput> value, IComparer<TInput> comparer) =>
        LessOrEqual(value, comparer, DefaultLessOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the provided value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer) =>
        LessOrEqual(valueProvider, comparer, DefaultLessOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(Task<TInput> value, string description)
        where TInput : IComparable<TInput> =>
        LessOrEqual(value, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        string description)
        where TInput : IComparable<TInput> =>
        LessOrEqual(valueProvider, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the specified value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" />, <paramref name="comparer" /> or <paramref name="description" /> is
    /// <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(
        Task<TInput> value,
        IComparer<TInput> comparer,
        string description) =>
        value != null
            ? comparer != null
                ? CreatePattern<TInput>(
                    async input => comparer.Compare(input, await value) <= 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer))
            : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is less than or equal
    /// to the provided value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> LessOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));

        return comparer != null
            ? CreatePattern<TInput>(
                async input => comparer.Compare(input, await memoizedProvider()) <= 0,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(Task<TInput> value)
        where TInput : IComparable<TInput> =>
        GreaterThan(value, DefaultGreaterThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(Func<Task<TInput>> valueProvider)
        where TInput : IComparable<TInput> =>
        GreaterThan(valueProvider, DefaultGreaterThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the specified value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(Task<TInput> value, IComparer<TInput> comparer) =>
        GreaterThan(value, comparer, DefaultGreaterThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the provided value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer) =>
        GreaterThan(valueProvider, comparer, DefaultGreaterThanDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(Task<TInput> value, string description)
        where TInput : IComparable<TInput> =>
        GreaterThan(value, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(
        Func<Task<TInput>> valueProvider,
        string description)
        where TInput : IComparable<TInput> =>
        GreaterThan(valueProvider, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the specified value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" />, <paramref name="comparer" /> or <paramref name="description" /> is
    /// <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(
        Task<TInput> value,
        IComparer<TInput> comparer,
        string description) =>
        value != null
            ? comparer != null
                ? CreatePattern<TInput>(
                    async input => comparer.Compare(input, await value) > 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer))
            : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than the provided value
    /// according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterThan<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));

        return comparer != null
            ? CreatePattern<TInput>(
                async input => comparer.Compare(input, await memoizedProvider()) > 0,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(Task<TInput> value)
        where TInput : IComparable<TInput> =>
        GreaterOrEqual(value, DefaultGreaterOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(Func<Task<TInput>> valueProvider)
        where TInput : IComparable<TInput> =>
        GreaterOrEqual(valueProvider, DefaultGreaterOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the specified value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(
        Task<TInput> value,
        IComparer<TInput> comparer) =>
        GreaterOrEqual(value, comparer, DefaultGreaterOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the provided value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer) =>
        GreaterOrEqual(valueProvider, comparer, DefaultGreaterOrEqualDescription);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the specified value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(Task<TInput> value, string description)
        where TInput : IComparable<TInput> =>
        GreaterOrEqual(value, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the provided value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        string description)
        where TInput : IComparable<TInput> =>
        GreaterOrEqual(valueProvider, Comparer<TInput>.Default, description);

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the specified value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="value">The value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the specified value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" />, <paramref name="comparer" /> or <paramref name="description" /> is
    /// <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(
        Task<TInput> value,
        IComparer<TInput> comparer,
        string description) =>
        value != null
            ? comparer != null
                ? CreatePattern<TInput>(
                    async input => comparer.Compare(input, await value) >= 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer))
            : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is greater than or equal
    /// to the provided value according to the specified comparer.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <param name="valueProvider">The provider of the value to compare with.</param>
    /// <param name="comparer">The comparer to use for comparison.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is greater than or equal to
    /// the provided value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="valueProvider" /> is not called until this pattern's
    /// <see cref="IAsyncPattern{TInput, TMatchResult}.MatchAsync(TInput)" /> method is called.
    /// </para>
    /// <para>
    /// The <paramref name="valueProvider" /> will be memoized, so it will be called once,
    /// and then its result will be cached. The caching process is not thread-safe,
    /// so there is a chance that the <paramref name="valueProvider" /> can be called
    /// more than once.
    /// </para>
    /// <para>
    /// If <paramref name="valueProvider" /> returns <see langword="null" />, an
    /// <see cref="InvalidOperationException" /> will be thrown.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
    /// is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> GreaterOrEqual<TInput>(
        Func<Task<TInput>> valueProvider,
        IComparer<TInput> comparer,
        string description)
    {
        var memoizedProvider = Memoize(valueProvider ?? throw new ArgumentNullException(nameof(valueProvider)));

        return comparer != null
            ? CreatePattern<TInput>(
                async input => comparer.Compare(input, await memoizedProvider()) >= 0,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is of the specified type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TType">The type to check for.</typeparam>
    /// <returns>
    /// A pattern which is matched successfully when the input value is of the specified type.
    /// </returns>
    /// <remarks>
    /// If the input is <see langword="null" />, then this pattern fails only when <typeparamref name="TType"/>
    /// is a non-nullable value type.
    /// </remarks>
    public static IAsyncPattern<TInput, TType> Type<TInput, TType>()
        where TType : TInput =>
        Type<TInput, TType>(String.Format(DefaultTypeDescriptionFormat, typeof(TType)));

    /// <summary>
    /// Returns a pattern which is matched successfully when the input value is of the specified type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TType">The type to check for.</typeparam>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the input value is of the specified type.
    /// </returns>
    /// <remarks>
    /// If the input is <see langword="null" />, then this pattern fails only when <typeparamref name="TType"/>
    /// is a non-nullable value type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TType> Type<TInput, TType>(string description)
        where TType : TInput =>
        CreatePattern<TInput, TType>(
            input => Task.FromResult(input is null && default(TType) is null
                ? MatchResult.Success<TType>(default)
                : (input is TType result ? MatchResult.Success(result) : MatchResult.Failure<TType>())),
            description ?? throw new ArgumentNullException(nameof(description)));

    /// <summary>
    /// Returns a pattern which is matched successfully when the specified pattern is not matched successfully.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern to invert.</param>
    /// <returns>
    /// A pattern which is matched successfully when the specified pattern is not matched successfully.
    /// </returns>
    /// <remarks>
    /// This pattern ignores the specified pattern's transformation
    /// and returns the input value if matched successfully.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> Not<TInput, TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern) =>
        pattern != null
            ? Not(
                pattern,
                pattern.Description.Length != 0
                    ? String.Format(DefaultNotDescriptionFormat, pattern.Description)
                    : String.Empty)
            : throw new ArgumentNullException(nameof(pattern));

    /// <summary>
    /// Returns a pattern which is matched successfully when the specified pattern is not matched successfully.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern to invert.</param>
    /// <param name="description">The description of the pattern.</param>
    /// <returns>
    /// A pattern which is matched successfully when the specified pattern is not matched successfully.
    /// </returns>
    /// <remarks>
    /// This pattern ignores the specified pattern's transformation
    /// and returns the input value if matched successfully.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern" /> or <paramref name="description" /> is <see langword="null" />.
    /// </exception>
    public static IAsyncPattern<TInput, TInput> Not<TInput, TMatchResult>(
        IAsyncPattern<TInput, TMatchResult> pattern,
        string description) =>
        pattern != null
            ? CreatePattern<TInput>(
                async input => !(await pattern.MatchAndThrowIfNull(input)).IsSuccessful,
                description ?? throw new ArgumentNullException(nameof(description)))
            : throw new ArgumentNullException(nameof(pattern));

    /// <summary>
    /// Matches the specified pattern and throws an <see cref="InvalidOperationException" />
    /// if the returned <see cref="Task{TResult}" /> is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="input">The input value to match against the pattern.</param>
    /// <returns>The pattern's result.</returns>
    /// <exception cref="InvalidOperationException">
    /// The pattern's match returned <see langword="null" />.
    /// </exception>
    internal static Task<MatchResult<TMatchResult>> MatchAndThrowIfNull<TInput, TMatchResult>(
        this IAsyncPattern<TInput, TMatchResult> pattern,
        TInput input) =>
        pattern.MatchAsync(input) ?? throw new InvalidOperationException("Task returned from a pattern is null");

    /// <summary>
    /// Memoizes the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the function's result.</typeparam>
    /// <param name="function">The function to memoize.</param>
    /// <returns>The memoized version of the specified function.</returns>
    private static Func<Task<T>> Memoize<T>(Func<Task<T>> function)
    {
        T? result = default;
        bool isMemoized = false;

        return async () =>
        {
            if (!isMemoized)
            {
                result = await (function() ??
                    throw new InvalidOperationException("Task returned from a provider function is null"));
                isMemoized = true;
            }

            return result!;
        };
    }
}
