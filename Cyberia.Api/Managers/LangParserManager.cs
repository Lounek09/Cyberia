using Cyberia.Api.Parser;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

namespace Cyberia.Api.Managers;

public static class LangParserManager
{
    public static string GetOutputDirectoryPath(LangType type, LangLanguage language)
    {
        return Path.Join(DofusApi.OutputPath, type.ToString().ToLower(), language.ToString().ToLower());
    }

    public static bool Parse(LangType type, LangLanguage language)
    {
        var repository = LangsWatcher.LangRepositories[(type, language)];

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
            Log.Error(e, "Une erreur est survenue lors du parsing des langs {LangType} en {LangLanguage}", type, language);
            return false;
        }

        return true;
    }

    public static bool ParseAll(LangType type)
    {
        var languages = Enum.GetValues<LangLanguage>();

        return languages.All(x => Parse(type, x));
    }
}
