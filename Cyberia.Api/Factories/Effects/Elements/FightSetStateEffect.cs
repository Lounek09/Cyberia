using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightSetStateEffect : Effect, IStateEffect
{
    public int StateId { get; init; }

    private FightSetStateEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int stateId)
        : base(id, duration, probability, criteria, effectArea)
    {
        StateId = stateId;
    }

    internal static FightSetStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
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
