using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Parser;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a service that manages the parsing of langs.
/// </summary>
public interface ILangsParser
{
    /// <summary>
    /// Parses the langs of the specified type and language.
    /// </summary>
    /// <param name="type">The type of langs to parse.</param>
    /// <param name="language">The language of langs to parse.</param>
    /// <returns><see langword="true"/> if the langs were parsed successfully; otherwise, <see langword="false"/>.</returns>
    /// 
    Task<bool> ParseAsync(LangType type, Language language);

    /// <summary>
    /// Parses the langs for all languages of the specified type.
    /// </summary>
    /// <param name="type">The type of langs to parse.</param>
    /// <returns><see langword="true"/> if the langs for each languages were parsed successfully; otherwise, <see langword="false"/>.</returns>
    Task<bool> ParseAllAsync(LangType type);
}

public sealed class LangsParser : ILangsParser
{
    public static readonly string OutputPath = "api";
    public static readonly IReadOnlyList<string> IgnoredLangs = ["dungeons", "lang"];

    private readonly ILangsWatcher _langsWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsParser"/> class.
    /// </summary>
    /// <param name="langsWatcher">The langs watcher to get the langs from.</param>
    public LangsParser(ILangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    public async Task<bool> ParseAsync(LangType type, Language language)
    {
        var repository = _langsWatcher.GetRepository(type, language);

        var outputPath = Path.Join(OutputPath, type.ToStringFast().ToLower(), language.ToStringFast());
        Directory.CreateDirectory(outputPath);

        List<Task> tasks = [];

        try
        {
            foreach (var lang in repository.Langs)
            {
                if (!IgnoredLangs.Contains(lang.Name))
                {
                    tasks.Add(Task.Run(() =>
                    {
                        using var parser = JsonLangParser.Create(lang);
                        parser.Parse();

                        File.WriteAllText(Path.Join(outputPath, $"{lang.Name}.json"), parser.ToString());
                    }));
                }
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while parsing langs {LangType} in {LangLanguage}", type, language);
            return false;
        }

        return true;
    }

    public async Task<bool> ParseAllAsync(LangType type)
    {
        var tasks = Enum.GetValues<Language>().Select(language => ParseAsync(type, language));
        var results = await Task.WhenAll(tasks);
        return results.All(x => x);
    }
}
