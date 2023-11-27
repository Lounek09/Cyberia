using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla;

public sealed class LangsType
{
    public LangType Type { get; init; }
    public LangDataCollection French { get; init; }
    public LangDataCollection English { get; init; }
    public LangDataCollection Spanish { get; init; }
    public LangDataCollection German { get; init; }
    public LangDataCollection Italian { get; init; }
    public LangDataCollection Netherlands { get; init; }
    public LangDataCollection Portuguese { get; init; }

    public LangsType(LangType type)
    {
        Type = type;
        French = LangDataCollection.Load(type, LangLanguage.FR);
        English = LangDataCollection.Load(type, LangLanguage.EN);
        Spanish = LangDataCollection.Load(type, LangLanguage.ES);
        German = LangDataCollection.Load(type, LangLanguage.DE);
        Italian = LangDataCollection.Load(type, LangLanguage.IT);
        Netherlands = LangDataCollection.Load(type, LangLanguage.NL);
        Portuguese = LangDataCollection.Load(type, LangLanguage.PT);
    }

    public LangDataCollection GetLangsByLanguage(LangLanguage language)
    {
        return language switch
        {
            LangLanguage.FR => French,
            LangLanguage.EN => English,
            LangLanguage.ES => Spanish,
            LangLanguage.DE => German,
            LangLanguage.IT => Italian,
            LangLanguage.NL => Netherlands,
            LangLanguage.PT => Portuguese,
            _ => throw new NotImplementedException()
        };
    }
}
