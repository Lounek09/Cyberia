using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LaunchSpellLevelEffect : Effect, ISpellLevelEffect
{
    public int SpellLevelId { get; init; }

    private LaunchSpellLevelEffect(int id, int spellLevelId)
        : base(id)
    {
        SpellLevelId = spellLevelId;
    }

    internal static LaunchSpellLevelEffect Create(int effectId, EffectParameters parameters)
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
            return GetDescription(culture, 0, string.Empty, $"{nameof(SpellLevelData)} {Translation.UnknownData(SpellLevelId, culture)}");
        }

        return GetDescription(culture, spellLevelData.Rank, string.Empty, spellLevelData.SpellData.Name.ToString(culture));
    }
}
