﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Runes;

public sealed class RuneData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("w")]
    public double Weight { get; init; }

    [JsonPropertyName("p")]
    public int Power { get; init; }

    [JsonPropertyName("pa")]
    public bool HasPa { get; init; }

    [JsonPropertyName("ra")]
    public bool HasRa { get; init; }

    [JsonConstructor]
    internal RuneData()
    {
        Name = string.Empty;
    }

    public string GetFullName()
    {
        return PatternDecoder.Description(Resources.Rune, Name);
    }
}