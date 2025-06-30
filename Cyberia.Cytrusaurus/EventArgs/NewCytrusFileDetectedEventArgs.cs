using Cyberia.Cytrusaurus.Models;

using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Cytrusaurus.EventArgs;

/// <summary>
/// Represents the event arguments for when a new Cytrus is detected.
/// </summary>
public sealed class NewCytrusFileDetectedEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets the current Cytrus data.
    /// </summary>
    public required Cytrus Cytrus { get; init; }

    /// <summary>
    /// Gets the old Cytrus data.
    /// </summary>
    public required Cytrus OldCytrus { get; init; }

    /// <summary>
    /// Gets the difference between the current and old Cytrus data.
    /// </summary>
    public required string Diff { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NewCytrusFileDetectedEventArgs"/> class.
    /// </summary>
    /// <param name="cytrus">The current Cytrus data.</param>
    /// <param name="oldCytrus">The old Cytrus data.</param>
    /// <param name="diff">The difference between the current and old Cytrus data.</param>
    [SetsRequiredMembers]
    internal NewCytrusFileDetectedEventArgs(Cytrus cytrus, Cytrus oldCytrus, string diff)
    {
        Cytrus = cytrus;
        OldCytrus = oldCytrus;
        Diff = diff;
    }
}
