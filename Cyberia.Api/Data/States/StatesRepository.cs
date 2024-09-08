using Cyberia.Api.Data.States.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
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

    public string GetStateNameById(int id)
    {
        var stateData = GetStateDataById(id);

        return stateData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : stateData.Name;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<StatesLocalizedRepository>(type, language);

        foreach (var stateLocalizedData in localizedRepository.States)
        {
            var stateData = GetStateDataById(stateLocalizedData.Id);
            if (stateData is not null)
            {
                stateData.Name.Add(twoLetterISOLanguageName, stateLocalizedData.Name);
                stateData.ShortName.Add(twoLetterISOLanguageName, stateLocalizedData.ShortName);
            }
        }
    }
}
