namespace Matchmaker
{
    /// <summary>
    /// A static class which is used to create match results.
    /// </summary>
    /// <seealso cref="MatchResult{T}" />
    public static class MatchResult
    {
        /// <summary>
        /// Creates a successful match result with the specified value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>A successful match result with the specified value.</returns>
        /// <seealso cref="Failure{T}()" />
        public static MatchResult<T> Success<T>(T value)
            => new MatchResult<T>(true, value);

        /// <summary>
        /// Creates a failed match result.
        /// </summary>
        /// <returns>A failed match result.</returns>
        /// <seealso cref="Success{T}(T)" />
        public static MatchResult<T> Failure<T>()
            => new MatchResult<T>(true, default);
    }
}
