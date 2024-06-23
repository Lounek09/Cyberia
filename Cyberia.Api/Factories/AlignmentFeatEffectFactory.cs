using Cyberia.Api.Factories.AlignmentFeatEffects;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

/// <summary>
/// Provides factory methods for creating <see cref="IAlignmentFeatEffect"/>.
/// </summary>
public static class AlignmentFeatEffectFactory
{
    /// <summary>
    /// A dictionary mapping alignment feat effect identifiers to their factory methods.
    /// </summary>
    private static readonly FrozenDictionary<int, Func<int, int[], IAlignmentFeatEffect?>> s_factories =
       new Dictionary<int, Func<int, int[], IAlignmentFeatEffect?>>()
       {
           { 1, CharacterBoostRangeAlignmentFeatEffect.Create },
           { 2, CharacterBoostInitiativeAlignmentFeatEffect.Create },
           { 3, CharacterBoostDamagesAlignmentFeatEffect.Create },
           { 4, CharacterBoostVitalityAlignmentFeatEffect.Create },
           { 6, CharacterBoostIntelligenceAlignmentFeatEffect.Create },
           { 7, CharacterBoostStrengthAlignmentFeatEffect.Create },
           { 8, CharacterBoostAgilityAlignmentFeatEffect.Create },
           { 9, CharacterBoostChanceAlignmentFeatEffect.Create },
           { 10, CharacterBoostCriticalHitAlignmentFeatEffect.Create },
           { 11, CharacterBoostActionPointsAlignmentFeatEffect.Create },
           { 12, CharacterBoostMovementPointsAlignmentFeatEffect.Create },
           { 13, CharacterBoostWisdomAlignmentFeatEffect.Create },
           { 14, CharacterBoostActionPointsLostAlignmentFeatEffect.Create },
           { 15, CharacterBoostActionPointsLostAlignmentFeatEffect.Create },
           { 16, StoreDiscountAlignmentFeatEffect.Create },
           { 17, CharacterBoostEarthElementPercentAlignmentFeatEffect.Create },
           { 18, CharacterBoostFireElementPercentAlignmentFeatEffect.Create },
           { 19, CharacterBoostWaterElementPercentAlignmentFeatEffect.Create },
           { 20, CharacterBoostAirElementPercentAlignmentFeatEffect.Create },
           { 21, CharacterBoostNeutralElementPercentAlignmentFeatEffect.Create },
           { 22, CharacterBoostMaximumEnergyPointsAlignmentFeatEffect.Create },
           { 23, CharacterEnergyLossBoostAlignmentFeatEffect.Create },
           { 24, CharacterBoostEarthElementResistAlignmentFeatEffect.Create },
           { 25, CharacterBoostFireElementResistAlignmentFeatEffect.Create },
           { 26, CharacterBoostWaterElementResistAlignmentFeatEffect.Create },
           { 27, CharacterBoostAirElementResistAlignmentFeatEffect.Create },
           { 28, CharacterBoostNeutralElementResistAlignmentFeatEffect.Create },
           { 29, CharacterBoostEarthElementPvpPercentAlignmentFeatEffect.Create },
           { 30, CharacterBoostFireElementPvpPercentAlignmentFeatEffect.Create },
           { 31, CharacterBoostWaterElementPvpPercentAlignmentFeatEffect.Create },
           { 32, CharacterBoostAirElementPvpPercentAlignmentFeatEffect.Create },
           { 33, CharacterBoostNeutralElementPvpPercentAlignmentFeatEffect.Create },
           { 34, CharacterBoostEarthElementPvpResistAlignmentFeatEffect.Create },
           { 35, CharacterBoostFireElementPvpResistAlignmentFeatEffect.Create },
           { 36, CharacterBoostWaterElementPvpResistAlignmentFeatEffect.Create },
           { 37, CharacterBoostAirElementPvpResistAlignmentFeatEffect.Create },
           { 38, CharacterBoostNeutralElementPvpResistAlignmentFeatEffect.Create },
           { 39, CharacterBoostHealBonusAlignmentFeatEffect.Create },
           { 40, CharacterBoostMagicFindAlignmentFeatEffect.Create }
       }.ToFrozenDictionary();

    /// <summary>
    /// Creates an <see cref="IAlignmentFeatEffect"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the alignment feat effect.</param>
    /// <param name="parameters">The parameters of the alignment feat effect.</param>
    /// <returns>The created <see cref="IAlignmentFeatEffect"/> if successful; otherwise, an <see cref="ErroredAlignmentFeatEffect"/> or <see cref="UntranslatedAlignmentFeatEffect"/> instance.</returns>
    public static IAlignmentFeatEffect Create(int id, params int[] parameters)
    {
        if (s_factories.TryGetValue(id, out var builder))
        {
            var alignmentFeatEffect = builder(id, parameters);
            if (alignmentFeatEffect is not null)
            {
                return alignmentFeatEffect;
            }

            Log.Error("Failed to create AlignmentFeatEffect {AlignmentFeatEffectId} from {@AlignmentFeatEffectParameters}",
                id,
                parameters);
            return new ErroredAlignmentFeatEffect(id, parameters);
        }

        Log.Warning("Unknown AlignmentFeatEffect {AlignmentFeatEffectId} from {@AlignmentFeatEffectParameters}",
            id,
            parameters);
        return new UntranslatedAlignmentFeatEffect(id, parameters);
    }
}
