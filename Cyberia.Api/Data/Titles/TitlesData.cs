﻿using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitlesData : IDofusData
{
    private const string FILE_NAME = "titles.json";

    [JsonPropertyName("PT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TitleData>))]
    public FrozenDictionary<int, TitleData> Titles { get; init; }

    [JsonConstructor]
    internal TitlesData()
    {
        Titles = FrozenDictionary<int, TitleData>.Empty;
    }

    internal static TitlesData Load()
    {
        return Datacenter.LoadDataFromFile<TitlesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public TitleData? GetTitleDataById(int id)
    {
        Titles.TryGetValue(id, out var titleData);
        return titleData;
    }

    public string GetTitleNameById(int id)
    {
        var titleData = GetTitleDataById(id);

        return titleData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : titleData.Name;
    }
}