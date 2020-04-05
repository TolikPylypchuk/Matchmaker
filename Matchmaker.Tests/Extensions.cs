namespace Matchmaker
{
    public static class Extensions
    {
        public static bool ImpliesThat(this bool premise, bool consequence)
            => !premise || consequence;
    }
}
