using Cyberia.Api.Data.Breeds.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedsData
    : IDofusData
{
    private const string c_fileName = "classes.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);
    private static readonly string s_customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

    [JsonPropertyName("G")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, BreedData>))]
    public FrozenDictionary<int, BreedData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsData()
    {
        Breeds = FrozenDictionary<int, BreedData>.Empty;
    }

    internal static async Task<BreedsData> LoadAsync()
    {
        var data = await Datacenter.LoadDataAsync<BreedsData>(s_filePath);
        var customData = await Datacenter.LoadDataAsync<BreedsCustomData>(s_customFilePath);

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
        name = name.NormalizeCustom();

        return Breeds.Values.FirstOrDefault(x => x.Name.NormalizeCustom().Equals(name));
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Breeds.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeCustom().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetBreedNameById(int id)
    {
        var breed = GetBreedDataById(id);

        return breed is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : breed.Name;
    }
}
