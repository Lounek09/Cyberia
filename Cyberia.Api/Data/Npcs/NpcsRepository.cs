﻿using Cyberia.Api.Data.Npcs.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "npc.json";

    [JsonPropertyName("N.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcActionData>))]
    public FrozenDictionary<int, NpcActionData> NpcActions { get; init; }

    [JsonPropertyName("N.d")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcData>))]
    public FrozenDictionary<int, NpcData> Npcs { get; init; }

    [JsonConstructor]
    internal NpcsRepository()
    {
        NpcActions = FrozenDictionary<int, NpcActionData>.Empty;
        Npcs = FrozenDictionary<int, NpcData>.Empty;
    }

    public NpcActionData? GetNpcActionDataById(int id)
    {
        NpcActions.TryGetValue(id, out var npcActionData);
        return npcActionData;
    }

    public NpcData? GetNpcDataById(int id)
    {
        Npcs.TryGetValue(id, out var npcData);
        return npcData;
    }

    public string GetNpcNameById(int id, Language language)
    {
        return GetNpcNameById(id, language.ToCulture());
    }

    public string GetNpcNameById(int id, CultureInfo? culture = null)
    {
        var npc = GetNpcDataById(id);

        return npc is null
            ? Translation.UnknownData(id, culture)
            : npc.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<NpcsLocalizedRepository>(type, language);

        foreach (var npcActionLocalizedData in localizedRepository.NpcActions)
        {
            var npcActionData = GetNpcActionDataById(npcActionLocalizedData.Id);
            npcActionData?.Name.TryAdd(twoLetterISOLanguageName, npcActionLocalizedData.Name);
        }

        foreach (var npcLocalizedData in localizedRepository.Npcs)
        {
            var npcData = GetNpcDataById(npcLocalizedData.Id);
            npcData?.Name.TryAdd(twoLetterISOLanguageName, npcLocalizedData.Name);
        }
    }
}
