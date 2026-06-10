using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolIncreaseSpellLevelEffect : Effect, ISpellEffect
{
    public int SpellId { get; init; }
    public int LevelIncrease { get; init; }

    private GladiatroolIncreaseSpellLevelEffect(int id, int spellId, int levelIncrease)
        : base(id)
    {
        SpellId = spellId;
        LevelIncrease = levelIncrease;
    }

    internal static GladiatroolIncreaseSpellLevelEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param2);
    }

    public SpellData? GetSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpellId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellName = DofusApi.Datacenter.SpellsRepository.GetSpellNameById(SpellId, culture);

        return GetDescription(culture, string.Empty, LevelIncrease, spellName);
    }
}
