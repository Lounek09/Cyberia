using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusTranslationRepository
{
    internal static T Load<T>(LangType type, LangLanguage language)
        where T : DofusTranslationRepository, IDofusRepository
    {
        var filePath = Path.Join(LangParserManager.GetOutputDirectoryPath(type, language), T.FileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File {filePath} not found to initialize {typeof(T).Name}");
        }

        using var json = File.OpenRead(filePath);

        return JsonSerializer.Deserialize<T>(json)
            ?? throw new JsonException($"Deserialization of {typeof(T).Name} from {filePath} returned null");
    }
}
