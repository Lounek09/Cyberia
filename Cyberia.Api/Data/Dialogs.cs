using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class DialogQuestionData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Question { get; init; }

    [JsonConstructor]
    internal DialogQuestionData()
    {
        Question = string.Empty;
    }
}

public sealed class DialogAnswerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Answer { get; init; }

    [JsonConstructor]
    internal DialogAnswerData()
    {
        Answer = string.Empty;
    }
}

public sealed class DialogsData : IDofusData
{
    private const string FILE_NAME = "dialog.json";

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

    internal static DialogsData Load()
    {
        return Datacenter.LoadDataFromFile<DialogsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
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
