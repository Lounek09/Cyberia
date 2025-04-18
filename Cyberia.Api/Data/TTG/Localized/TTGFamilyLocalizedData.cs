﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG.Localized;

internal sealed class TTGFamilyLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TTGFamilyLocalizedData()
    {
        Name = string.Empty;
    }
}
