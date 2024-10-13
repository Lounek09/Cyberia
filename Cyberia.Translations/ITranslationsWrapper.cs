using System.Resources;

namespace Cyberia.Translations;

public interface ITranslationsWrapper
{
    static abstract ResourceManager ResourceManager { get; }
}
