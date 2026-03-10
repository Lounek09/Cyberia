using Cyberia.Api.Data.Monsters;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IMonsterRaceEffect
{
    int MonsterRaceId { get; }

    MonsterRaceData? GetMonsterRaceData();
}
