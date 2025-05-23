﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapSuperAreaData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Name { get; init; }

    [JsonConstructor]
    internal MapSuperAreaData()
    {
        Name = LocalizedString.Empty;
    }
}
