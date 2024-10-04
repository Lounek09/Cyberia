namespace Cyberia.Langzilla.Enums;

/// <summary>
/// Type of the lang.
/// </summary>
public enum LangType
{
    /// <summary>
    /// Represents the lang for the official servers.
    /// </summary>
    Official,
    /// <summary>
    /// Represents the lang for the beta servers.
    /// </summary>
    Beta,
    /// <summary>
    /// Represents the lang for the temporis servers.
    /// </summary>
    Temporis
}

public static class ExtendLangType
{
    public static string ToStringFast(this LangType type)
    {
        return Enum.GetName(type) ?? type.ToString();
    }
}
