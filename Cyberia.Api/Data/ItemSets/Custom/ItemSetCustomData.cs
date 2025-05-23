﻿using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Custom;

internal sealed class ItemSetCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("e")]
    [JsonConverter(typeof(EffectReadOnlyListOfReadOnlyListConverter))]
    public IReadOnlyList<IReadOnlyList<IEffect>> Effects { get; init; }

    [JsonConstructor]
    internal ItemSetCustomData()
    {
        Effects = [];
    }
}
