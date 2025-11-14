namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Enum"/>.
/// </summary>
public static class EnumExtensions
{
    extension<T>(T value) where T : struct, Enum
    {
        /// <summary>
        /// Converts an enum value to its string representation using a fast method.
        /// </summary>
        /// <returns>The string representation of the enum value.</returns>
        public string ToStringFast()
        {
            return Enum.GetName(value) ?? value.ToString();
        }
    }
}
