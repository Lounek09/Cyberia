using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    internal sealed class BreedCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bs")]
        public int SpecialSpellId { get; init; }

        [JsonPropertyName("is")]
        public int ItemSetId { get; init; }

        public BreedCustom()
        {

        }
    }

    internal sealed class BreedsCustomData
    {
        [JsonPropertyName("CG")]
        public List<BreedCustom> BreedsCustom { get; init; }

        public BreedsCustomData()
        {
            BreedsCustom = new();
        }
    }
}
