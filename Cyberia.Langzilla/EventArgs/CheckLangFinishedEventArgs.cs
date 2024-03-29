using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when a check of the langs is finished.
/// </summary>
public sealed class CheckLangFinishedEventArgs
    : System.EventArgs
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
    /// Gets the list of the updated checked langs.
    /// </summary>
    public IReadOnlyList<Lang> UpdatedLangs { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckLangFinishedEventArgs"/> class.
    /// </summary>
    /// <param name="type">The type of the langs.</param>
    /// <param name="language">The language of the langs.</param>
    /// <param name="updatedLangs">The list of updated langs.</param>
    internal CheckLangFinishedEventArgs(LangType type, LangLanguage language, IReadOnlyList<Lang> updatedLangs)
    {
        Type = type;
        Language = language;
        UpdatedLangs = updatedLangs;
    }
}
