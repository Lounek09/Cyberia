using Cyberia.Api.Data;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.EffectAreas;

/// <summary>
/// Represents an effect area in the game.
/// </summary>
[JsonConverter(typeof(EffectAreaConverter))]
public readonly record struct EffectArea
{
    /// <summary>
    /// Gets the unique identifier of the effect area.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the size of the effect area. Represents infinity if greater than or equal to 63.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EffectArea"/> struct.
    /// </summary>
    /// <param name="id">The unique identifier of the effect area.</param>
    /// <param name="size">The size of the effect area.</param>
    public EffectArea(int id, int size)
    {
        Id = id;
        Size = size;
    }

    /// <summary>
    /// Retrieves the icon image path for the effect area from the CDN asynchronously.
    /// </summary>
    /// <param name="size">The requested size of the icon image.</param>
    /// <returns>The icon image path url.</returns>
    public Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return CdnManager.GetImagePathAsync("effectareas", Id, size);
    }

    /// <summary>
    /// Gets the size of the effect area as a string.
    /// </summary>
    /// <returns>A string representing the size of the effect area. Returns "Infinity" if the size is greater than or equal to 63.</returns>
    public string GetSize()
    {
        return Size >= 63 ? ApiTranslations.ShortInfinity : Size.ToString();
    }

    /// <summary>
    /// Generates a human-readable description of the effect area.
    /// </summary>
    /// <returns>The <see cref="Description"/> object containing the description of the effect area.</returns>
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

        return new($"#1 - {effectAreaName}", GetSize());
    }
}
