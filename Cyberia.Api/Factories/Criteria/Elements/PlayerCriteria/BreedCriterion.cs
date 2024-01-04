using Cyberia.Api.Data.Breeds;

namespace Cyberia.Api.Factories.Criteria;

public sealed record BreedCriterion
    : Criterion, ICriterion
{
    public int BreedId { get; init; }

    public BreedCriterion(string id, char @operator, int breedId)
        : base(id, @operator)
    {
        BreedId = breedId;
    }

    internal static BreedCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var breedId))
        {
            return new(id, @operator, breedId);
        }

        return null;
    }

    public BreedData? GetBreedData()
    {
        return DofusApi.Datacenter.BreedsData.GetBreedDataById(BreedId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Breed.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var breedName = DofusApi.Datacenter.BreedsData.GetBreedNameById(BreedId);

        return GetDescription(breedName);
    }
}
