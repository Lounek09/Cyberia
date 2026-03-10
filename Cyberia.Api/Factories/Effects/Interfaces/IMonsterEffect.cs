using Cyberia.Api.Data.Monsters;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IMonsterEffect
{
    int MonsterId { get; }

    MonsterData? GetMonsterData();
}
