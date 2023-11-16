using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class DialogQuestionData
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

    public sealed class DialogAnswerData
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

    public sealed class DialogsData
    {
        private const string FILE_NAME = "dialog.json";

        [JsonPropertyName("D.q")]
        public List<DialogQuestionData> DialogQuestions { get; init; }

        [JsonPropertyName("D.a")]
        public List<DialogAnswerData> DialogAnswers { get; init; }

        [JsonConstructor]
        public DialogsData()
        {
            DialogQuestions = [];
            DialogAnswers = [];
        }

        internal static DialogsData Load()
        {
            return Json.LoadFromFile<DialogsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public DialogQuestionData? GetDialogQuestionDataById(int od)
        {
            return DialogQuestions.Find(x => x.Id == od);
        }

        public DialogAnswerData? GetDialogAnswerDataById(int id)
        {
            return DialogAnswers.Find(x => x.Id == id);
        }
    }
}
