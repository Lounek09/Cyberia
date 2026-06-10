using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightUnsetStateEffect : Effect, IStateEffect
{
    public int StateId { get; init; }

    private FightUnsetStateEffect(int id, int stateId)
        : base(id)
    {
        StateId = stateId;
    }

    internal static FightUnsetStateEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public StateData? GetStateData()
    {
        return DofusApi.Datacenter.StatesRepository.GetStateDataById(StateId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var stateName = DofusApi.Datacenter.StatesRepository.GetStateNameById(StateId, culture);

        return GetDescription(culture, string.Empty, string.Empty, stateName);
    }
}
