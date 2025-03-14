﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names;

public sealed class TaxCollectorLastNameData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Name { get; init; }

    [JsonConstructor]
    internal TaxCollectorLastNameData()
    {
        Name = LocalizedString.Empty;
    }
}
