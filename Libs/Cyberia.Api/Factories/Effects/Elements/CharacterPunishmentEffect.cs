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

    private CharacterPunishmentEffect(int id, int effectId, int maxBoost, int turn)
        : base(id)
    {
        EffectId = effectId;
        MaxBoost = maxBoost;
        Turn = turn;
    }

    internal static CharacterPunishmentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
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
