namespace Cyberia.Langzilla.Primitives;

/// <summary>
/// Represents the identifier used to retrieve langs.
/// </summary>
/// <param name="Type">The type of the langs.</param>
/// <param name="Language">The language of the langs.</param>
public readonly record struct LangsIdentifier(LangType Type, Language Language);
