namespace Matchmaker;

/// <summary>
/// Represents an exception which is thrown when a match expression hasn't found a successful pattern.
/// </summary>
/// <seealso cref="Match{TInput, TOutput}" />
/// <seealso cref="Match{TInput}" />
/// <seealso cref="AsyncMatch{TInput, TOutput}" />
/// <seealso cref="AsyncMatch{TInput}" />
[ExcludeFromCodeCoverage]
public class MatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MatchException" /> class.
    /// </summary>
    public MatchException()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchException" /> class.
    /// </summary>
    /// <param name="message">The message which describes this exception.</param>
    public MatchException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchException" /> class.
    /// </summary>
    /// <param name="message">The message which describes this exception.</param>
    /// <param name="innerException">The exception, which caused this exception.</param>
    public MatchException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
