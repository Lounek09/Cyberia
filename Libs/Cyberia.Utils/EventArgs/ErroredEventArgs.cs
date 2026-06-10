using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Utils.EventArgs;

public abstract class ErroredEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets the exception that occurred, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string ErrorMessage { get; init; }

    /// <inheritdoc cref="ErroredEventArgs(Exception?, string)"/>
    [SetsRequiredMembers]
    public ErroredEventArgs(string errorMessage)
        : this(null, errorMessage) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErroredEventArgs"/> class.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="exception">The exception that occurred, if any.</param>
    [SetsRequiredMembers]
    public ErroredEventArgs(Exception? exception, string errorMessage)
    {
        Exception = exception;
        ErrorMessage = errorMessage;
    }
}
