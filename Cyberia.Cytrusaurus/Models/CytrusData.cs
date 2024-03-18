using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Cytrusaurus.Models;

public sealed class CytrusData
{
    /// <summary>
    /// Version of Cytrus
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; init; }

    /// <summary>
    /// Name of the environment
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Collection of games
    /// </summary>
    [JsonPropertyName("games")]
    public Dictionary<string, CytrusGame> Games { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusData"/> class.
    /// </summary>
    [JsonConstructor]
    internal CytrusData()
    {
        Name = string.Empty;
        Games = [];
    }

    /// <summary>
    /// Loads the Cytrus data from a file.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A new instance of the <see cref="CytrusData"/> class if the file does not exist; otherwise, the data loaded from the file.</returns>
    internal static CytrusData LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return new();
        }

        var json = File.ReadAllText(path);
        return Load(json);
    }

    /// <summary>
    /// Loads the Cytrus data from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to load the data from.</param>
    /// <returns>A new instance of the <see cref="CytrusData"/> class if the data could not be deserialized; otherwise, the deserialized data.</returns>
    internal static CytrusData Load(string json)
    {
        var data = JsonSerializer.Deserialize<CytrusData>(json);
        if (data is null)
        {
            Log.Error("Failed to deserialize the JSON to initialize {TypeName}", typeof(CytrusData).Name);
            return new();
        }

        return data;
    }

    /// <summary>
    /// Retrieves a game by its name.
    /// </summary>
    /// <param name="name">The name of the game.</param>
    /// <returns>The game if found; otherwise, null.</returns>
    public CytrusGame? GetGameByName(string name)
    {
        Games.TryGetValue(name, out var cytrusGame);
        return cytrusGame;
    }
}
