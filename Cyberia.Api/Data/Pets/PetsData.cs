using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public class PetsData
    : IDofusData
{
    private const string FILE_NAME = "pets.json";

    [JsonPropertyName("PET")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, PetData>))]
    public FrozenDictionary<int, PetData> Pets { get; init; }

    [JsonConstructor]
    internal PetsData()
    {
        Pets = FrozenDictionary<int, PetData>.Empty;
    }

    internal static PetsData Load()
    {
        return Datacenter.LoadDataFromFile<PetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public PetData? GetPetDataByItemId(int id)
    {
        Pets.TryGetValue(id, out var petData);
        return petData;
    }
}
