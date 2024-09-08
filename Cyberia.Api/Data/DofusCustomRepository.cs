using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusCustomRepository
{
    /// <summary>
    /// Load a custom repository from a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the custom repository.</typeparam>
    /// <returns>The loaded custom repository.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the JSON file is not found.</exception>
    /// <exception cref="JsonException">Thrown when the deserialization of the JSON file returns null.</exception>
    internal static T Load<T>()
        where T : DofusCustomRepository, IDofusRepository
    {
        var filePath = Path.Join(DofusApi.CustomPath, T.FileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File {filePath} not found to initialize {typeof(T).Name}");
        }

        using var json = File.OpenRead(filePath);

        return JsonSerializer.Deserialize<T>(json)
            ?? throw new JsonException($"Deserialization of {typeof(T).Name} from {filePath} returned null");
    }
}
