using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Dialogs.Localized;

internal sealed class DialogsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => DialogsRepository.FileName;

    [JsonPropertyName("D.q")]
    public IReadOnlyList<DialogQuestionLocalizedData> DialogQuestions { get; init; }

    [JsonPropertyName("D.a")]
    public IReadOnlyList<DialogAnswerLocalizedData> DialogAnswers { get; init; }

    [JsonConstructor]
    internal DialogsLocalizedRepository()
    {
        DialogQuestions = ReadOnlyCollection<DialogQuestionLocalizedData>.Empty;
        DialogAnswers = ReadOnlyCollection<DialogAnswerLocalizedData>.Empty;
    }
}
