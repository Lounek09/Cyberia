﻿using Cyberia.Api.Data.Runes;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface IRuneGeneratorEffect
{
    int RuneId { get; init; }

    RuneData? GetRuneData()
    {
        return DofusApi.Datacenter.RunesData.GetRuneDataById(RuneId);
    }

    int GetRandomValue();
}