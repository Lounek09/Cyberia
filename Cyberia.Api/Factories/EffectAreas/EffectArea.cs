using Cyberia.Api.Data;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.EffectAreas;

[JsonConverter(typeof(EffectAreaConverter))]
public readonly record struct EffectArea(int Id, int Size)
{
    public Task<string> GetImagePathAsync(CdnImageSize size)
    {
        return CdnManager.GetImagePathAsync("effectareas", Id, size);
    }

    public string GetSize()
    {
        return Size >= 63 ? ApiTranslations.Infinity : Size.ToString();
    }

    public Description GetDescription()
    {
        if (Id == EffectAreaFactory.Default.Id)
        {
            return Description.Empty;
        }

        var effectAreaName = ApiTranslations.ResourceManager.GetString($"EffectArea.{Id}");
        if (effectAreaName is null)
        {
            Log.Warning("Unknown {EffectArea} {EffectAreaId}", nameof(EffectArea), Id);
            effectAreaName = Translation.Format(ApiTranslations.Unknown_Data, Id);
        }

        return new($"#1 {effectAreaName}", GetSize());
    }
}
