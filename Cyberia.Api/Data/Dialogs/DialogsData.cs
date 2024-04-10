using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs;

public sealed class DialogsData : IDofusData
{
    private const string c_fileName = "dialog.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<DialogsData>(s_filePath);
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
