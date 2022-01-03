namespace Matchmaker;

using System;

public static class Extensions
{
    public static bool ImpliesThat(this bool premise, bool consequence) =>
        !premise || consequence;

    public static bool ImpliesThat(this bool premise, Func<bool> consequence) =>
        !premise || consequence();
}
