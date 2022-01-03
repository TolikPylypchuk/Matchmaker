namespace Matchmaker.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// A general container class for extension methods.
/// </summary>
public static class MatchExtensions
{
    /// <summary>
    /// Forces the enumeration of an <see cref="IEnumerable{T}" /> and ignores the result.
    /// </summary>
    /// <typeparam name="T">The type of the items in the enumerate.</typeparam>
    /// <param name="items">The enumerable to enumerate.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items" /> is <see langword="null" />.
    /// </exception>
    public static void Enumerate<T>(this IEnumerable<T> items)
    {
        foreach (var _ in items ?? throw new ArgumentNullException(nameof(items))) { }
    }

    /// <summary>
    /// Forces the enumeration of an <see cref="IAsyncEnumerable{T}" /> asynchronously and ignores the result.
    /// </summary>
    /// <typeparam name="T">The type of the items in the enumerate.</typeparam>
    /// <param name="items">The async enumerable to enumerate.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items" /> is <see langword="null" />.
    /// </exception>
    public static Task EnumerateAsync<T>(this IAsyncEnumerable<T> items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        async Task SafeEnumerateAsync()
        {
            await foreach (var _ in items) { }
        }

        return SafeEnumerateAsync();
    }
}
