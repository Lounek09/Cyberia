
using Cyberia.Api.Data.Rides.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
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

    public string GetRideNameById(int id, Language language)
    {
        return GetRideNameById(id, language.ToCulture());
    }

    public string GetRideNameById(int id, CultureInfo? culture = null)
    {
        var rideData = GetRideDataById(id);

        return rideData is null
            ? Translation.UnknownData(id, culture)
            : rideData.Name.ToString(culture);
    }

    public RideAbilityData? GetRideAbilityDataById(int id)
    {
        RideAbilities.TryGetValue(id, out var rideAbilityData);
        return rideAbilityData;
    }

    public string GetRideAbilityNameById(int id, Language language)
    {
        return GetRideAbilityNameById(id, language.ToCulture());
    }

    public string GetRideAbilityNameById(int id, CultureInfo? culture = null)
    {
        var rideAbilityData = GetRideAbilityDataById(id);

        return rideAbilityData is null
            ? Translation.UnknownData(id, culture)
            : rideAbilityData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<RidesLocalizedRepository>(identifier);

        foreach (var rideLocalizedData in localizedRepository.Rides)
        {
            var rideDate = GetRideDataById(rideLocalizedData.Id);
            rideDate?.Name.TryAdd(twoLetterISOLanguageName, rideLocalizedData.Name);
        }

        foreach (var rideAbilityLocalizedData in localizedRepository.RideAbilities)
        {
            var rideAbilityData = GetRideAbilityDataById(rideAbilityLocalizedData.Id);
            if (rideAbilityData is not null)
            {
                rideAbilityData.Name.TryAdd(twoLetterISOLanguageName, rideAbilityLocalizedData.Name);
                rideAbilityData.Description.TryAdd(twoLetterISOLanguageName, rideAbilityLocalizedData.Description);
            }
        }
    }
}
