using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;

using System.Collections.Frozen;
using System.Text.Json;

namespace Cyberia.Api.Factories;

public readonly record struct EffectParameters(long Param1, long Param2, long Param3, string Param4);

public static class EffectFactory
{
    private static readonly FrozenDictionary<int, Func<int, EffectParameters, int, int, CriteriaReadOnlyCollection, EffectArea, IEffect>> s_factory =
        new Dictionary<int, Func<int, EffectParameters, int, int, CriteriaReadOnlyCollection, EffectArea, IEffect>>()
        {
            { 4, CharacterTeleportOnSameMapEffect.Create },
            { 5, CharacterPushEffect.Create },
            { 6, CharacterPullEffect.Create },
            { 7, CharacterDivorceWifeOrHusbandEffect.Create },
            { 8, CharacterExchangePlacesEffect.Create },
            { 9, CharacterDodgeHitEffect.Create },
            { 10, CharacterLearnEmoteEffect.Create },
            { 34, QuestStartEffect.Create },
            { 35, QuestEndEffect.Create },
            { 50, CarryCharacterEffect.Create },
            { 51, ThrowCarriedCaracterEffect.Create },
            { 77, CharacterMovementPointsStealEffect.Create },
            { 78, CharacterMovementPointsWinEffect.Create },
            { 79, CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect.Create },
            { 81, CharacterLifePointsWinWithoutElementEffect.Create },
            { 82, CharacterLifePointsStealWithoutBoostEffect.Create },
            { 84, CharacterActionPointsStealEffect.Create },
            { 85, CharacterLifePointsLostBasedOnCasterLifeFromWaterEffect.Create },
            { 86, CharacterLifePointsLostBasedOnCasterLifeFromEarthEffect.Create },
            { 87, CharacterLifePointsLostBasedOnCasterLifeFromAirEffect.Create },
            { 88, CharacterLifePointsLostBasedOnCasterLifeFromFireEffect.Create },
            { 89, CharacterLifePointsLostBasedOnCasterLifeEffect.Create },
            { 90, CharacterDispatchLifePointsPercentEffect.Create },
            { 91, CharacterLifePointsStealFromWaterEffect.Create },
            { 92, CharacterLifePointsStealFromEarthEffect.Create },
            { 93, CharacterLifePointsStealFromAirEffect.Create },
            { 94, CharacterLifePointsStealFromFireEffect.Create },
            { 95, CharacterLifePointsStealEffect.Create },
            { 96, CharacterLifePointsLostFromWaterEffect.Create },
            { 97, CharacterLifePointsLostFromEarthEffect.Create },
            { 98, CharacterLifePointsLostFromAirEffect.Create },
            { 99, CharacterLifePointsLostFromFireEffect.Create },
            { 100, CharacterLifePointsLostEffect.Create },
            { 101, CharacterActionPointsLostEffect.Create },
            { 105, CharacterLifeLostModeratorEffect.Create },
            { 106, CharacterSpellReflectorEffect.Create },
            { 107, CharacterLifeLostReflectorEffect.Create },
            { 108, CharacterLifePointsWinFromFireEffect.Create },
            { 109, CharacterLifePointsLostCasterEffect.Create },
            { 110, CharacterBoostLifePointsEffect.Create },
            { 111, CharacterBoostActionPointsEffect.Create },
            { 112, CharacterBoostDamagesEffect.Create },
            { 113, CharacterDoubleReceivedDamageOrGiveLifeEffect.Create },
            { 114, CharacterMultiplyDamagesEffect.Create },
            { 115, CharacterBoostCriticalHitEffect.Create },
            { 116, CharacterDeboostRangeEffect.Create },
            { 117, CharacterBoostRangeEffect.Create },
            { 118, CharacterBoostStrengthEffect.Create },
            { 119, CharacterBoostAgilityEffect.Create },
            { 120, CharacterActionPointsWinEffect.Create },
            { 121, CharacterBoostDamagesForAllGameEffect.Create },
            { 122, CharacterBoostCriticalMissEffect.Create },
            { 123, CharacterBoostChanceEffect.Create },
            { 124, CharacterBoostWisdomEffect.Create },
            { 125, CharacterBoostVitalityEffect.Create },
            { 126, CharacterBoostIntelligenceEffect.Create },
            { 127, CharacterMovementPointsLostEffect.Create },
            { 128, CharacterBoostMovementPointsEffect.Create },
            { 130, CharacterStealGoldEffect.Create },
            { 131, CharacterManaUseKillLifeFireEffect.Create },
            { 132, CharacterRemoveAllEffectsEffect.Create },
            { 133, CharacterActionPointsLostCasterEffect.Create },
            { 134, CharacterMovementPointsLostCasterEffect.Create },
            { 135, CharacterDeboostRangeCasterEffect.Create },
            { 136, CharacterBoostRangeCasterEffect.Create },
            { 137, CharacterBoostDamagesCasterEffect.Create },
            { 138, CharacterBoostDamagesPercentEffect.Create },
            { 139, CharacterEnergyPointsWinEffect.Create },
            { 140, CharacterPassNextTurnEffect.Create },
            { 141, CharacterKillEffect.Create },
            { 142, CharacterBoostPhysicalDamagesEffect.Create },
            { 143, CharacterLifePointsWinWithoutBoostEffect.Create },
            { 144, CharacterLifePointsLostNoBoostEffect.Create },
            { 145, CharacterDeboostDamagesEffect.Create },
            { 146, CharacterCurseEffect.Create },
            { 147, CharacterResurectAllyInFightEffect.Create },
            { 148, CharacterAddFollowingCharacterEffect.Create },
            { 149, CharacterChangeLookEffect.Create },
            { 150, CharacterMakeInvisibleEffect.Create },
            { 152, CharacterDeboostChanceEffect.Create },
            { 153, CharacterDeboostVitalityEffect.Create },
            { 154, CharacterDeboostAgilityEffect.Create },
            { 155, CharacterDeboostIntelligenceEffect.Create },
            { 156, CharacterDeboostWisdomEffect.Create },
            { 157, CharacterDeboostStrengthEffect.Create },
            { 158, CharacterBoostMaximumWeightEffect.Create },
            { 159, CharacterDeboostMaximumWeightEffect.Create },
            { 160, CharacterBoostActionPointsLostDodgeEffect.Create },
            { 161, CharacterBoostMovementPointsLostDodgeEffect.Create },
            { 162, CharacterDeboostActionPointsLostDodgeEffect.Create },
            { 163, CharacterDeboostMovementPointsLostDodgeEffect.Create },
            { 164, CharacterLifeLostPercentReductorEffect.Create },
            { 165, CharacterBoostOneWeaponDamagePercentEffect.Create },
            { 166, CharacterActionPointsReflectorEffect.Create },
            { 168, CharacterDeboostActionPointsEffect.Create },
            { 169, CharacterDeboostMovementPointsEffect.Create },
            { 171, CharacterDeboostCriticalHitEffect.Create },
            { 172, CharacterDeboostMagicalReductionEffect.Create },
            { 173, CharacterDeboostPhysicalReductionEffect.Create },
            { 174, CharacterBoostInitiativeEffect.Create },
            { 175, CharacterDeboostInitiativeEffect.Create },
            { 176, CharacterBoostMagicFindEffect.Create },
            { 177, CharacterDeboostMagicFindEffect.Create },
            { 178, CharacterBoostHealBonusEffect.Create },
            { 179, CharacterDeboostHealBonusEffect.Create },
            { 180, CharacterAddDoubleUseSummonSlotEffect.Create },
            { 181, SummonCreatureEffect.Create },
            { 182, CharacterBoostMaximumSummonedCreaturesEffect.Create },
            { 183, CharacterBoostMagicalReductionEffect.Create },
            { 184, CharacterBoostPhysicalReductionEffect.Create },
            { 185, SummonStaticCreatureEffect.Create },
            { 186, CharacterDeboostDamagesPercentEffect.Create },
            { 188, CharacterAlignmentSideSetEffect.Create },
            { 192, CharacterInventoryRemoveItemEffect.Create },
            { 193, CharacterInventoryAddItemCheckEffect.Create },
            { 194, CharacterInventoryGainKamasEffect.Create },
            { 197, CharacterTransformEffect.Create },
            { 201, DecorsAddObjectEffect.Create },
            { 202, DecorsRevealUnvisibleEffect.Create },
            { 206, CharacterResurrectionEffect.Create },
            { 208, CharacterDisplaySpellAnimationEffect.Create },
            { 210, CharacterBoostEarthElementPercentEffect.Create },
            { 211, CharacterBoostWaterElementPercentEffect.Create },
            { 212, CharacterBoostAirElementPercentEffect.Create },
            { 213, CharacterBoostFireElementPercentEffect.Create },
            { 214, CharacterBoostNeutralElementPercentEffect.Create },
            { 215, CharacterDeboostEarthElementPercentEffect.Create },
            { 216, CharacterDeboostWaterElementPercentEffect.Create },
            { 217, CharacterDeboostAirElementPercentEffect.Create },
            { 218, CharacterDeboostFireElementPercentEffect.Create },
            { 219, CharacterDeboostNeutralElementPercentEffect.Create },
            { 220, CharacterReflectorUnboostedEffect.Create },
            { 221, CharacterInventoryAddItemRandomNoCheckEffect.Create },
            { 222, CharacterInventoryAddItemFromRandomDropEffect.Create },
            { 225, CharacterBoostTrapEffect.Create },
            { 226, CharacterBoostTrapPercentEffect.Create },
            { 228, CharacterDisplaySpellAnimation2Effect.Create },
            { 229, CharacterGainRideEffect.Create },
            { 230, CharacterEnergyLossBoostEffect.Create },
            { 233, CharacterInventoryRemoveItemAroundEffect.Create },
            { 239, CharacterTransform2Effect.Create },
            { 240, CharacterBoostEarthElementResistEffect.Create },
            { 241, CharacterBoostWaterElementResistEffect.Create },
            { 242, CharacterBoostAirElementResistEffect.Create },
            { 243, CharacterBoostFireElementResistEffect.Create },
            { 244, CharacterBoostNeutralElementResistEffect.Create },
            { 245, CharacterDeboostEarthElementResistEffect.Create },
            { 246, CharacterDeboostWaterElementResistEffect.Create },
            { 247, CharacterDeboostAirElementResistEffect.Create },
            { 248, CharacterDeboostFireElementResistEffect.Create },
            { 249, CharacterDeboostNeutralElementResistEffect.Create },
            { 250, CharacterBoostEarthElementPvpPercentEffect.Create },
            { 251, CharacterBoostWaterElementPvpPercentEffect.Create },
            { 252, CharacterBoostAirElementPvpPercentEffect.Create },
            { 253, CharacterBoostFireElementPvpPercentEffect.Create },
            { 254, CharacterBoostNeutralElementPvpPercentEffect.Create },
            { 255, CharacterDeboostEarthElementPvpPercentEffect.Create },
            { 256, CharacterDeboostWaterElementPvpPercentEffect.Create },
            { 257, CharacterDeboostAirElementPvpPercentEffect.Create },
            { 258, CharacterDeboostFireElementPvpPercentEffect.Create },
            { 259, CharacterDeboostNeutralElementPvpPercentEffect.Create },
            { 260, CharacterBoostEarthElementPvpResistEffect.Create },
            { 261, CharacterBoostWaterElementPvpResistEffect.Create },
            { 262, CharacterBoostAirElementPvpResistEffect.Create },
            { 263, CharacterBoostFireElementPvpResistEffect.Create },
            { 264, CharacterBoostNeutralElementPvpResistEffect.Create },
            { 265, CharacterLifeLostCasterModeratorEffect.Create },
            { 266, CharacterStealChanceEffect.Create },
            { 267, CharacterStealVitalityEffect.Create },
            { 268, CharacterStealAgilityEffect.Create },
            { 269, CharacterStealIntelligenceEffect.Create },
            { 270, CharacterStealWisdomEffect.Create },
            { 271, CharacterStealStrengthEffect.Create },
            { 275, CharacterLifePointsLostBasedOnCasterLifeMissingFromWaterEffect.Create },
            { 276, CharacterLifePointsLostBasedOnCasterLifeMissingFromEarthEffect.Create },
            { 277, CharacterLifePointsLostBasedOnCasterLifeMissingFromAirEffect.Create },
            { 278, CharacterLifePointsLostBasedOnCasterLifeMissingFromFireEffect.Create },
            { 279, CharacterLifePointsLostBasedOnCasterLifeMissingEffect.Create },
            { 280, BoostSpellRangeMinEffect.Create },
            { 281, BoostSpellRangeMaxEffect.Create },
            { 282, BoostSpellRangeableEffect.Create },
            { 283, BoostSpellDamagesEffect.Create },
            { 284, BoostSpellHealEffect.Create },
            { 285, BoostSpellActionPointsCostEffect.Create },
            { 286, BoostSpellCastIntervalEffect.Create },
            { 287, BoostSpellCriticalHitRateEffect.Create },
            { 288, BoostSpellCastOutLineEffect.Create },
            { 289, BoostSpellNoLineOfSightEffect.Create },
            { 290, BoostSpellMaxPertTurnEffect.Create },
            { 291, BoostSpellMaxPerTargetEffect.Create },
            { 292, BoostSpellCastIntervalSetEffect.Create },
            { 293, BoostSpellBaseDamageEffect.Create },
            { 294, DeboostSpellRangeMaxEffect.Create },
            { 295, DeboostSpellRangeMinEffect.Create },
            { 296, DeboostSpellActionPointsCostEffect.Create },
            { 297, DeboostSpellOccupiedCellEffect.Create },
            { 298, DeboostSpellFreeCellEffect.Create },
            { 299, BoostSpellFreeCellEffect.Create },
            { 300, LaunchSpellLevelEffect.Create },
            { 320, CharacterStealRangeEffect.Create },
            { 333, CharacterChangeColorEffect.Create }, //Check params and description
            { 335, CharacterAddAppearanceEffect.Create }, //Check params and description
            { 400, FightAddTrapCastingSpellEffect.Create },
            { 401, FightAddGlyphCastingSpellEffect.Create },
            { 402, FightAddGlyphCastingSpellEndTurnEffect.Create },
            { 405, FightKillAndSummonEffect.Create },
            { 406, CharacterDispellSpellEffect.Create },
            { 448, FarmObjectEfficacityEffect.Create },
            { 513, AddPrismeEffect.Create },
            { 521, ItemUnbreakableEffect.Create },
            { 600, GotoWaypointEffect.Create },
            { 601, GotoMapEffect.Create },
            { 602, SaveWaypointEffect.Create },
            { 603, CharacterLearnJobEffect.Create },
            { 604, CharacterLearnSpellLevelEffect.Create },
            { 605, CharacterGainXpEffect.Create },
            { 606, CharacterGainWisdomEffect.Create },
            { 607, CharacterGainStrengthEffect.Create },
            { 608, CharacterGainChanceEffect.Create },
            { 609, CharacterGainAgilityEffect.Create },
            { 610, CharacterGainVitalityEffect.Create },
            { 611, CharacterGainIntelligenceEffect.Create },
            { 612, CharacterGainStatsPointsEffect.Create },
            { 613, CharacterGainSpellPointsEffect.Create },
            { 614, CharacterGainJobXpEffect.Create },
            { 615, CharacterUnlearnJobEffect.Create },
            { 616, CharacterUnlearnSpellEffect.Create },
            { 620, CharacterReadBookEffect.Create },
            { 621, ChracterSummonMonsterEffect.Create },
            { 622, GotoHouseEffect.Create },
            { 623, CharacterSummonMonsterGroupEffect.Create },
            { 624, CharacterUnlearnGuildSpellEffect.Create },
            { 626, CharacterResetCharacsEffect.Create },
            { 627, CharacterSummonMonsterGroupSetMapEffect.Create }, //Check params and description when used
            { 628, CharacterSummonMonsterGroupDynamicEffect.Create },
            { 629, CharacterLearnSpellEffect.Create },
            { 640, CharacterGainHonourEffect.Create },
            { 641, CharacterGainDishonourEffect.Create },
            { 642, CharacterLoseHonourEffect.Create },
            { 643, CharacterLoseDishonourEffect.Create },
            { 645, CharacterMapResurectionAlliesEffect.Create },
            { 646, MapHealAlliesEffect.Create },
            { 647, MapForceEnnemiesGhostEffect.Create },
            { 648, ForceEnnemyGhostEffect.Create },
            { 649, FakeAlignmentEffect.Create },
            { 666, NoopEffect.Create },
            { 667, KillFightEffect.Create },
            { 669, IncarnationEffect.Create },
            { 670, CharacterLifePointsLostBasedOnCasterLifeReducedByCasterEffect.Create },
            { 671, CharacterLifePointsLostBasedOnCasterLifeNotReducedEffect.Create },
            { 672, CharacterLifePointsLostBasedOnCasterLifeMidLifeEffect.Create },
            { 699, CharacterReferencementEffect.Create },
            { 700, ItemChangeEffectEffect.Create }, //Check params and description
            { 701, ItemAddEffectEffect.Create },
            { 702, ItemAddDurabilityEffect.Create },
            { 705, CaptureSoulEffect.Create },
            { 706, CaptureRideEffect.Create },
            { 710, CharacterAddCostToActionEffect.Create },
            { 715, LadderSuperRaceEffect.Create },
            { 716, LadderRaceEffect.Create },
            { 717, LadderIdEffect.Create },
            { 720, PvpLadderEffect.Create },
            { 721, GhoulOwnerEffect.Create }, //Check params and description
            { 722, CharacterLearnSpellTemporaryEffect.Create },
            { 723, GainAuraEffect.Create },
            { 724, GainTitleEffect.Create },
            { 725, CharacterRenameInvalidGuildEffect.Create },
            { 730, TeleportNearestPrismEffect.Create },
            { 731, AutoAggressEnemyPlayerEffect.Create },
            { 740, ShushuStackRuneWeaponEffect.Create },
            { 741, ShushuStockedRuneEffect.Create },
            { 742, ShushuStackRuneEffect.Create },
            { 750, BoostSoulCaptureBonusEffect.Create },
            { 751, BoostRideXpBonusEffect.Create },
            { 760, RemoveOnMoveEffect.Create },
            { 765, CharacterSacrifyEffect.Create },
            { 770, ClockwiseConfusionDegreeEffect.Create },
            { 771, ClockwiseConfusionPi2Effect.Create },
            { 772, ClockwiseConfusionPi4Effect.Create },
            { 773, CounterClockwiseConfusionDegreeEffect.Create },
            { 774, CounterClockwiseConfusionPi2Effect.Create },
            { 775, CounterClockwiseConfusionPi4Effect.Create },
            { 776, CharacterBoostPermanentDamagePercentEffect.Create },
            { 780, CharacterSummonDeadAllyInFightEffect.Create },
            { 781, CharacterUnluckyEffect.Create },
            { 782, CharacterMaximizeRollEffect.Create },
            { 783, CharacterPushUpToEffect.Create },
            { 784, CharacterTeleportToFightStartPosEffect.Create },
            { 785, CharacterWalkFourDirEffect.Create },
            { 786, CharacterHealAttackersEffect.Create },
            { 787, LaunchSpellEffect.Create },
            { 788, CharacterPunishmentEffect.Create },
            { 791, MarkTargetMercenaryEffect.Create },
            { 795, HuntToolEffect.Create },
            { 800, ItemPetsLifePointsEffect.Create },
            { 805, ItemChangeDurationEffect.Create },
            { 806, ItemPetsShapeEffect.Create },
            { 807, ItemPetsEatEffect.Create },
            { 808, PetsLastMealEffect.Create },
            { 810, SizeEffect.Create },
            { 811, ItemBuffChangeDurationEffect.Create },
            { 812, ItemChangeDurabilityEffect.Create },
            { 814, ItemDungeonKeyDateEffect.Create },
            { 825, ItemTeleportForceEffect.Create },
            { 826, ItemTeleportMapReferenceEffect.Create },
            { 830, GotoGuildPropertyEffect.Create },
            { 850, ChangeNameEffect.Create },
            { 851, ChangeColorsEffect.Create },
            { 852, ChangeSexeEffect.Create },
            { 853, MimysymbicEffect.Create },
            { 856, ShowCardCollectionEffect.Create },
            { 905, FightChallengeAgainstMonsterEffect.Create },
            { 930, RideIncreaseSerenityLowerAggressivenessEffect.Create },
            { 931, RideIncreaseAggressivenessLowerSerenityEffect.Create },
            { 932, RideIncreaseEnduranceEffect.Create },
            { 933, RideLowerEnduranceEffect.Create },
            { 934, RideIncreaseLoveEffect.Create },
            { 935, RideLowerLoveEffect.Create },
            { 936, RideSpeedMaturityEffect.Create },
            { 937, RideSlowMaturityEffect.Create },
            { 939, PetSetPowerBoostEffect.Create },
            { 940, PetPowerBoostEffect.Create },
            { 945, RideGainAbilityEffect.Create },
            { 946, FarmTempWithdrawItemEffect.Create },
            { 947, FarmWithdrawItemEffect.Create },
            { 948, FarmPlaceItemEffect.Create },
            { 949, MountRideEffect.Create },
            { 950, FightSetStateEffect.Create },
            { 951, FightUnsetStateEffect.Create },
            { 960, HuntTargetAlignmentEffect.Create },
            { 961, HuntTargetRankEffect.Create },
            { 962, HuntTargetLevelEffect.Create },
            { 963, CreatedSinceEffect.Create },
            { 964, HuntTargetNameEffect.Create },
            { 969, ItemMimysymbicAppearanceEffect.Create },
            { 970, ItemLivingIdEffect.Create },
            { 971, ItemLivingMoodEffect.Create },
            { 972, ItemLivingSkinEffect.Create },
            { 973, ItemLivingCategoryEffect.Create },
            { 974, ItemLivingLevelEffect.Create },
            { 975, ItemLivingMaxSkinEffect.Create },
            { 983, MarkNotTradableEffect.Create },
            { 984, MarkLegitOwnerEffect.Create }, //Check params and description when used
            { 985, SetCraftermageEffect.Create },
            { 986, MarkTargetEffect.Create },
            { 987, SetOwnerEffect.Create },
            { 988, SetCrafterEffect.Create },
            { 989, SeekTargetEffect.Create },
            { 990, ShowTextEffect.Create },
            { 994, RideInvalidEffect.Create },
            { 995, RideDetailsEffect.Create },
            { 996, RideOwnerEffect.Create },
            { 997, RideNameEffect.Create },
            { 998, RideCertificateValidityEffect.Create },
            { 999, ItemTeleportEffect.Create },
            { 2001, CharacterInventoryGainKamasEffect.Create },
            { 2008, CharacterBoostDealtDamagePercentMultiplierEffect.Create },
            { 2009, CharacterDeboostDealtDamagePercentMultiplierEffect.Create },
            { 2010, CharacterBoostChargeEffect.Create },
            { 2011, CharacterDeboostChargeEffect.Create },
            { 2050, CharacterGainXpFromLevelEffect.Create },
            { 2100, CharacterActionPointsLostEffect.Create }, //Duplicate
            { 2101, GiveTTGCardFromFamilyEffect.Create },
            { 2102, AddTTGCardToBinderEffect.Create },
            { 2107, GiveTTGCardFromRarityEffect.Create },
            { 2111, CharacterDeboostReflectorUnboostedEffect.Create },
            { 2112, CharacterDeboostMaximumSummonedCreaturesEffect.Create },
            { 2113, CharacterDeboostTrapEffect.Create },
            { 2114, CharacterDeboostTrapPercentEffect.Create },
            { 2116, GladiatroolBoostExtraEffectEffect.Create },
            { 2117, GladiatroolBoostNoEffectEffect.Create },
            { 2118, CharacterDeboostLifePointsEffect.Create },
            { 2123, CharacterReduceWeaponCostEffect.Create },
            { 2124, CharacterBoostRangeForAllSpellEffect.Create },
            { 2126, CharacterLifePointsLostFromBestElementEffect.Create },
            { 2127, GetPulledEffect.Create },
            { 2128, FightSetStateEffect.Create }, //Duplicate check params and description
            { 2129, FightUnsetStateEffect.Create }, //Duplicate check params and description
            { 2130, FightSaveCurrentPositionEffect.Create },
            { 2131, FightRollbackPreviousPositionEffect.Create },
            { 2132, FightDrawOneCardEffect.Create },
            { 2133, FightReshuffleHandEffect.Create },
            { 2134, CharacterLifePointsStealFromBestElementEffect.Create },
            { 2135, CharacterStealAllCaracsEffect.Create },
            { 2136, CharacterLifePointsLostBasedOnCasterLifeEffect.Create }, //Duplicate
            { 2137, FightSetStateEffect.Create }, //Duplicate check params and description
            { 2138, BoostSpellDamagePercentEffect.Create },
            { 2139, FightTeleswapMirrorCasterEffect.Create },
            { 2142, ItemChangeEffectEffect.Create }, //Duplicate
            { 2143, TeleportCreatureOnSameMapEffect.Create },
            { 2144, AddMonsterToFightEffect.Create },
            { 2146, ItemFragmentCompletionEffect.Create },
            { 2147, ItemCurrentSoulEaterEffect.Create },
            { 2148, ItemMaxSoulEaterEffect.Create },
            { 2149, ItemCeremonialChangeDurabilityEffect.Create },
            { 2150, ReplaceEffect.Create },
            { 2151, MarkNeverTradableStrongEffect.Create },
            { 2152, CharacterInventoryAddItemRandomQuantityEffect.Create },
            { 2153, TeleportGroupMemberEffect.Create },
            { 2154, LockToAccountEffect.Create },
            { 2155, LockToAccountUntilEffect.Create }
        }.ToFrozenDictionary();

