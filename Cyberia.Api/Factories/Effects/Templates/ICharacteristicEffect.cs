using Cyberia.Api.Enums;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface ICharacteristicEffect
{
    int CharacteristicId { get; }

    Element? GetElement();
}
