namespace PatternMatching
{
	/// <summary>
	/// A static class which is used to create match expressions.
	/// </summary>
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
		public static Matcher<TInput, TOutput> Create<TInput, TOutput>()
			=> new Matcher<TInput, TOutput>();

		/// <summary>
		/// Creates a match statement.
		/// </summary>
		/// <typeparam name="TInput">The type of the input value.</typeparam>
		/// <returns>
		/// A matcher which specifies the patterns to match with and actions which are executed.
		/// </returns>
		public static Matcher<TInput> Create<TInput>()
			=> new Matcher<TInput>();
	}
}
