using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<int> SpellsId { get; init; }

    [JsonPropertyName("e")]
    [JsonConverter(typeof(EffectReadOnlyListConverter))]
    public IReadOnlyList<IEffect> Effects { get; init; }

    [JsonConstructor]
    internal IncarnationData()
    {
        Name = string.Empty;
        SpellsId = [];
        Effects = [];
    }

    public async Task<string> GetBigImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/big", GfxId, size);
    }

    public async Task<string> GetFaceImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/faces", GfxId, size);
    }

    public async Task<string> GetMiniImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/mini", GfxId, size);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(Id);
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

    public IReadOnlyList<IEffect> GetRealEffects()
    {
        var itemData = GetItemData();
        if (itemData is not null)
        {
            var itemStatsData = itemData.GetItemStatsData();
            if (itemStatsData is not null)
            {
                var effects = itemStatsData.Effects.Where(x => x is not MarkNotTradableEffect).ToList();
                effects.AddRange(Effects);

                return effects;
            }
        }

        return Effects;
    }
}
