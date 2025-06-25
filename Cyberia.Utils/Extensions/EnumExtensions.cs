namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts an enum value to its string representation using a fast method.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The enum value to convert.</param>
    /// <returns>The string representation of the enum value.</returns>
    public static string ToStringFast<T>(this T value)
        where T : struct, Enum
    {
        return Enum.GetName(value) ?? value.ToString();
    }
}
