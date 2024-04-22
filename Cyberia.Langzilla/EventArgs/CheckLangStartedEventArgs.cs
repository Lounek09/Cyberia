using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when a check of the langs is started.
/// </summary>
public sealed class CheckLangStartedEventArgs : System.EventArgs
{
    /// <summary>
    /// The checked lang repository.
    /// </summary>
    public LangRepository Repository { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckLangStartedEventArgs"/> class.
    /// </summary>
    /// <param name="repository">The checked lang repository.</param>
    internal CheckLangStartedEventArgs(LangRepository repository)
    {
        Repository = repository;
    }
}
