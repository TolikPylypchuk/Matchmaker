using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Matchmaker.Linq
{
    /// <summary>
    /// A container class for extension methods for <see cref="MatchResult{T}" />.
    /// </summary>
    /// <seealso cref="MatchResult{T}" />
    /// <seealso cref="MatchResult" />
    public static class MatchResultExtensions
    {
        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be returned.</param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        [return: MaybeNull]
        public static T GetValueOrDefault<T>(this MatchResult<T> result)
            => result.GetValueOrDefault(default(T));

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be returned.</param>
        /// <param name="defaultValue">The value to return if the result is not successful.</param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        [return: MaybeNull]
        public static T GetValueOrDefault<T>(this MatchResult<T> result, [AllowNull] T defaultValue)
            => result.IsSuccessful ? result.Value : defaultValue;

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be returned.</param>
        /// <param name="defaultValueProvider">
        /// The function which provides the value to return if the result is not successful.
        /// </param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="defaultValueProvider" /> is <see langword="null" />.
        /// </exception>
        [return: MaybeNull]
        public static T GetValueOrDefault<T>(this MatchResult<T> result, Func<T> defaultValueProvider)
            => defaultValueProvider != null
                ? result.IsSuccessful ? result.Value : defaultValueProvider()
                : throw new ArgumentNullException(nameof(defaultValueProvider));

        /// <summary>
        /// Returns the result's value if it's successful, or throws the provided exception otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be returned.</param>
        /// <param name="exceptionProvider">
        /// The function which provides the exception to throw if the result is not successful.
        /// </param>
        /// <returns>The result's value if it's successful.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exceptionProvider" /> is <see langword="null" />.
        /// </exception>
        [return: MaybeNull]
        public static T GetValueOrThrow<T>(this MatchResult<T> result, Func<Exception> exceptionProvider)
            => exceptionProvider != null
                ? result.IsSuccessful ? result.Value : throw exceptionProvider()
                : throw new ArgumentNullException(nameof(exceptionProvider));

        /// <summary>
        /// Returns a result which contains a mapped value if the specified result is successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="result">The result whose value should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        /// <returns>
        /// A result which contains a mapped value if the specified result is successful. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="mapper" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mapper" /> is <see langword="null" />.
        /// </exception>
        public static MatchResult<TResult> Select<T, TResult>(this MatchResult<T> result, Func<T, TResult> mapper)
            => mapper != null
                ? result.IsSuccessful ? MatchResult.Success(mapper(result.Value!)) : MatchResult.Failure<TResult>()
                : throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Returns a flat-mapped result if the specified result is successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="result">The result whose value should be flat-mapped.</param>
        /// <param name="binder">The result binding function.</param>
        /// <returns>
        /// A flat-mapped result if the specified result is successful. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="binder" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="binder" /> is <see langword="null" />.
        /// </exception>
        public static MatchResult<TResult> Bind<T, TResult>(
            this MatchResult<T> result,
            Func<T, MatchResult<TResult>> binder)
            => binder != null
                ? result.IsSuccessful ? binder(result.Value!) : MatchResult.Failure<TResult>()
                : throw new ArgumentNullException(nameof(binder));

        /// <summary>
        /// Returns a successful result only if the specified result is successful
        /// and its value satisfies a specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be checked.</param>
        /// <param name="predicate">The predicate to use to check the value.</param>
        /// <returns>
        /// A successfult result if the specified result is successful and its value satisfies a specified predicate.
        /// Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="predicate" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static MatchResult<T> Where<T>(this MatchResult<T> result, Func<T, bool> predicate)
            => predicate != null
                ? result.IsSuccessful && predicate(result.Value!) ? result : MatchResult.Failure<T>()
                : throw new ArgumentNullException(nameof(predicate));

        /// <summary>
        /// Returns a successful result if the specified result is successful
        /// and contains a value that can be cast to <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="result">The result whose value should be cast.</param>
        /// <returns>
        /// A successful result if the specified result is successful and contains a value that can be cast to
        /// <typeparamref name="TResult" />. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// If the result contains <see langword="null" />, then this method returns a failed result only when
        /// <typeparamref name="TResult" /> is a non-nullable value type.
        /// </remarks>
        public static MatchResult<TResult> Cast<T, TResult>(this MatchResult<T> result)
            where TResult : T
            => result.Bind(value =>
                value is null && default(TResult) is null
                    ? MatchResult.Success<TResult>(default)
                    : value is TResult actualValue ? MatchResult.Success(actualValue) : MatchResult.Failure<TResult>());

        /// <summary>
        /// Performs a specified action on the result's value if it's successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="result">The result whose value should be passed to the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>The result for which the action should be executed.</returns>
        /// <remarks>
        /// The <paramref name="action" />'s parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public static MatchResult<T> Do<T>(this MatchResult<T> result, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (result.IsSuccessful)
            {
                action(result.Value!);
            }

            return result;
        }

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be mapped.</param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        /// <remarks>
        /// The resulting <see cref="Task{TResult}" /> may contain <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> is <see langword="null" />.
        /// </exception>
        public static async Task<T> GetValueOrDefault<T>(this Task<MatchResult<T>> futureResult)
        {
            var result = await (futureResult ?? throw new ArgumentNullException(nameof(futureResult)));
            return result.GetValueOrDefault(default(T))!;
        }

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be returned.</param>
        /// <param name="defaultValue">The value to return if the result is not successful.</param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        /// <remarks>
        /// The resulting <see cref="Task{TResult}" /> may contain <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> is <see langword="null" />.
        /// </exception>
        public static async Task<T> GetValueOrDefault<T>(
            this Task<MatchResult<T>> futureResult,
            [AllowNull] T defaultValue)
        {
            var result = await(futureResult ?? throw new ArgumentNullException(nameof(futureResult)));
            return result.IsSuccessful ? result.Value! : defaultValue!;
        }

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be mapped.</param>
        /// <param name="defaultValueProvider">
        /// The function which provides the value to return if the result is not successful.
        /// </param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        /// <remarks>
        /// The resulting <see cref="Task{TResult}" /> may contain <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="defaultValueProvider" /> is <see langword="null" />.
        /// </exception>
        public static async Task<T> GetValueOrDefault<T>(
            this Task<MatchResult<T>> futureResult,
            Func<T> defaultValueProvider)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (defaultValueProvider == null)
            {
                throw new ArgumentNullException(nameof(defaultValueProvider));
            }

            var result = await futureResult;

            return result.IsSuccessful ? result.Value! : defaultValueProvider();
        }

        /// <summary>
        /// Returns the result's value if it's successful, or the default one otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be mapped.</param>
        /// <param name="defaultValueProvider">
        /// The function which provides the value to return if the result is not successful.
        /// </param>
        /// <returns>
        /// The result's value if it's successful, or the default one otherwise.
        /// </returns>
        /// <remarks>
        /// The resulting <see cref="Task{TResult}" /> may contain <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="defaultValueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="defaultValueProvider" /> returned <see langword="null" />.
        /// </exception>
        public static async Task<T> GetValueOrDefault<T>(
            this Task<MatchResult<T>> futureResult,
            Func<Task<T>> defaultValueProvider)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (defaultValueProvider == null)
            {
                throw new ArgumentNullException(nameof(defaultValueProvider));
            }

            var result = await futureResult;

            return result.IsSuccessful
                ? result.Value!
                : await defaultValueProvider.CallAndThrowIfNull();
        }

        /// <summary>
        /// Returns the result's value if it's successful, or throws the provided exception otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be mapped.</param>
        /// <param name="exceptionProvider">
        /// The function which provides the exception to throw if the result is not successful.
        /// </param>
        /// <returns>The result's value if it's successful.</returns>
        /// <remarks>
        /// The resulting <see cref="Task{TResult}" /> may contain <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="exceptionProvider" /> is <see langword="null" />.
        /// </exception>
        public static async Task<T> GetValueOrThrow<T>(
            this Task<MatchResult<T>> futureResult,
            Func<Exception> exceptionProvider)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (exceptionProvider == null)
            {
                throw new ArgumentNullException(nameof(exceptionProvider));
            }

            var result = await futureResult;

            return result.IsSuccessful ? result.Value! : throw exceptionProvider();
        }

        /// <summary>
        /// Returns a result which contains a mapped value if the specified result is successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be mapped.</param>
        /// <param name="mapper">The result mapping function.</param>
        /// <returns>
        /// A result which contains a mapped value if the specified result is successful. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="mapper" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="mapper" /> is <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<TResult>> Select<T, TResult>(
            this Task<MatchResult<T>> futureResult,
            Func<T, TResult> mapper)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return (await futureResult).Select(mapper);
        }

        /// <summary>
        /// Returns a flat-mapped result if the specified result is successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be flat-mapped.</param>
        /// <param name="binder">The result binding function.</param>
        /// <returns>
        /// A flat-mapped result if the specified result is successful. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="binder" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="binder" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="binder" /> returned <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<TResult>> Bind<T, TResult>(
            this Task<MatchResult<T>> futureResult,
            Func<T, Task<MatchResult<TResult>>> binder)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            var result = await futureResult;

            return result.IsSuccessful
                ? await binder.CallAndThrowIfNull(result.Value!)
                : MatchResult.Failure<TResult>();
        }

        /// <summary>
        /// Returns a successful result only if the specified result is successful
        /// and its value satisfies a specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be checked.</param>
        /// <param name="predicate">The predicate to use to check the value.</param>
        /// <returns>
        /// A successfult result if the specified result is successful and its value satisfies a specified predicate.
        /// Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="predicate" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<T>> Where<T>(
            this Task<MatchResult<T>> futureResult,
            Func<T, bool> predicate)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (await futureResult).Where(predicate);
        }

        /// <summary>
        /// Returns a successful result only if the specified result is successful
        /// and its value satisfies a specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be checked.</param>
        /// <param name="predicate">The predicate to use to check the value.</param>
        /// <returns>
        /// A successfult result if the specified result is successful and its value satisfies a specified predicate.
        /// Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// The <paramref name="predicate" /> function's parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="predicate" /> returned <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<T>> Where<T>(
            this Task<MatchResult<T>> futureResult,
            Func<T, Task<bool>> predicate)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = await futureResult;

            return result.IsSuccessful && await predicate.CallAndThrowIfNull(result.Value!)
                ? result
                : MatchResult.Failure<T>();
        }

        /// <summary>
        /// Returns a successful result if the specified result is successful
        /// and contains a value that can be cast to <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <typeparam name="TResult">The type of the returned result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be cast.</param>
        /// <returns>
        /// A successful result if the specified result is successful and contains a value that can be cast to
        /// <typeparamref name="TResult" />. Otherwise, a failed result.
        /// </returns>
        /// <remarks>
        /// If the result contains <see langword="null" />, then this method returns a failed result only when
        /// <typeparamref name="TResult" /> is a non-nullable value type.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> is <see langword="null" />.
        /// </exception>
        public static Task<MatchResult<TResult>> Cast<T, TResult>(this Task<MatchResult<T>> futureResult)
            where TResult : T
            => futureResult.Bind(value =>
                Task.FromResult(value is null && default(TResult) is null
                    ? MatchResult.Success<TResult>(default)
                    : value is TResult actualValue
                        ? MatchResult.Success(actualValue)
                        : MatchResult.Failure<TResult>()));

        /// <summary>
        /// Performs a specified action on the result's value if it's successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be passed to the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>The result for which the action should be executed.</returns>
        /// <remarks>
        /// The <paramref name="action" />'s parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<T>> Do<T>(this Task<MatchResult<T>> futureResult, Action<T> action)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var result = await futureResult;

            if (result.IsSuccessful)
            {
                action(result.Value!);
            }

            return result;
        }

        /// <summary>
        /// Performs a specified action on the result's value if it's successful.
        /// </summary>
        /// <typeparam name="T">The type of the result's value.</typeparam>
        /// <param name="futureResult">The result whose value should be passed to the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>The result for which the action should be executed.</returns>
        /// <remarks>
        /// The <paramref name="action" />'s parameter may be <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="futureResult" /> or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="action" /> returned <see langword="null" />.
        /// </exception>
        public static async Task<MatchResult<T>> Do<T>(this Task<MatchResult<T>> futureResult, Func<T, Task> action)
        {
            if (futureResult == null)
            {
                throw new ArgumentNullException(nameof(futureResult));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var result = await futureResult;

            if (result.IsSuccessful)
            {
                await action.CallAndThrowIfNull(result.Value!);
            }

            return result;
        }

        /// <summary>
        /// Calls the specified function, but throws <see cref="InvalidOperationException" /> if it returnes
        /// <see langword="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the function's input.</typeparam>
        /// <param name="func">The function to call.</param>
        /// <param name="input">The function's input value.</param>
        /// <returns>The specified function's result.</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="func" /> returned <see langword="null" />.
        /// </exception>
        private static Task CallAndThrowIfNull<T>(this Func<T, Task> func, T input)
            => func(input) ?? throw new InvalidOperationException("The function returned null");

        /// <summary>
        /// Calls the specified function, but throws <see cref="InvalidOperationException" /> if it returnes
        /// <see langword="null" />.
        /// </summary>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="func">The function to call.</param>
        /// <returns>The specified function's result.</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="func" /> returned <see langword="null" />.
        /// </exception>
        private static Task<TResult> CallAndThrowIfNull<TResult>(this Func<Task<TResult>> func)
            => func() ?? throw new InvalidOperationException("The function returned null");

        /// <summary>
        /// Calls the specified function, but throws <see cref="InvalidOperationException" /> if it returnes
        /// <see langword="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the function's input.</typeparam>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="func">The function to call.</param>
        /// <param name="input">The function's input value.</param>
        /// <returns>The specified function's result.</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="func" /> returned <see langword="null" />.
        /// </exception>
        private static Task<TResult> CallAndThrowIfNull<T, TResult>(this Func<T, Task<TResult>> func, T input)
            => func(input) ?? throw new InvalidOperationException("The function returned null");
    }
}
