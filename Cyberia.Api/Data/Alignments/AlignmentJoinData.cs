﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

internal sealed class AlignmentJoinData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<bool> Values { get; init; }

    [JsonConstructor]
    internal AlignmentJoinData()
    {
        Values = [];
    }

    public bool CanJoin(int targetAlignmentId)
    {
        return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
    }
}
