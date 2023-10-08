using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record BreedCriterion : Criterion, ICriterion<BreedCriterion>
    {
        public int BreedId { get; init; }

        public BreedCriterion(string id, char @operator, int breedId) :
            base(id, @operator)
        {
            BreedId = breedId;
        }

        public static BreedCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int breedId))
                return new(id, @operator, breedId);

            return null;
        }

        public BreedData? GetBreedData()
        {
            return DofusApi.Instance.Datacenter.BreedsData.GetBreedDataById(BreedId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Breed.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string breedName = DofusApi.Instance.Datacenter.BreedsData.GetBreedNameById(BreedId);

            return GetDescription(breedName);
        }
    }
}
