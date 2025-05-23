﻿using Cyberia.Api.Data.Dialogs.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "dialog.json";

    [JsonPropertyName("D.q")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DialogQuestionData>))]
    public FrozenDictionary<int, DialogQuestionData> DialogQuestions { get; init; }

    [JsonPropertyName("D.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DialogAnswerData>))]
    public FrozenDictionary<int, DialogAnswerData> DialogAnswers { get; init; }

    [JsonConstructor]
    internal DialogsRepository()
    {
        DialogQuestions = FrozenDictionary<int, DialogQuestionData>.Empty;
        DialogAnswers = FrozenDictionary<int, DialogAnswerData>.Empty;
    }

    public DialogQuestionData? GetDialogQuestionDataById(int id)
    {
        DialogQuestions.TryGetValue(id, out var dialogQuestionData);
        return dialogQuestionData;
    }

    public DialogAnswerData? GetDialogAnswerDataById(int id)
    {
        DialogAnswers.TryGetValue(id, out var dialogAnswerData);
        return dialogAnswerData;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<DialogsLocalizedRepository>(type, language);

        foreach (var dialogQuestionLocalizedData in localizedRepository.DialogQuestions)
        {
            var dialogQuestionData = GetDialogQuestionDataById(dialogQuestionLocalizedData.Id);
            dialogQuestionData?.Message.TryAdd(twoLetterISOLanguageName, dialogQuestionLocalizedData.Message);
        }

        foreach (var dialogAnswerLocalizedData in localizedRepository.DialogAnswers)
        {
            var dialogAnswerData = GetDialogAnswerDataById(dialogAnswerLocalizedData.Id);
            dialogAnswerData?.Message.TryAdd(twoLetterISOLanguageName, dialogAnswerLocalizedData.Message);
        }
    }
}
