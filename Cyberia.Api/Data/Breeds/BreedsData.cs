using Cyberia.Api.Data.Breeds.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedsData : IDofusData
{
    private const string FILE_NAME = "classes.json";

    [JsonPropertyName("G")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, BreedData>))]
    public FrozenDictionary<int, BreedData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsData()
    {
        Breeds = FrozenDictionary<int, BreedData>.Empty;
    }

    internal static BreedsData Load()
    {
        var data = Datacenter.LoadDataFromFile<BreedsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        var customData = Datacenter.LoadDataFromFile<BreedsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

        foreach (var breedCustomData in customData.Breeds)
        {
            var breedData = data.GetBreedDataById(breedCustomData.Id);
            if (breedData is not null)
            {
                breedData.SpecialSpellId = breedCustomData.SpecialSpellId;
                breedData.ItemSetId = breedCustomData.ItemSetId;
            }
        }

        return data;
    }

    public BreedData? GetBreedDataById(int id)
    {
        Breeds.TryGetValue(id, out var breedData);
        return breedData;
    }

    public BreedData? GetBreedDataByName(string name)
    {
        return Breeds.Values.FirstOrDefault(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Breeds.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetBreedNameById(int id)
    {
        var breed = GetBreedDataById(id);

        return breed is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : breed.Name;
    }
}
