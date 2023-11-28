using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellsData : IDofusData
{
    private const string FILE_NAME = "spells.json";

    [JsonPropertyName("S")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpellData>))]
    public FrozenDictionary<int, SpellData> Spells { get; init; }

    [JsonConstructor]
    internal SpellsData()
    {
        Spells = FrozenDictionary<int, SpellData>.Empty;
    }

    internal static SpellsData Load()
    {
        return Datacenter.LoadDataFromFile<SpellsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public SpellData? GetSpellDataById(int id)
    {
        Spells.TryGetValue(id, out var spellData);
        return spellData;
    }

    public IEnumerable<SpellData> GetSpellsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Spells.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetSpellNameById(int id)
    {
        var spellData = GetSpellDataById(id);

        return spellData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : spellData.Name;
    }

    public SpellLevelData? GetSpellLevelDataById(int id)
    {
        foreach (var spellData in Spells.Values)
        {
            for (var i = 1; i <= 6; i++)
            {
                var spellLevelData = spellData.GetSpellLevelData(i);
                if (spellLevelData is null)
                {
                    break;
                }

                if (spellLevelData.Id == id)
                {
                    return spellLevelData;
                }
            }
        }

        return null;
    }
}
