namespace Cyberia.Api.Managers;

public readonly record struct EffectArea(int Id, int Size)
{
    public static readonly EffectArea Default = new(80, 0);

    public string GetImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/effectareas/{Id}.png";
    }

    public string GetSize()
    {
        return Size >= 63 ? Resources.Infinity : Size.ToString();
    }

    public Description GetDescription()
    {
        if (Id == Default.Id)
        {
            return new();
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

public static class EffectAreaManager
{
    public static EffectArea Create(string compressedEffectArea)
    {
        if (compressedEffectArea.Length != 2)
        {
            Log.Warning("Failed to create EffectArea from {CompressedEffectArea}", compressedEffectArea);
            return EffectArea.Default;
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
