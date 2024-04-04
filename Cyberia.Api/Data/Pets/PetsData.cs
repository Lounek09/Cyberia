using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public class PetsData
    : IDofusData
{
    private const string FILE_NAME = "pets.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("PET")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, PetData>))]
    public FrozenDictionary<int, PetData> Pets { get; init; }

    [JsonConstructor]
    internal PetsData()
    {
        Pets = FrozenDictionary<int, PetData>.Empty;
    }

    internal static async Task<PetsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<PetsData>(FILE_PATH);
    }

    public PetData? GetPetDataByItemId(int id)
    {
        Pets.TryGetValue(id, out var petData);
        return petData;
    }
}
