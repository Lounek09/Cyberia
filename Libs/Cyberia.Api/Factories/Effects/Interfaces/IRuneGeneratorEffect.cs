using Cyberia.Api.Data.Runes;
using Cyberia.Api.Enums;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IRuneGeneratorEffect
{
    Rune Rune { get; }

    RuneData? GetRuneData();

    int GetRandomValue();
}
