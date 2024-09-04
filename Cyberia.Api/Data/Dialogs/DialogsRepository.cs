using Cyberia.Api.JsonConverters;

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
}
