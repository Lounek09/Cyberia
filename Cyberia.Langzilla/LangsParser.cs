using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Parser;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a service that manages the parsing of langs.
/// </summary>
public sealed class LangsParser
{
    public static readonly string OutputPath = "api";
    public static readonly IReadOnlyList<string> IgnoredLangs = ["dungeons", "lang"];

    private readonly LangsWatcher _langsWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsParser"/> class.
    /// </summary>
    /// <param name="langsWatcher">The langs watcher to get the langs from.</param>
    public LangsParser(LangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    /// <summary>
    /// Parses the langs of the specified type and language.
    /// </summary>
    /// <param name="type">The type of langs to parse.</param>
    /// <param name="language">The language of langs to parse.</param>
    /// <returns><see langword="true"/> if the langs were parsed successfully; otherwise, <see langword="false"/>.</returns>
    public bool Parse(LangType type, LangLanguage language)
    {
        var repository = _langsWatcher.GetRepository(type, language);

        var outputPath = Path.Join(OutputPath, type.ToStringFast().ToLower(), language.ToStringFast());
        Directory.CreateDirectory(outputPath);

        try
        {
            foreach (var lang in repository.Langs)
            {
                if (!IgnoredLangs.Contains(lang.Name))
                {
                    using var parser = JsonLangParser.Create(lang);
                    File.WriteAllText(Path.Join(outputPath, $"{lang.Name}.json"), parser.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while parsing langs {LangType} in {LangLanguage}", type, language);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Parses the langs for all languages of the specified type.
    /// </summary>
    /// <param name="type">The type of langs to parse.</param>
    /// <returns><see langword="true"/> if the langs for each languages were parsed successfully; otherwise, <see langword="false"/>.</returns>"
    public bool ParseAll(LangType type)
    {
        return Enum.GetValues<LangLanguage>().All(x => Parse(type, x));
    }
}
