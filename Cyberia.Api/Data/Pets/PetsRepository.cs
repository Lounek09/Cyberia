using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public sealed class PetsRepository : IDofusRepository
{
    private const string c_fileName = "pets.json";

    [JsonPropertyName("PET")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, PetData>))]
    public FrozenDictionary<int, PetData> Pets { get; init; }

    [JsonConstructor]
    internal PetsRepository()
    {
        Pets = FrozenDictionary<int, PetData>.Empty;
    }

    internal static PetsRepository Load(string _)
    {
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        return Datacenter.LoadRepository<PetsRepository>(customFilePath);
    }

    public PetData? GetPetDataByItemId(int id)
    {
        Pets.TryGetValue(id, out var petData);
        return petData;
    }
}