    public static IEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        if (s_factory.TryGetValue(effectId, out var builder))
        {
            return builder(effectId, parameters, duration, probability, criteria, effectArea);
        }

        Log.Warning("Unknown Effect {EffectId} with {@EffectParameters}", effectId, parameters);
        return UntranslatedEffect.Create(effectId, parameters, duration, probability, criteria, effectArea);
    }

    public static IEffect Create(string compressedEffect)
    {
        var args = compressedEffect.Split('#');

        if (args.Length == 0)
        {
            Log.Error("Failed to create Effect from {CompressedEffect}", compressedEffect);
            return ErroredEffect.Create(compressedEffect);
        }

        var id = (int)args[0].ToInt64OrDefaultFromHex();
        EffectParameters parameters = new()
        {
            Param1 = args.Length > 1 ? args[1].ToInt64OrDefaultFromHex() : 0,
            Param2 = args.Length > 2 ? args[2].ToInt64OrDefaultFromHex() : 0,
            Param3 = args.Length > 3 ? args[3].ToInt64OrDefaultFromHex() : 0,
            Param4 = args.Length > 4 ? args[4] : string.Empty
        };

        return Create(id, parameters, 0, 0, [], EffectAreaFactory.Default);
    }

    public static IEnumerable<IEffect> CreateMany(string compressedEffects)
    {
        foreach (var compressedEffect in compressedEffects.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            yield return Create(compressedEffect);
        }
    }

    public static IEffect Create(JsonElement compressedEffect, EffectArea effectArea)
    {
        if (compressedEffect.ValueKind is not JsonValueKind.Array)
        {
            Log.Error("Failed to create Effect from {@CompressedEffect}", compressedEffect);
            return ErroredEffect.Create(compressedEffect.ToString());
        }

        var length = compressedEffect.GetArrayLength();
        if (length < 7)
        {
            Log.Error("Failed to create Effect from {@CompressedEffect}", compressedEffect);
            return ErroredEffect.Create(compressedEffect.ToString());
        }

        var id = compressedEffect[0].GetInt32OrDefault();
        if (id == default)
        {
            Log.Error("Failed to create Effect from {@CompressedEffect}", compressedEffect);
            return ErroredEffect.Create(compressedEffect.ToString());
        }

        var parameters = new EffectParameters()
        {
            Param1 = compressedEffect[1].GetInt64OrDefault(),
            Param2 = compressedEffect[2].GetInt64OrDefault(),
            Param3 = compressedEffect[3].GetInt64OrDefault(),
            Param4 = length > 7 ? compressedEffect[7].GetStringOrEmpty() : string.Empty
        };
        var duration = compressedEffect[4].GetInt32OrDefault();
        var probability = compressedEffect[5].GetInt32OrDefault();
        var criteria = CriterionFactory.CreateMany(compressedEffect[6].GetStringOrEmpty());

        return Create(id, parameters, duration, probability, criteria, effectArea);
    }

    public static IEnumerable<IEffect> CreateMany(IReadOnlyList<JsonElement> compressedEffects, IReadOnlyList<EffectArea> effectAreas)
    {
        for (var i = 0; i < compressedEffects.Count; i++)
        {
            var effectArea = effectAreas.Count > i ? effectAreas[i] : EffectAreaFactory.Default;

            yield return Create(compressedEffects[i], effectArea);
        }
    }
}
