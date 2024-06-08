using Cyberia.Api.Data.Breeds.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedsRepository : IDofusRepository
{
    private const string c_fileName = "classes.json";

    [JsonPropertyName("G")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, BreedData>))]
    public FrozenDictionary<int, BreedData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsRepository()
    {
        Breeds = FrozenDictionary<int, BreedData>.Empty;
    }

    internal static BreedsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<BreedsRepository>(filePath);
        var customData = Datacenter.LoadRepository<BreedsCustomRepository>(customFilePath);

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
        name = name.NormalizeToAscii();

        return Breeds.Values.FirstOrDefault(x => x.Name.NormalizeToAscii().Equals(name));
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Breeds.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetBreedNameById(int id)
    {
        var breed = GetBreedDataById(id);

        return breed is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : breed.Name;
    }
}
