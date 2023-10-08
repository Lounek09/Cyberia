using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS.Custom
{
    internal sealed class BreedCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bs")]
        public int SpecialSpellId { get; init; }

        [JsonPropertyName("is")]
        public int ItemSetId { get; init; }

        public BreedCustomData()
        {

        }
    }

    internal sealed class BreedsCustomData
    {
        [JsonPropertyName("CG")]
        public List<BreedCustomData> Breeds { get; init; }

        public BreedsCustomData()
        {
            Breeds = new();
        }
    }
}
