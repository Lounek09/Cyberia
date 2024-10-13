
using Cyberia.Api.Data.Rides.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RidesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "rides.json";

    [JsonPropertyName("RI")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideData>))]
    public FrozenDictionary<int, RideData> Rides { get; init; }

    [JsonPropertyName("RIA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideAbilityData>))]
    public FrozenDictionary<int, RideAbilityData> RideAbilities { get; init; }

    [JsonConstructor]
    internal RidesRepository()
    {
        Rides = FrozenDictionary<int, RideData>.Empty;
        RideAbilities = FrozenDictionary<int, RideAbilityData>.Empty;
    }

    public RideData? GetRideDataById(int id)
    {
        Rides.TryGetValue(id, out var rideData);
        return rideData;
    }

    public string GetRideNameById(int id)
    {
        var rideData = GetRideDataById(id);

        return rideData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : rideData.Name;
    }

    public RideAbilityData? GetRideAbilityDataById(int id)
    {
        RideAbilities.TryGetValue(id, out var rideAbilityData);
        return rideAbilityData;
    }

    public string GetRideAbilityNameById(int id)
    {
        var rideAbilityData = GetRideAbilityDataById(id);

        return rideAbilityData is null ? Translation.Format(ApiTranslations.Unknown_Data, id) : rideAbilityData.Name;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<RidesLocalizedRepository>(type, language);

        foreach (var rideLocalizedData in localizedRepository.Rides)
        {
            var rideDate = GetRideDataById(rideLocalizedData.Id);
            rideDate?.Name.Add(twoLetterISOLanguageName, rideLocalizedData.Name);
        }

        foreach (var rideAbilityLocalizedData in localizedRepository.RideAbilities)
        {
            var rideAbilityData = GetRideAbilityDataById(rideAbilityLocalizedData.Id);
            if (rideAbilityData is not null)
            {
                rideAbilityData.Name.Add(twoLetterISOLanguageName, rideAbilityLocalizedData.Name);
                rideAbilityData.Description.Add(twoLetterISOLanguageName, rideAbilityLocalizedData.Description);
            }
        }
    }
}
