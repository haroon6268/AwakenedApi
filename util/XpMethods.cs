namespace AwakenedApi.util;

public static class XpMethods
{
    /// <summary>
    /// Converts experience points to level using the formula: Math.Floor(Math.Sqrt(xp / 12.5))
    /// </summary>
    /// <param name="xp">The experience points to convert</param>
    /// <returns>The calculated level</returns>
    public static int XpToLevel(int xp)
    {
        return (int)Math.Floor(Math.Sqrt(xp / 12.5));
    }

    /// <summary>
    /// Converts level to experience points using the formula: Math.Round(12.5 * Math.Pow(level, 2))
    /// </summary>
    /// <param name="level">The level to convert</param>
    /// <returns>The calculated experience points</returns>
    public static int LevelToXp(int level)
    {
        return (int)Math.Round(12.5 * Math.Pow(level, 2));
    }
}
