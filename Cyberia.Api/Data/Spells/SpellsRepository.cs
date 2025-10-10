using Cyberia.Api.Data.Spells.Localized;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "spells.json";

    [JsonPropertyName("S")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpellData>))]
    public FrozenDictionary<int, SpellData> Spells { get; init; }

    [JsonConstructor]
    internal SpellsRepository()
    {
        Spells = FrozenDictionary<int, SpellData>.Empty;
    }

    public SpellData? GetSpellDataById(int id)
    {
        Spells.TryGetValue(id, out var spellData);
        return spellData;
    }

    public IEnumerable<SpellData> GetSpellsDataByName(string name, Language language)
    {
        return GetSpellsDataByName(name, language.ToCulture());
    }

    public IEnumerable<SpellData> GetSpellsDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Spells.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public IEnumerable<SpellData> GetSpellsDataWithEffectId(int effectId)
    {
        return Spells.Values.Where(x =>
        {
            return x.GetSpellLevelsData().Any(y =>
            {
                return y.Effects.Any(z => z.Id == effectId) ||
                    y.CriticalEffects.Any(z => z.Id == effectId);
            });
        });
    }

    public IEnumerable<SpellData> GetSpellsDataWithCriterionId(string criterionId)
    {
        return Spells.Values.Where(x =>
        {
            return x.GetSpellLevelsData().Any(y =>
            {
                return y.Effects.Any(z =>
                {
                    return z.Criteria.OfType<ICriterion>()
                        .Any(x => x.Id.Equals(criterionId));
                });
            });
        });
    }

    public string GetSpellNameById(int id, Language language)
    {
        return GetSpellNameById(id, language.ToCulture());
    }

    public string GetSpellNameById(int id, CultureInfo? culture = null)
    {
        var spellData = GetSpellDataById(id);

        return spellData is null
            ? Translation.UnknownData(id, culture)
            : spellData.Name.ToString(culture);
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

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<SpellsLocalizedRepository>(identifier);

        foreach (var spellLocalizedData in localizedRepository.Spells)
        {
            var spellData = GetSpellDataById(spellLocalizedData.Id);
            if (spellData is not null)
            {
                spellData.Name.TryAdd(twoLetterISOLanguageName, spellLocalizedData.Name);
                spellData.Description.TryAdd(twoLetterISOLanguageName, spellLocalizedData.Description);
            }
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var spellData in Spells.Values)
        {
            var i = 1;
            foreach (var spellLevelData in spellData.GetSpellLevelsData())
            {
                spellLevelData.SpellData = spellData;
                spellLevelData.Rank = i++;
            }
        }
    }
}
