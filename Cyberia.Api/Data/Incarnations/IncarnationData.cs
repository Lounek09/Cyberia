﻿using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
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
    [JsonConverter(typeof(ItemEffectListConverter))]
    private IReadOnlyList<IEffect> EffectsFromLeveling { get; init; }

    [JsonConstructor]
    internal IncarnationData()
    {
        Name = string.Empty;
        SpellsId = [];
        EffectsFromLeveling = [];
    }

    public async Task<string> GetImgPath()
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/artworks/{GfxId}.png";

        if (await DofusApi.HttpClient.ExistsAsync(url))
        {
            return url;
        }

        return $"{DofusApi.Config.CdnUrl}/images/artworks/unknown.png";
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                yield return spellData;
            }
        }
    }

    public IReadOnlyList<IEffect> GetEffects()
    {
        var itemData = GetItemData();
        if (itemData is not null)
        {
            var itemStatsData = itemData.GetItemStatsData();
            if (itemStatsData is not null)
            {
                var effects = itemStatsData.Effects.Where(x => x is not ExchangeableEffect).ToList();
                effects.AddRange(EffectsFromLeveling);

                return effects.AsReadOnly();
            }
        }

        return EffectsFromLeveling;
    }
}