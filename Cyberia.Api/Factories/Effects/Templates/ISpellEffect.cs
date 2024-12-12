using Cyberia.Api.Data.Spells;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface ISpellEffect
{
    public int SpellId { get; init; }

    public SpellData? GetSpellData();
}
