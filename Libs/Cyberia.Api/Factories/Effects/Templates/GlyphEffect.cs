using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record GlyphEffect : Effect, ISpellEffect
{
    public int SpellId { get; init; }
    public int Level { get; init; }

    protected GlyphEffect(int id, int spellId, int level)
        : base(id)
    {
        SpellId = spellId;
        Level = level;
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId, culture);

        return GetDescription(culture, spellName, Level);
    }
}
