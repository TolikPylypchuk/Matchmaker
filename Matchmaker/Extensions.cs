using System.Collections.Generic;

namespace Matchmaker
{
    /// <summary>
    /// A container class for extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Forces the enumeration of an <see cref="IEnumerable{T}" /> and ignores the result.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerate.</typeparam>
        /// <param name="items">The enumerable to enumerate.</param>
        public static void Enumerate<T>(this IEnumerable<T> items)
        {
            foreach (var _ in items) { }
        }
    }
}
