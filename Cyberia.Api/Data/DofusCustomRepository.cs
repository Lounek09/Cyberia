using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusCustomRepository
{
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
