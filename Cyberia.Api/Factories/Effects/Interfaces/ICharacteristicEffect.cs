using Cyberia.Api.Enums;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ICharacteristicEffect
{
    int CharacteristicId { get; }

    Element? GetElement();
}
