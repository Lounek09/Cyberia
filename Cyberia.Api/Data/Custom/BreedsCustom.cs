using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Custom
{
    internal sealed class BreedCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bs")]
        public int SpecialSpellId { get; init; }

        [JsonPropertyName("is")]
        public int ItemSetId { get; init; }

        [JsonConstructor]
        internal BreedCustomData()
        {

        }
    }

    internal sealed class BreedsCustomData
    {
        [JsonPropertyName("CG")]
        public List<BreedCustomData> Breeds { get; init; }

        [JsonConstructor]
        public BreedsCustomData()
        {
            Breeds = [];
        }
    }
}
