using System.Collections.ObjectModel;
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
        Games = ReadOnlyDictionary<string, CytrusGame>.Empty;
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
