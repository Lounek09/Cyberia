using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla.EventArgs;

public sealed class CheckLangFinishedEventArgs
    : System.EventArgs
{
    public LangType Type { get; init; }
    public LangLanguage Language { get; init; }
    public IReadOnlyList<LangData> UpdatedLangsData { get; init; }

    public CheckLangFinishedEventArgs(LangType type, LangLanguage language, IReadOnlyList<LangData> updatedLangsData)
    {
        Type = type;
        Language = language;
        UpdatedLangsData = updatedLangsData;
    }
}
