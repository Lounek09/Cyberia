using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LaunchSpellLevelEffect : Effect
{
    public int SpellLevelId { get; init; }

    private LaunchSpellLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellLevelId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    internal static LaunchSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public SpellLevelData? GetSpellLevelData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellLevelDataById(SpellLevelId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spellLevelData = GetSpellLevelData();
        if (spellLevelData is null)
        {
            return GetDescription(culture, $"{nameof(SpellLevelData)} {Translation.UnknownData(SpellLevelId, culture)}", 0);
        }

        return GetDescription(culture, spellLevelData.SpellData.Name.ToString(culture), spellLevelData.Rank);
    }
}
