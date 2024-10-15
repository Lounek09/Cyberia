using Cyberia.Api.Data.Rides;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainRideEffect : Effect
{
    public int RideId { get; init; }
    public int RideAbilityId { get; init; }
    public bool Infertile { get; init; }

    private CharacterGainRideEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int rideId, int rideAbilityId, bool infertile)
        : base(id, duration, probability, criteria, effectArea)
    {
        RideId = rideId;
        RideAbilityId = rideAbilityId;
        Infertile = infertile;
    }

    internal static CharacterGainRideEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2, parameters.Param1 == 1);
    }

    public RideData? GetRideData()
    {
        return DofusApi.Datacenter.RidesRepository.GetRideDataById(RideId);
    }

    public RideAbilityData? GetRideAbilityData()
    {
        return DofusApi.Datacenter.RidesRepository.GetRideAbilityDataById(RideAbilityId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var value = DofusApi.Datacenter.RidesRepository.GetRideNameById(RideId, culture);

        var rideAbility = GetRideAbilityData();
        if (rideAbility is not null)
        {
            value += ' ' + rideAbility.Name.ToString(culture);
        }

        if (Infertile)
        {
            value += ' ' + Translation.Get<ApiTranslations>("Infertile", culture);
        }

        return GetDescription(culture, value);
    }
}
