﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Custom;

public sealed class EffectCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("c")]
    public int CharacteristicId { get; init; }

    [JsonPropertyName("o")]
    public string Operator { get; init; }

    [JsonPropertyName("t")]
    public bool ShowInTooltip { get; init; }

    [JsonPropertyName("j")]
    public bool ShowInDiceModePossible { get; init; }

    [JsonPropertyName("e")]
    public char? Element { get; init; }

    [JsonConstructor]
    internal EffectCustomData()
    {
        Description = string.Empty;
        Operator = string.Empty;
    }
}
