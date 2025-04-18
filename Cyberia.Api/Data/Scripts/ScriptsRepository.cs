﻿using Cyberia.Api.Data.Scripts.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Scripts;

public sealed class ScriptsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "scripts.json";

    [JsonPropertyName("SCR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ScriptDialogData>))]
    public FrozenDictionary<int, ScriptDialogData> ScriptDialogs { get; init; }

    [JsonConstructor]
    internal ScriptsRepository()
    {
        ScriptDialogs = FrozenDictionary<int, ScriptDialogData>.Empty;
    }

    public ScriptDialogData? GetScriptDialogDataById(int id)
    {
        ScriptDialogs.TryGetValue(id, out var scriptDialog);
        return scriptDialog;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<ScriptsLocalizedRepository>(type, language);

        foreach (var scriptDialogLocalizedData in localizedRepository.ScriptDialogs)
        {
            var scriptDialogData = GetScriptDialogDataById(scriptDialogLocalizedData.Id);
            scriptDialogData?.Message.TryAdd(twoLetterISOLanguageName, scriptDialogLocalizedData.Message);
        }
    }
}
