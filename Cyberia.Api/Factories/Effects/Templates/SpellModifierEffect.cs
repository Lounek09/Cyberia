using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record SpellModifierEffect : Effect, ISpellEffect
{
    public int SpellId { get; init; }
    public int Value { get; init; }

    protected SpellModifierEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int spellId, int value)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        SpellId = spellId;
        Value = value;
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId, culture);

        return GetDescription(culture, spellName, string.Empty, Value);
    }
}
