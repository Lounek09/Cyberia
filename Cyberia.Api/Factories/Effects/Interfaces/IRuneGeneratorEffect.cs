using Cyberia.Api.Data.Runes;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IRuneGeneratorEffect
{
    int RuneId { get; }

    RuneData? GetRuneData();

    int GetRandomValue();
}
