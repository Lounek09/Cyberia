using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record StateEffect : Effect
{
    public int StateId { get; init; }

    protected StateEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int stateId)
        : base(id, duration, probability, criteria, effectArea)
    {
        StateId = stateId;
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
