using LanguageExt;

namespace PatternMatching
{
	public interface IPattern<in TInput, TMatchResult>
	{
		Option<TMatchResult> Match(TInput input);
	}
}
