using System.Runtime.CompilerServices;

namespace Matchmaker;

/// <summary>
/// A static class which is used to create match expressions.
/// </summary>
/// <seealso cref="Match{TInput, TOutput}" />
/// <seealso cref="Match{TInput}" />
/// <seealso cref="MatchBuilder{TInput, TOutput}" />
/// <seealso cref="MatchBuilder{TInput}" />
public static class Match
{
    /// <summary>
    /// Creates a match expression.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the result.</typeparam>
    /// <returns>
    /// A match expression which specifies the patterns to match with and functions which are executed.
    /// </returns>
    public static Match<TInput, TOutput> Create<TInput, TOutput>() =>
        new(fallthroughByDefault: false);

    /// <summary>
    /// Creates a match expression.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the result.</typeparam>
    /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
    /// <returns>
    /// A match expression which specifies the patterns to match with and functions which are executed.
    /// </returns>
    public static Match<TInput, TOutput> Create<TInput, TOutput>(bool fallthroughByDefault) =>
        new(fallthroughByDefault);

    /// <summary>
    /// Creates a match statement.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <returns>
    /// A match statement which specifies the patterns to match with and actions which are executed.
    /// </returns>
    public static Match<TInput> Create<TInput>() =>
        new(fallthroughByDefault: false);

    /// <summary>
    /// Creates a match statement with the specified default fallthrough behaviour.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
    /// <returns>
    /// A match statement which specifies the patterns to match with and actions which are executed.
    /// </returns>
    public static Match<TInput> Create<TInput>(bool fallthroughByDefault) =>
        new(fallthroughByDefault);

    /// <summary>
    /// Creates a match expression which is then globally cached.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the result.</typeparam>
    /// <param name="buildAction">The action which builds the match expression.</param>
    /// <param name="sourceFilePath">The path to the source file in which this method is called.</param>
    /// <param name="sourceLineNumber">The line in the source file where this method is called.</param>
    /// <returns>The globally cached match expression.</returns>
    /// <remarks>
    /// <para>
    /// The key of the cache is the location of this method's call in the source code.
    /// This way, a new match expression object will not be allocated every time pattern matching occurs
    /// in the same place more than once.
    /// </para>
    /// <para>
    /// The pattern caching process is not thread-safe.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buildAction" /> or <paramref name="sourceFilePath" /> is <see langword="null" />.
    /// </exception>
    public static Match<TInput, TOutput> CreateStatic<TInput, TOutput>(
        Action<MatchBuilder<TInput, TOutput>> buildAction,
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (buildAction == null)
        {
            throw new ArgumentNullException(nameof(buildAction));
        }

        if (sourceFilePath == null)
        {
            throw new ArgumentNullException(nameof(sourceFilePath));
        }

        string key = $"{sourceFilePath}:{sourceLineNumber}";

        if (Match<TInput, TOutput>.Cache.TryGetValue(key, out var match))
        {
            return match;
        }

        var builder = new MatchBuilder<TInput, TOutput>();
        buildAction(builder);

        match = builder.Build();

        Match<TInput, TOutput>.Cache.Add(key, match);

        return match;
    }

    /// <summary>
    /// Creates a match statement which is then globally cached.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <param name="buildAction">The action which builds the match statement.</param>
    /// <param name="sourceFilePath">The path to the source file in which this method is called.</param>
    /// <param name="sourceLineNumber">The line in the source file where this method is called.</param>
    /// <returns>The globally cached match expression.</returns>
    /// <remarks>
    /// <para>
    /// The key of the cache is the location of this method's call in the source code.
    /// This way, a new match statement object will not be allocated every time pattern matching occurs
    /// in the same place more than once.
    /// </para>
    /// <para>
    /// The pattern caching process is not thread-safe.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buildAction" /> or <paramref name="sourceFilePath" /> is <see langword="null" />.
    /// </exception>
    public static Match<TInput> CreateStatic<TInput>(
        Action<MatchBuilder<TInput>> buildAction,
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (buildAction == null)
        {
            throw new ArgumentNullException(nameof(buildAction));
        }

        if (sourceFilePath == null)
        {
            throw new ArgumentNullException(nameof(sourceFilePath));
        }

        string key = $"{sourceFilePath}:{sourceLineNumber}";

        if (Match<TInput>.Cache.TryGetValue(key, out var match))
        {
            return match;
        }

        var builder = new MatchBuilder<TInput>();
        buildAction(builder);

        match = builder.Build();

        Match<TInput>.Cache.Add(key, match);

        return match;
    }

    /// <summary>
    /// Clears the global cache of static <see cref="Match{TInput, TOutput}" /> instances.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the result.</typeparam>
    /// <remarks>
    /// <para>
    /// After calling this method all
    /// <see cref="CreateStatic{TInput, TOutput}(Action{MatchBuilder{TInput, TOutput}}, string, int)" />
    /// calls will recalculate match expressions for types <typeparamref name="TInput" /> and
    /// <typeparamref name="TOutput" />.
    /// </para>
    /// <para>
    /// Clearing the cache is not thread-safe.
    /// </para>
    /// </remarks>
    public static void ClearCache<TInput, TOutput>() =>
        Match<TInput, TOutput>.Cache.Clear();

    /// <summary>
    /// Clears the global cache of static <see cref="Match{TInput}" /> instances.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <remarks>
    /// <para>
    /// After calling this method all <see cref="CreateStatic{TInput}(Action{MatchBuilder{TInput}}, string, int)" />
    /// calls will recalculate match statements for type <typeparamref name="TInput" />.
    /// </para>
    /// <para>
    /// Clearing the cache is not thread-safe.
    /// </para>
    /// </remarks>
    public static void ClearCache<TInput>() =>
        Match<TInput>.Cache.Clear();
}
