﻿using Cyberia.Api.Data.Runes;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface IRuneGeneratorEffect
{
    int RuneId { get; }

    RuneData? GetRuneData()
    {
        return DofusApi.Datacenter.RunesRepository.GetRuneDataById(RuneId);
    }

    int GetRandomValue();
}
