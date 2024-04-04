using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogsData
    : IDofusData
{
    private const string FILE_NAME = "dialog.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("D.q")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DialogQuestionData>))]
    public FrozenDictionary<int, DialogQuestionData> DialogQuestions { get; init; }

    [JsonPropertyName("D.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DialogAnswerData>))]
    public FrozenDictionary<int, DialogAnswerData> DialogAnswers { get; init; }

    [JsonConstructor]
    internal DialogsData()
    {
        DialogQuestions = FrozenDictionary<int, DialogQuestionData>.Empty;
        DialogAnswers = FrozenDictionary<int, DialogAnswerData>.Empty;
    }

    internal static async Task<DialogsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<DialogsData>(FILE_PATH);
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
