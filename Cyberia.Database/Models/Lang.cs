using Cyberia.Langzilla.Enums;

namespace Cyberia.Database.Models;

/// <summary>
/// Represents a lang file of Dofus Retro.
/// </summary>
public sealed class Lang : IDatabaseEntity<int>
{
    public int Id { get; init; }

    /// <summary>
    /// Gets the type of the lang file.
    /// </summary>
    public required LangType Type { get; init; }

    /// <summary>
    /// Gets the language of the lang file.
    /// </summary>
    public required Language Language { get; init; }

    /// <summary>
    /// Gets the name of the lang file.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the version of the lang file.
    /// </summary>
    public required int Version { get; set; }

    /// <summary>
    /// Gets or sets whether the lang file is new.
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Lang"/> class.
    /// </summary>
    public Lang() { }
}
