using Cyberia.Database.Models;
using Cyberia.Langzilla.Primitives;

using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when a check of the langs is finished.
/// </summary>
public sealed class NewLangsDetectedEventArgs : System.EventArgs
{
    /// <summary>
    /// The identifier of the check Langs.
    /// </summary>
    public required LangsIdentifier Identifier { get; init; }

    /// <summary>
    /// The list of the updated langs.
    /// </summary>
    public required IReadOnlyList<Lang> UpdatedLangs { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NewLangsDetectedEventArgs"/> class.
    /// </summary>
    /// <param name="identifier">The identifier of the check Langs.</param>
    /// <param name="updatedLangs">The list of the updated langs.</param>
    [SetsRequiredMembers]
    internal NewLangsDetectedEventArgs(LangsIdentifier identifier, IReadOnlyList<Lang> updatedLangs)
    {
        Identifier = identifier;
        UpdatedLangs = updatedLangs;
    }
}
