namespace Matchmaker;

/// <summary>
/// Contains extension methods for internal use.
/// </summary>
internal static class InternalExtensions
{
    /// <summary>
    /// Returns a pseudo-asynchronous function which executes the specified action.
    /// </summary>
    /// <typeparam name="T">The type of the action's parameter.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <returns>A pseudo-asynchronous function which executes the specified action.</returns>
    internal static Func<T, Task> AsAsync<T>(this Action<T> action) =>
        action != null
            ? value =>
            {
                action(value);
                return Task.CompletedTask;
            }
            : throw new ArgumentNullException(nameof(action));

    /// <summary>
    /// Returns a pseudo-asynchronous function which executes the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the function's parameter.</typeparam>
    /// <typeparam name="TResult">The type of the function's result.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>A pseudo-asynchronous function which executes the specified function.</returns>
    internal static Func<T, Task<TResult>> AsAsync<T, TResult>(this Func<T, TResult> func) =>
        func != null
            ? value => Task.FromResult(func(value))
            : throw new ArgumentNullException(nameof(func));
}
