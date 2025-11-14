using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Cytrusaurus.EventArgs;

/// <summary>
/// Represents the event arguments for when an error occurs in the <see cref="ICytrusWatcher"/>.
/// </summary>
public sealed class CytrusErroredEventArgs : ErroredEventArgs
{
    /// <inheritdoc/>
    [SetsRequiredMembers]
    public CytrusErroredEventArgs(string errorMessage)
        : base(null, errorMessage) { }

    /// <inheritdoc/>
    [SetsRequiredMembers]
    public CytrusErroredEventArgs(Exception? exception, string errorMessage)
        : base(exception, errorMessage) { }
}
