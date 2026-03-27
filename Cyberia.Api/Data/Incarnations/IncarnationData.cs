using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Utils;
using Cyberia.Langzilla.Primitives;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<int> SpellsId { get; init; }

    [JsonPropertyName("e")]
    public EffectReadOnlyCollection Effects { get; init; }

    [JsonConstructor]
    internal IncarnationData()
    {
        SpellsId = ReadOnlyCollection<int>.Empty;
        Effects = EffectReadOnlyCollection.Empty;
    }

    public async Task<string> GetBigImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/big", GfxId, size);
    }

    public async Task<string> GetFaceImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/faces", GfxId, size);
    }

    public async Task<string> GetMiniImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/mini", GfxId, size);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(Id);
    }

    public string GetItemName(Language language)
    {
        return GetItemName(language.ToCulture());
    }

    public string GetItemName(CultureInfo? culture = null)
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemNameById(Id, culture);
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                yield return spellData;
            }
        }
    }

    public IReadOnlyList<Effect> GetRealEffects()
    {
        var itemData = GetItemData();
        if (itemData is not null)
        {
            var itemStatsData = itemData.GetItemStatsData();
            if (itemStatsData is not null)
            {
                return Effects.Concat(itemStatsData.Effects).ToList();
            }
        }

        return Effects;
    }
}
