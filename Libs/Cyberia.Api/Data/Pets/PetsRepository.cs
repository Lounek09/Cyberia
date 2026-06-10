using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public sealed class PetsRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => "pets.json";

    [JsonPropertyName("PET")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, PetData>))]
    public FrozenDictionary<int, PetData> Pets { get; init; }

    [JsonConstructor]
    internal PetsRepository()
    {
        Pets = FrozenDictionary<int, PetData>.Empty;
    }

    public PetData? GetPetDataByItemId(int id)
    {
        Pets.TryGetValue(id, out var petData);
        return petData;
    }
}
