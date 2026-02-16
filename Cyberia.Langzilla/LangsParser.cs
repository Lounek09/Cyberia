using Cyberia.Langzilla.Parser;
using Cyberia.Langzilla.Primitives;

using System.Collections.ObjectModel;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a service that manages the parsing of langs.
/// </summary>
public interface ILangsParser
{
    /// <summary>
    /// Parses the langs of the specified type and language.
    /// </summary>
    /// <param name="identifier">The identifier of the langs to parse.</param>
    /// <returns><see langword="true"/> if the langs were parsed successfully; otherwise, <see langword="false"/>.</returns>
    Task<bool> ParseAsync(LangsIdentifier identifier);

    /// <summary>
    /// Parses the langs for all languages of the specified type.
    /// </summary>
    /// <param name="type">The type of langs to parse.</param>
    /// <returns><see langword="true"/> if the langs for each languages were parsed successfully; otherwise, <see langword="false"/>.</returns>
    Task<bool> ParseAllAsync(LangType type);
}

public sealed class LangsParser : ILangsParser
{
    /// <summary>
    /// The root output directory.
    /// </summary>
    public static readonly string OutputPath = "api";

    /// <summary>
    /// The list of langs that are ignored during parsing.
    /// </summary>
    public static readonly ReadOnlySet<string> IgnoredLangs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "dungeons",
        "lang"
    }.AsReadOnly();

    private readonly ILangsWatcher _langsWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsParser"/> class.
    /// </summary>
    /// <param name="langsWatcher">The langs watcher to get the langs from.</param>
    public LangsParser(ILangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    public async Task<bool> ParseAsync(LangsIdentifier identifier)
    {
        var repository = _langsWatcher.GetRepository(identifier);
        var outputPath = Path.Join(OutputPath, identifier.Type.ToStringFast().ToLower(), identifier.Language.ToStringFast());

        Directory.CreateDirectory(outputPath);

        using CancellationTokenSource cts = new();

        try
        {
            await Parallel.ForEachAsync(repository.Langs, new ParallelOptions { CancellationToken = cts.Token }, async (lang, token) =>
            {
                if (IgnoredLangs.Contains(lang.Name))
                {
                    return;
                }

                try
                {
                    using var parser = JsonLangParser.Create(lang);
                    parser.Parse();

                    var filePath = Path.Join(outputPath, $"{lang.Name}.json");
                    await File.WriteAllTextAsync(filePath, parser.ToString(), token);
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occurred while parsing {LangType} {LangName} lang in {Language}", identifier.Type, lang.Name, identifier.Language);

                    cts.Cancel();
                }
            });
        }
        catch (OperationCanceledException)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ParseAllAsync(LangType type)
    {
        var tasks = Enum.GetValues<Language>()
            .Select(language => ParseAsync(new LangsIdentifier(type, language)));

        var results = await Task.WhenAll(tasks);

        return results.All(x => x);
    }
}
