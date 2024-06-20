using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories;

public static class EffectAreaFactory
{
    public static readonly EffectArea Default = new(80, 0);

    public static EffectArea Create(ReadOnlySpan<char> compressedEffectArea)
    {
        if (compressedEffectArea.Length != 2)
        {
            Log.Warning("Failed to create EffectArea from {CompressedEffectArea}", compressedEffectArea.ToString());
            return Default;
        }

        return new EffectArea(compressedEffectArea[0], PatternDecoder.CharToBase64Index(compressedEffectArea[1]));
    }

    public static List<EffectArea> CreateMany(ReadOnlySpan<char> compressedEffectAreas)
    {
        List<EffectArea> effectAreas = new(compressedEffectAreas.Length / 2);

        var length = compressedEffectAreas.Length - 1;
        for (var i = 0; i < length; i += 2)
        {
            var effectArea = ;
            effectAreas.Add(effectArea);
        }

        return effectAreas;
    }
}
