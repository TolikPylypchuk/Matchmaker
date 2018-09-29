namespace PatternMatching
{
	public static class Match
	{
		public static Matcher<TInput, TOutput> Create<TInput, TOutput>()
			=> new Matcher<TInput, TOutput>();

		public static Matcher<TInput> Create<TInput>()
			=> new Matcher<TInput>();
	}
}
