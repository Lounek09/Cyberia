﻿using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellsRepository : IDofusRepository
{
    private const string c_fileName = "spells.json";

    [JsonPropertyName("S")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpellData>))]
    public FrozenDictionary<int, SpellData> Spells { get; init; }

    [JsonConstructor]
    internal SpellsRepository()
    {
        Spells = FrozenDictionary<int, SpellData>.Empty;
    }

    internal static SpellsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        var data = Datacenter.LoadRepository<SpellsRepository>(filePath);

        foreach (var spellData in data.Spells.Values)
        {
            var i = 1;
            foreach (var spellLevelData in spellData.GetSpellLevelsData())
            {
                spellLevelData.SpellData = spellData;
                spellLevelData.Rank = i++;
            }
        }

        return data;
    }

    public SpellData? GetSpellDataById(int id)
    {
        Spells.TryGetValue(id, out var spellData);
        return spellData;
    }

    public IEnumerable<SpellData> GetSpellsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Spells.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetSpellNameById(int id)
    {
        var spellData = GetSpellDataById(id);

        return spellData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : spellData.Name;
    }

    public SpellLevelData? GetSpellLevelDataById(int id)
    {
        foreach (var spellData in Spells.Values)
        {
            foreach (var spellLevelData in spellData.GetSpellLevelsData())
            {
                if (spellLevelData.Id == id)
                {
                    return spellLevelData;
                }
            }
        }

        return null;
    }
}