using Cyberia.Api.Data;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Utils;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
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
        return ImageUrlProvider.GetImagePathAsync("effectareas", Id, size);
    }

    public string GetName(Language language)
    {
        return GetName(language.ToCulture());
    }

    public string GetName(CultureInfo? culture = null)
    {
        if (Translation.TryGet<ApiTranslations>($"EffectArea.{Id}", out var effectAreaName, culture))
        {
            return effectAreaName;
        }

        Log.Warning("Unknown {EffectArea} {EffectAreaId}", nameof(EffectArea), Id);
        return Translation.UnknownData(Id, culture);
    }

    /// <summary>
    /// Gets the size of the effect area as a string for the specified language.
    /// </summary>
    /// <param name="language">The language to get the size for.</param>
    /// <returns>A string representation of the size of the effect area for the specified language.</returns>
    public string GetSize(Language language)
    {
        return GetSize(language.ToCulture());
    }

    /// <summary>
    /// Gets the size of the effect area as a string for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to get the size for.</param>
    /// <returns>A string representation of the size of the effect area for the specified culture.</returns>
    public string GetSize(CultureInfo? culture = null)
    {
        return Size >= 63
            ? Translation.Get<ApiTranslations>("ShortInfinity", culture)
            : Size.ToString();
    }

    /// <summary>
    /// Generates a human-readable description of the effect area for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect area for the specified language.</returns>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates a human-readable description of the effect area for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect area for the specified culture.</returns>
    public DescriptionString GetDescription(CultureInfo? culture = null)
    {
        if (Id == EffectAreaFactory.Default.Id)
        {
            return DescriptionString.Empty;
        }

        return new DescriptionString($"#1 - {GetName(culture)}", GetSize(culture));
    }
}
