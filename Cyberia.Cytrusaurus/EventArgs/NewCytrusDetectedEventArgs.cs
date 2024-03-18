using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus.EventArgs;

/// <summary>
/// Provides data for the NewCytrusDetected event.
/// </summary>
public sealed class NewCytrusDetectedEventArgs
    : System.EventArgs
{
    /// <summary>
    /// Gets the current Cytrus data.
    /// </summary>
    public CytrusData CytrusData { get; init; }

    /// <summary>
    /// Gets the old Cytrus data.
    /// </summary>
    public CytrusData OldCytrusData { get; init; }

    /// <summary>
    /// Gets the difference between the current and old Cytrus data.
    /// </summary>
    public string Diff { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NewCytrusDetectedEventArgs"/> class.
    /// </summary>
    /// <param name="cytrusData">The current Cytrus data.</param>
    /// <param name="oldCytrusData">The old Cytrus data.</param>
    /// <param name="diff">The difference between the current and old Cytrus data.</param>
    public NewCytrusDetectedEventArgs(CytrusData cytrusData, CytrusData oldCytrusData, string diff)
    {
        CytrusData = cytrusData;
        OldCytrusData = oldCytrusData;
        Diff = diff;
    }
}
