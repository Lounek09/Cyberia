using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLearnSpellLevelEffect : Effect, ISpellLevelEffect
{
    public int SpellLevelId { get; init; }

    private CharacterLearnSpellLevelEffect(int id, int spellLevelId)
        : base(id)
    {
        SpellLevelId = spellLevelId;
    }

    internal static CharacterLearnSpellLevelEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
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
