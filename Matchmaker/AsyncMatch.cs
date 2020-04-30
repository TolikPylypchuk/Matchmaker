using System;
using System.Runtime.CompilerServices;

namespace Matchmaker
{
    /// <summary>
    /// A static class which is used to create asynchronous match expressions.
    /// </summary>
    /// <seealso cref="AsyncMatch{TInput, TOutput}" />
    /// <seealso cref="AsyncMatch{TInput}" />
    public static class AsyncMatch
    {
        /// <summary>
        /// Creates an asynchronous match expression.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <returns>
        /// A match expression which specifies the patterns to match with and functions which are executed.
        /// </returns>
        /// <seealso cref="Create{TInput, TOutput}(bool)" />
        public static AsyncMatch<TInput, TOutput> Create<TInput, TOutput>()
            => new AsyncMatch<TInput, TOutput>(fallthroughByDefault: false);

        /// <summary>
        /// Creates an asynchronous match expression.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        /// <returns>
        /// A match expression which specifies the patterns to match with and functions which are executed.
        /// </returns>
        /// <seealso cref="Create{TInput, TOutput}()" />
        public static AsyncMatch<TInput, TOutput> Create<TInput, TOutput>(bool fallthroughByDefault)
            => new AsyncMatch<TInput, TOutput>(fallthroughByDefault);

        /// <summary>
        /// Creates an asynchronous match statement.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <returns>
        /// A match statement which specifies the patterns to match with and actions which are executed.
        /// </returns>
        /// <seealso cref="Create{TInput}(bool)" />
        public static AsyncMatch<TInput> Create<TInput>()
            => new AsyncMatch<TInput>(fallthroughByDefault: false);

        /// <summary>
        /// Creates an asynchronous match statement with the specified default fallthrough behaviour.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        /// <returns>
        /// A match statement which specifies the patterns to match with and actions which are executed.
        /// </returns>
        /// <seealso cref="Create{TInput}()" />
        public static AsyncMatch<TInput> Create<TInput>(bool fallthroughByDefault)
            => new AsyncMatch<TInput>(fallthroughByDefault);

        /// <summary>
        /// Creates an asynchronous match expression which is then globally cached.
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
        /// The pattern caching process is thread-safe.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buildAction" /> or <paramref name="sourceFilePath" /> is <see langword="null" />.
        /// </exception>
        public static AsyncMatch<TInput, TOutput> CreateStatic<TInput, TOutput>(
            Action<AsyncMatchBuilder<TInput, TOutput>> buildAction,
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

            return AsyncMatch<TInput, TOutput>.Cache.GetOrAdd(key, k =>
            {
                var builder = new AsyncMatchBuilder<TInput, TOutput>();
                buildAction(builder);
                return builder.Build();
            });
        }

        /// <summary>
        /// Creates an asynchronous match statement which is then globally cached.
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
        /// The pattern caching process is thread-safe.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buildAction" /> or <paramref name="sourceFilePath" /> is <see langword="null" />.
        /// </exception>
        public static AsyncMatch<TInput> CreateStatic<TInput>(
            Action<AsyncMatchBuilder<TInput>> buildAction,
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

            return AsyncMatch<TInput>.Cache.GetOrAdd(key, k =>
            {
                var builder = new AsyncMatchBuilder<TInput>();
                buildAction(builder);
                return builder.Build();
            });
        }

        /// <summary>
        /// Clears the global cache of static <see cref="AsyncMatch{TInput, TOutput}" /> instances.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <remarks>
        /// <para>
        /// After calling this method all
        /// <see cref="CreateStatic{TInput, TOutput}(Action{AsyncMatchBuilder{TInput, TOutput}}, string, int)" />
        /// calls will recalculate match expressions for types <typeparamref name="TInput" /> and
        /// <typeparamref name="TOutput" />.
        /// </para>
        /// <para>
        /// Clearing the cache is thread-safe.
        /// </para>
        /// </remarks>
        public static void ClearCache<TInput, TOutput>()
            => AsyncMatch<TInput, TOutput>.Cache.Clear();

        /// <summary>
        /// Clears the global cache of static <see cref="AsyncMatch{TInput}" /> instances.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <remarks>
        /// <para>
        /// After calling this method all
        /// <see cref="CreateStatic{TInput}(Action{AsyncMatchBuilder{TInput}}, string, int)" />
        /// calls will recalculate match statements for type <typeparamref name="TInput" />.
        /// </para>
        /// <para>
        /// Clearing the cache is thread-safe.
        /// </para>
        /// </remarks>
        public static void ClearCache<TInput>()
            => AsyncMatch<TInput>.Cache.Clear();
    }
}
