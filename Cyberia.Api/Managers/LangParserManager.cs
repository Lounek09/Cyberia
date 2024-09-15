using Cyberia.Api.Parser;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

namespace Cyberia.Api.Managers;

public static class LangParserManager
{
    public static string GetOutputDirectoryPath(LangType type, LangLanguage language)
    {
        return Path.Join(DofusApi.OutputPath, type.ToStringFast().ToLower(), language.ToStringFast());
    }

    public static bool Parse(LangType type, LangLanguage language)
    {
        LangsWatcher langsWatcher = new(); //TODO: Temporary solution, should be injected
        var repository = langsWatcher.GetRepository(type, language);

        var directoryPath = GetOutputDirectoryPath(type, language);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            foreach (var lang in repository.Langs)
            {
                if (LangParser.IgnoredLangs.Contains(lang.Name))
                {
                    continue;
                }

                using var parser = LangParser.Create(lang);
                File.WriteAllText(Path.Join(directoryPath, $"{lang.Name}.json"), parser.ToString());
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while parsing langs {LangType} in {LangLanguage}", type, language);
            return false;
        }

        return true;
    }

    public static bool ParseAll(LangType type)
    {
        return Enum.GetValues<LangLanguage>()
                   .All(x => Parse(type, x));
    }
}
