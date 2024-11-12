using Cyberia.Api.Data.States;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface IStateEffect
{
    public int StateId { get; init; }

    public StateData? GetStateData();
}
