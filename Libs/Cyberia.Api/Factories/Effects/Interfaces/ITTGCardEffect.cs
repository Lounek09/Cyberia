using Cyberia.Api.Data.TTG;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ITTGCardEffect
{
    int TTGCardId { get; }

    TTGCardData? GetTTGCardData();
}
