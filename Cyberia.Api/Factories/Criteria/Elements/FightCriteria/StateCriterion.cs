using Cyberia.Api.Data.States;

namespace Cyberia.Api.Factories.Criteria;

public sealed record StateCriterion
    : Criterion, ICriterion
{
    public int StateId { get; init; }

    private StateCriterion(string id, char @operator, int stateId)
        : base(id, @operator)
    {
        StateId = stateId;
    }

    internal static StateCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var stateId))
        {
            return new(id, @operator, stateId);
        }

        return null;
    }

    public StateData? GetStateData()
    {
        return DofusApi.Datacenter.StatesData.GetStateDataById(StateId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.State.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var stateName = DofusApi.Datacenter.StatesData.GetStateNameById(StateId);

        return GetDescription(stateName);
    }
}
