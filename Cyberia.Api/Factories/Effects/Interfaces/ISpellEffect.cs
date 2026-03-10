using Cyberia.Api.Data.Spells;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ISpellEffect
{
    public int SpellId { get; }

    public SpellData? GetSpellData();
}
