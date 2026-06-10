using Cyberia.Api.Data.States.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StatesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "states.json";

    [JsonPropertyName("ST")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, StateData>))]
    public FrozenDictionary<int, StateData> States { get; init; }

    [JsonConstructor]
    internal StatesRepository()
    {
        States = FrozenDictionary<int, StateData>.Empty;
    }

    public StateData? GetStateDataById(int id)
    {
        States.TryGetValue(id, out var stateData);
        return stateData;
    }

    public string GetStateNameById(int id, Language language)
    {
        return GetStateNameById(id, language.ToCulture());
    }

    public string GetStateNameById(int id, CultureInfo? culture = null)
    {
        var stateData = GetStateDataById(id);

        return stateData is null
            ? Translation.UnknownData(id, culture)
            : stateData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<StatesLocalizedRepository>(identifier);

        foreach (var stateLocalizedData in localizedRepository.States)
        {
            var stateData = GetStateDataById(stateLocalizedData.Id);
            if (stateData is not null)
            {
                stateData.Name.TryAdd(twoLetterISOLanguageName, stateLocalizedData.Name);
                stateData.ShortName.TryAdd(twoLetterISOLanguageName, stateLocalizedData.ShortName);
            }
        }
    }
}
