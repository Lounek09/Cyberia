using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when an error occurs in the <see cref="ILangsWatcher"/>.
/// </summary>
public sealed class LangsErroredEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets the exception that occurred, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string ErrorMessage { get; init; }

    /// <inheritdoc cref="LangsErroredEventArgs(Exception?, string)"/>
    [SetsRequiredMembers]
    public LangsErroredEventArgs(string errorMessage)
        : this(null, errorMessage)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsErroredEventArgs"/> class.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="exception">The exception that occurred, if any.</param>
    [SetsRequiredMembers]
    public LangsErroredEventArgs(Exception? exception, string errorMessage)
    {
        Exception = exception;
        ErrorMessage = errorMessage;
    }
}
