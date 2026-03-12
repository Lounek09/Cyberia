using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterPunishmentEffect : Effect, EffectEffect
{
    public int EffectId { get; init; }
    public int MaxBoost { get; init; }
    public int Turn { get; init; }

    private CharacterPunishmentEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int effectId, int maxBoost, int turn)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        EffectId = effectId;
        MaxBoost = maxBoost;
        Turn = turn;
    }

    internal static CharacterPunishmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public EffectData? GetEffectData()
    {
        return DofusApi.Datacenter.EffectsRepository.GetEffectDataById(EffectId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, MaxBoost, Turn);
    }
}
