using System.Resources;

namespace Cyberia.Translations;

public interface ITranslationsWrapper
{
    static abstract ResourceManager ResourceManager { get; }
}

public sealed class ApiTranslations : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => Resources.ApiTranslations.ResourceManager;
}

public sealed class BotTranslations : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => Resources.BotTranslations.ResourceManager;
}

public sealed class WebTranslations : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => Resources.WebTranslations.ResourceManager;
}

