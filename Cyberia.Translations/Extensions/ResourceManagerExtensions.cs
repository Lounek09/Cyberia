using Cyberia.Langzilla.Enums;

using System.Resources;

namespace Cyberia.Translations.Extensions;

public static class ResourceManagerExtensions
{
    /// <summary>
    /// Returns the value of the string resource localized for the specified language.
    /// </summary>
    /// <param name="resourceManager">The resource manager to retrieve the resource from.</param>
    /// <param name="name">The name of the resource to retrieve.</param>
    /// <param name="language">The language to localize the resource for.</param>
    /// <returns>The value of the resource localized for the specified language, or <see langword="null"/> if <paramref name="name"/> cannot be found in the resource set.</returns>
    public static string? GetString(this ResourceManager resourceManager, string name, LangLanguage language)
    {
        return resourceManager.GetString(name, language.ToCulture());
    }
}
