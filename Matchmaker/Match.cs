namespace Matchmaker
{
    /// <summary>
    /// A static class which is used to create match expressions.
    /// </summary>
    /// <seealso cref="Match{TInput, TOutput}" />
    /// <seealso cref="Match{TInput}" />
    public static class Match
    {
        /// <summary>
        /// Creates a match expression.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <returns>
        /// A matcher which specifies the patterns to match with and functions which are executed.
        /// </returns>
        public static Match<TInput, TOutput> Create<TInput, TOutput>()
            => new Match<TInput, TOutput>(fallthroughByDefault: false);

        /// <summary>
        /// Creates a match expression.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        /// <returns>
        /// A matcher which specifies the patterns to match with and functions which are executed.
        /// </returns>
        public static Match<TInput, TOutput> Create<TInput, TOutput>(bool fallthroughByDefault)
            => new Match<TInput, TOutput>(fallthroughByDefault);

        /// <summary>
        /// Creates a match statement.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <returns>
        /// A matcher which specifies the patterns to match with and actions which are executed.
        /// </returns>
        public static Match<TInput> Create<TInput>()
            => new Match<TInput>(fallthroughByDefault: false);

        /// <summary>
        /// Creates a match statement with the specified default fallthrough behaviour.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <param name="fallthroughByDefault">The default fallthrough behaviour.</param>
        /// <returns>
        /// A matcher which specifies the patterns to match with and actions which are executed.
        /// </returns>
        public static Match<TInput> Create<TInput>(bool fallthroughByDefault)
            => new Match<TInput>(fallthroughByDefault);
    }
}
