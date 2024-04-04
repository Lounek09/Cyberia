using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public class PetsData
    : IDofusData
{
    private const string c_fileName = "pets.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<PetsData>(s_filePath);
    }

    public PetData? GetPetDataByItemId(int id)
    {
        Pets.TryGetValue(id, out var petData);
        return petData;
    }
}
