using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolChangeSpellToBestElementEffect : Effect, ISpellEffect
{
    public int SpellId { get; init; }

    private GladiatroolChangeSpellToBestElementEffect(int id, int spellId)
        : base(id)
    {
        SpellId = spellId;
    }

    internal static GladiatroolChangeSpellToBestElementEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId, culture);

        return GetDescription(culture, string.Empty, string.Empty, spellName);
    }
}
