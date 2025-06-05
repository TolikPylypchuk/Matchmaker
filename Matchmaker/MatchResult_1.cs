namespace Matchmaker;

/// <summary>
/// Represents the result of a pattern match.
/// </summary>
/// <typeparam name="T">The type of the value contained in this class.</typeparam>
/// <remarks>
/// If the result is successful, it contains a value which may be <see langword="null" />. If it is not,
/// then it doesn't contain a value.
/// </remarks>
/// <seealso cref="MatchResult" />
/// <seealso cref="MatchResultExtensions" />
public readonly struct MatchResult<T> : IEquatable<MatchResult<T>>
{
    /// <summary>
    /// The value of the result if it's successful.
    /// </summary>
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
    [AllowNull]
    [MaybeNull]
#endif
    private readonly T value;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchResult{T}" /> class.
    /// </summary>
    /// <param name="isSuccessful">The value which indicates whether the match result is successful.</param>
    /// <param name="value">The value of the result, if it is successful.</param>
    internal MatchResult(
        bool isSuccessful,
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        [AllowNull]
#endif
        T value)
    {
        this.IsSuccessful = isSuccessful;
        this.value = value;
    }

    /// <summary>
    /// Gets the value which indicates whether the match result is successful.
    /// </summary>
    public bool IsSuccessful { get; }

    /// <summary>
    /// Gets the value if the match result is successful. If it is not, then throws an
    /// <see cref="InvalidOperationException" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The result is not successful.
    /// </exception>
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
    [MaybeNull]
#endif
    public T Value =>
        this.IsSuccessful
            ? this.value
            : throw new InvalidOperationException("Cannot get the value - result is not successful.");

    /// <summary>
    /// Gets the instance of a failed match result.
    /// </summary>
    internal static MatchResult<T> Failure { get; } = new MatchResult<T>(false, default);

    /// <summary>
    /// Compares this match result to another object.
    /// </summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>
    /// <see langword="true" /> if this match result is equal to the other object.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is MatchResult<T> other && this.Equals(other);

    /// <summary>
    /// Compares this match result to another match result.
    /// </summary>
    /// <param name="other">The match result to compare to.</param>
    /// <returns>
    /// <see langword="true" /> if this match result is equal to the other match result.
    /// Otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(MatchResult<T> other) =>
        (this.IsSuccessful == other.IsSuccessful) && Equals(this.value, other.value);

    /// <summary>
    /// Returns the hash code of this match result.
    /// </summary>
    /// <returns>The hash code of this match result.</returns>
    public override int GetHashCode() =>
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        HashCode.Combine(this.IsSuccessful, this.value);
#else
        this.value?.GetHashCode() ?? 0;
#endif

    /// <summary>
    /// Returns the string representation of this match result.
    /// </summary>
    /// <returns>The string representation of this match result.</returns>
    public override string ToString() =>
        this.IsSuccessful ? $"Success: {this.Value}" : "Failure";

    /// <summary>
    /// Compares two match results for equality.
    /// </summary>
    /// <param name="left">The left match result.</param>
    /// <param name="right">The right match result.</param>
    /// <returns>
    /// <see langword="true" /> if the match results are equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(MatchResult<T> left, MatchResult<T> right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two match results for inequality.
    /// </summary>
    /// <param name="left">The left match result.</param>
    /// <param name="right">The right match result.</param>
    /// <returns>
    /// <see langword="true" /> if the match results are not equal. Otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(MatchResult<T> left, MatchResult<T> right) =>
        !left.Equals(right);
}
