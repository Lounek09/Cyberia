using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla
{
    public sealed class LangsType
    {
        public LangType Type { get; init; }
        public LangsData French { get; init; }
        public LangsData English { get; init; }
        public LangsData Spanish { get; init; }
        public LangsData German { get; init; }
        public LangsData Italian { get; init; }
        public LangsData Netherlands { get; init; }
        public LangsData Portuguese { get; init; }

        public LangsType(LangType type)
        {
            Type = type;
            French = LangsData.Load(type, Language.FR);
            English = LangsData.Load(type, Language.EN);
            Spanish = LangsData.Load(type, Language.ES);
            German = LangsData.Load(type, Language.DE);
            Italian = LangsData.Load(type, Language.IT);
            Netherlands = LangsData.Load(type, Language.NL);
            Portuguese = LangsData.Load(type, Language.PT);
        }

        public LangsData GetLangsByLanguage(Language language)
        {
            return language switch
            {
                Language.FR => French,
                Language.EN => English,
                Language.ES => Spanish,
                Language.DE => German,
                Language.IT => Italian,
                Language.NL => Netherlands,
                Language.PT => Portuguese,
                _ => throw new NotImplementedException()
            };
        }
    }
}
