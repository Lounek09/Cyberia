using Cyberia.Api.Data.Monsters;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IMonsterSuperRaceEffect
{
    int MonsterSuperRaceId { get; }

    MonsterSuperRaceData? GetMonsterSuperRaceData();
}
