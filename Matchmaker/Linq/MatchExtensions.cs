using System;
using System.Collections.Generic;

namespace Matchmaker.Linq
{
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
    }
}
