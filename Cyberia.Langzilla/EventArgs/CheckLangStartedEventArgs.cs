using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when a check of the langs is started.
/// </summary>
public sealed class CheckLangStartedEventArgs : System.EventArgs
{

    /// <summary>
    /// Gets the type of the checked langs.
    /// </summary>
    public LangType Type { get; init; }

    /// <summary>
    /// Gets the language of the checked langs.
    /// </summary>
    public LangLanguage Language { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckLangStartedEventArgs"/> class.
    /// </summary>
    /// <param name="type">The type of the langs.</param>
    /// <param name="language">The language of the langs.</param>
    internal CheckLangStartedEventArgs(LangType type, LangLanguage language)
    {
        Type = type;
        Language = language;
    }
}
