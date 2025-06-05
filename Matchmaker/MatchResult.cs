namespace Matchmaker;

/// <summary>
/// A static class which is used to create match results.
/// </summary>
/// <seealso cref="MatchResult{T}" />
public static class MatchResult
{
    /// <summary>
    /// Returns a successful match result with the specified value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <returns>A successful match result with the specified value.</returns>
    public static MatchResult<T> Success<T>(
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        [AllowNull]
#endif
        T value) =>
        new(true, value);

    /// <summary>
    /// Returns a failed match result.
    /// </summary>
    /// <returns>A failed match result.</returns>
    public static MatchResult<T> Failure<T>() =>
        MatchResult<T>.Failure;
}
