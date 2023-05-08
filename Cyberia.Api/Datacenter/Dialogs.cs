using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class DialogQuestion
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Question { get; init; }

        public DialogQuestion()
        {
            Question = string.Empty;
        }
    }

    public sealed class DialogAnswer
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Answer { get; init; }

        public DialogAnswer()
        {
            Answer = string.Empty;
        }
    }

    public sealed class DialogsData
    {
        private const string FILE_NAME = "dialog.json";

        [JsonPropertyName("Dq")]
        public List<DialogQuestion> DialogQuestions { get; init; }

        [JsonPropertyName("Da")]
        public List<DialogAnswer> DialogAnswers { get; init; }

        public DialogsData()
        {
            DialogQuestions = new();
            DialogAnswers = new();
        }

        internal static DialogsData Build()
        {
            return Json.LoadFromFile<DialogsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public DialogQuestion? GetDialogQuestionById(int od)
        {
            return DialogQuestions.Find(x => x.Id == od);
        }

        public DialogAnswer? GetDialogAnswerById(int id)
        {
            return DialogAnswers.Find(x => x.Id == id);
        }
    }
}
