using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLearnSpellLevelEffect : Effect, ISpellLevelEffect
{
    public int SpellLevelId { get; init; }

    private CharacterLearnSpellLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int spellLevelId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        SpellLevelId = spellLevelId;
    }

    internal static CharacterLearnSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
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
            return GetDescription(culture, string.Empty, 0, $"{nameof(SpellLevelData)} {Translation.UnknownData(SpellLevelId, culture)}");
        }

        return GetDescription(culture, string.Empty, spellLevelData.Rank, spellLevelData.SpellData.Name.ToString(culture));
    }
}
