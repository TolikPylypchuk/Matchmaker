namespace Matchmaker;

public static class Extensions
{
    public static bool ImpliesThat(this bool premise, bool consequence) =>
        !premise || consequence;

    public static bool ImpliesThat(this bool premise, Func<bool> consequence) =>
        !premise || consequence();

    public static async Task<bool> ImpliesThatAsync(this bool premise, Func<Task<bool>> consequence) =>
        !premise || await consequence();
}
