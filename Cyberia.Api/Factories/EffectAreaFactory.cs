using Cyberia.Api.Data;

namespace Cyberia.Api.Managers;

public readonly record struct EffectArea(int Id, int Size)
{
    public Task<string> GetImagePathAsync(CdnImageSize size)
    {
        return CdnManager.GetImagePathAsync("effectareas", Id, size);
    }

    public string GetSize()
    {
        return Size >= 63 ? Resources.Infinity : Size.ToString();
    }

    public Description GetDescription()
    {
        if (Id == EffectAreaFactory.Default.Id)
        {
            return Description.Empty;
        }

        var effectAreaName = Resources.ResourceManager.GetString($"EffectArea.{Id}");
        if (effectAreaName is null)
        {
            Log.Warning("Unknown {EffectArea} {EffectAreaId}", nameof(EffectArea), Id);
            effectAreaName = PatternDecoder.Description(Resources.Unknown_Data, Id);
        }

        return new($"#1 {effectAreaName}", GetSize());
    }
}

public static class EffectAreaFactory
{
    public static readonly EffectArea Default = new(80, 0);

    public static EffectArea Create(string compressedEffectArea)
    {
        if (compressedEffectArea.Length != 2)
        {
            Log.Warning("Failed to create EffectArea from {CompressedEffectArea}", compressedEffectArea);
            return Default;
        }

        return new(compressedEffectArea[0], PatternDecoder.Base64(compressedEffectArea[1]));
    }

    public static IEnumerable<EffectArea> CreateMany(string compressedEffectAreas)
    {
        foreach (var value in compressedEffectAreas.SplitByLength(2))
        {
            yield return Create(value);
        }
    }
}
