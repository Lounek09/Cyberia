﻿using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGData : IDofusData
{
    private const string FILE_NAME = "ttg.json";

    [JsonPropertyName("TTG.c")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGCardData>))]
    public FrozenDictionary<int, TTGCardData> TTGCards { get; set; }

    [JsonPropertyName("TTG.e")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGEntityData>))]
    public FrozenDictionary<int, TTGEntityData> TTGEntities { get; set; }

    [JsonPropertyName("TTG.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGFamilyData>))]
    public FrozenDictionary<int, TTGFamilyData> TTGFamilies { get; set; }

    [JsonConstructor]
    internal TTGData()
    {
        TTGCards = FrozenDictionary<int, TTGCardData>.Empty;
        TTGEntities = FrozenDictionary<int, TTGEntityData>.Empty;
        TTGFamilies = FrozenDictionary<int, TTGFamilyData>.Empty;
    }

    internal static TTGData Load()
    {
        return Datacenter.LoadDataFromFile<TTGData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public TTGCardData? GetTTGCardDataById(int id)
    {
        TTGCards.TryGetValue(id, out var ttgCardData);
        return ttgCardData;
    }

    public TTGEntityData? GetTTGEntityDataById(int id)
    {
        TTGEntities.TryGetValue(id, out var ttgEntityData);
        return ttgEntityData;
    }

    public string GetTTGEntityNameById(int id)
    {
        var ttgEntityData = GetTTGEntityDataById(id);

        return ttgEntityData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : ttgEntityData.Name;
    }

    public TTGFamilyData? GetTTGFamilyDataById(int id)
    {
        TTGFamilies.TryGetValue(id, out var ttgFamilyData);
        return ttgFamilyData;
    }

    public string GetTTGFamilyNameById(int id)
    {
        var ttgFamilyData = GetTTGFamilyDataById(id);

        return ttgFamilyData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : ttgFamilyData.Name;
    }
}