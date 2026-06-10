using Cyberia.Api.Data.States;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IStateEffect
{
    public int StateId { get; }

    public StateData? GetStateData();
}
