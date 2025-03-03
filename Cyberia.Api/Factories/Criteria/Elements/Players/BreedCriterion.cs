﻿using Cyberia.Api.Data.Breeds;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record BreedCriterion : Criterion
{
    public int BreedId { get; init; }

    public BreedCriterion(string id, char @operator, int breedId)
        : base(id, @operator)
    {
        BreedId = breedId;
    }

    internal static BreedCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var breedId))
        {
            return new(id, @operator, breedId);
        }

        return null;
    }

    public BreedData? GetBreedData()
    {
        return DofusApi.Datacenter.BreedsRepository.GetBreedDataById(BreedId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Breed.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var breedName = DofusApi.Datacenter.BreedsRepository.GetBreedNameById(BreedId, culture);

        return GetDescription(culture, breedName);
    }
}
