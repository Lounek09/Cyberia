using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories;

/// <summary>
/// Provides factory methods for creating <see cref="EffectArea"/>.
/// </summary>
public static class EffectAreaFactory
{
    /// <summary>
    /// Gets the default <see cref="EffectArea"/>.
    /// </summary>
    public static readonly EffectArea Default = new(80, 0);

    /// <summary>
    /// Creates an <see cref="EffectArea"/> from a compressed string representation.
    /// </summary>
    /// <param name="compressedEffectArea">The compressed string representation of the effect area.</param>
    /// <returns>The created <see cref="EffectArea"/>.</returns>
    public static EffectArea Create(ReadOnlySpan<char> compressedEffectArea)
    {
        if (compressedEffectArea.Length != 2)
        {
            Log.Warning("Failed to create EffectArea from {CompressedEffectArea}", compressedEffectArea.ToString());
            return Default;
        }

        return new EffectArea(compressedEffectArea[0], PatternDecoder.CharToBase64Index(compressedEffectArea[1]));
    }

    /// <summary>
    /// Creates a list of <see cref="EffectArea"/> from a compressed string representation.
    /// </summary>
    /// <param name="compressedEffectAreas">The compressed string representation of multiple effect areas.</param>
    /// <returns>The list of created <see cref="EffectArea"/>.</returns>
    public static List<EffectArea> CreateMany(ReadOnlySpan<char> compressedEffectAreas)
    {
        List<EffectArea> effectAreas = new(compressedEffectAreas.Length / 2);

        var length = compressedEffectAreas.Length - 1;
        for (var i = 0; i < length; i += 2)
        {
            var effectArea = new EffectArea(compressedEffectAreas[i], PatternDecoder.CharToBase64Index(compressedEffectAreas[i + 1]));
            effectAreas.Add(effectArea);
        }

        return effectAreas;
    }
}
