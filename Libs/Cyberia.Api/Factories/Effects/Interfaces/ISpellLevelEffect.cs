using Cyberia.Api.Data.Spells;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ISpellLevelEffect
{
    public int SpellLevelId { get; }

    public SpellLevelData? GetSpellLevelData();
}
