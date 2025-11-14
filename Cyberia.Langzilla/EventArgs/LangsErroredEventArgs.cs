using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when an error occurs in the <see cref="ILangsWatcher"/>.
/// </summary>
public sealed class LangsErroredEventArgs : ErroredEventArgs
{
    /// <inheritdoc/>
    [SetsRequiredMembers]
    public LangsErroredEventArgs(string errorMessage)
        : base(null, errorMessage) { }

    /// <inheritdoc/>
    [SetsRequiredMembers]
    public LangsErroredEventArgs(Exception? exception, string errorMessage)
        : base(exception, errorMessage) { }
}
