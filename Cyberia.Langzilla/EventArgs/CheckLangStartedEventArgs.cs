using Cyberia.Langzilla.Enums;


namespace Cyberia.Langzilla.EventArgs;

public sealed class CheckLangStartedEventArgs
    : System.EventArgs
{
    public LangType Type { get; init; }
    public LangLanguage Language { get; init; }

    public CheckLangStartedEventArgs(LangType type, LangLanguage language)
    {
        Type = type;
        Language = language;
    }
}
