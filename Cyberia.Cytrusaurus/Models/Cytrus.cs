using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Cytrusaurus.Models;

/// <summary>
/// Represents the Cytrus data.
/// </summary>
public sealed class Cytrus
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
    public IReadOnlyDictionary<string, CytrusGame> Games { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cytrus"/> class.
    /// </summary>
    [JsonConstructor]
    internal Cytrus()
    {
        Name = string.Empty;
        Games = new Dictionary<string, CytrusGame>();
    }

    /// <summary>
    /// Loads the <see cref="Cytrus"/> data from a file.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A new instance of the <see cref="Cytrus"/> class if the file does not exist; otherwise, the data loaded from the file.</returns>
    internal static Cytrus LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return new();
        }

        var json = File.ReadAllText(path);
        return Load(json);
    }

    /// <summary>
    /// Loads the <see cref="Cytrus"/> data from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to load the data from.</param>
    /// <returns>A new instance of the <see cref="Cytrus"/> class if the data could not be deserialized; otherwise, the deserialized data.</returns>
    internal static Cytrus Load(string json)
    {
        var cytrus = JsonSerializer.Deserialize<Cytrus>(json);
        if (cytrus is null)
        {
            Log.Error("Failed to deserialize the JSON to initialize {TypeName}", typeof(Cytrus).Name);
            return new();
        }

        return cytrus;
    }

    /// <summary>
    /// Retrieves a game by its name.
    /// </summary>
    /// <param name="name">The name of the game.</param>
    /// <returns>The <see cref="CytrusGame"/> if found; otherwise, <see langword="null"/>.</returns>
    public CytrusGame? GetGameByName(string name)
    {
        Games.TryGetValue(name, out var cytrusGame);
        return cytrusGame;
    }
}
