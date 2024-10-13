using System.Resources;

namespace Cyberia.Translations;

public sealed class Api : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => ApiTranslations.ResourceManager;
}

public sealed class Bot : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => BotTranslations.ResourceManager;
}

public sealed class Web : ITranslationsWrapper
{
    public static ResourceManager ResourceManager => WebTranslations.ResourceManager;
}

