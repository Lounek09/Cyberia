﻿using Cyberia.Api.JsonConverters;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemTypeData : IDofusData<int>
{
    public const int TYPE_PET = 18;

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("t")]
    public int ItemSuperTypeId { get; init; }

    [JsonPropertyName("z")]
    [JsonConverter(typeof(EffectAreaConverter))]
    public EffectArea EffectArea { get; init; }

    [JsonConstructor]
    internal ItemTypeData()
    {
        Name = string.Empty;
    }
}