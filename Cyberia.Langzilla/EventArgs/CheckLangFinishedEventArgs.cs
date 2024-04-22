using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla.EventArgs;

/// <summary>
/// Represents the event arguments for when a check of the langs is finished.
/// </summary>
public sealed class CheckLangFinishedEventArgs : System.EventArgs
{
    /// <summary>
    /// The checked lang repository.
    /// </summary>
    public LangRepository Repository { get; init; }

    /// <summary>
    /// The list of the updated checked langs.
    /// </summary>
    public IReadOnlyList<Lang> UpdatedLangs { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckLangFinishedEventArgs"/> class.
    /// </summary>
    /// <param name="repository">The checked lang repository.</param>
    /// <param name="updatedLangs">The list of updated langs.</param>
    internal CheckLangFinishedEventArgs(LangRepository repository, IReadOnlyList<Lang> updatedLangs)
    {
        Repository = repository;
        UpdatedLangs = updatedLangs;
    }
}
