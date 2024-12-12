using Cyberia.Api.Data.Spells;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface ISpellLevelEffect
{
    public int SpellLevelId { get; init; }

    public SpellLevelData? GetSpellLevelData();
}
