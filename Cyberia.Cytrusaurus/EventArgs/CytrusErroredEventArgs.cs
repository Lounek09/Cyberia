using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Cytrusaurus.EventArgs;

/// <summary>
/// Represents the event arguments for when an error occurs in the Cytrus watcher.
/// </summary>
public sealed class CytrusErroredEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets the exception that occurred, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string ErrorMessage { get; init; }

    /// <inheritdoc cref="CytrusErroredEventArgs(Exception?, string)"/>
    [SetsRequiredMembers]
    public CytrusErroredEventArgs(string errorMessage)
        : this(null, errorMessage)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusErroredEventArgs"/> class.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="exception">The exception that occurred, if any.</param>
    [SetsRequiredMembers]
    public CytrusErroredEventArgs(Exception? exception, string errorMessage)
    {
        Exception = exception;
        ErrorMessage = errorMessage;
    }
}
