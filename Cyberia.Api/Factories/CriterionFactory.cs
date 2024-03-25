using Cyberia.Api.Factories.Criteria;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

public static class CriterionFactory
{
    private static readonly FrozenDictionary<string, Func<string, char, string[], ICriterion?>> _factory =
        new Dictionary<string, Func<string, char, string[], ICriterion?>>()
        {
            { "BI", UnusableItemCriterion.Create },
            { "CA", AgilityCriterion.Create },
            { "Ca", BaseAgilityCriterion.Create },
            { "CB", BestElementCriterion.Create },
            { "CC", ChanceCriterion.Create },
            { "Cc", BaseChanceCriterion.Create },
            { "CD", DisgracePointCriterion.Create },
            { "CH", HonorPointCriterion.Create },
            { "CI", IntelligenceCriterion.Create },
            { "Ci", BaseIntelligenceCriterion.Create },
            { "Cl", PercentVitalityCriterion.Create },
            { "CM", MovementPointCriterion.Create },
            { "Cm", CurrentMovementPointCriterion.Create },
            { "CO", HomeownerCriterion.Create },
            { "CP", ActionPointCriterion.Create },
            { "Cp", CurrentActionPointCriterion.Create },
            { "CS", StrengthCriterion.Create },
            { "Cs", BaseStrengthCriterion.Create },
            { "CV", VitalityCriterion.Create },
            { "Cv", BaseVitalityCriterion.Create },
            { "CW", WisdomCriterion.Create },
            { "Cw", BaseWisdomCriterion.Create },
            { "Cz", AvailableSummonCriterion.Create },
            { "FC", CardCombinationCriterion.Create },
            { "FS", StateCriterion.Create },
            { "FT", TurnCriterion.Create },
            { "Fz", MonsterSummonCriterion.Create },
            { "MK", MapPlayerCriterion.Create },
            { "Pa", AlignmentLevelCriterion.Create },
            { "PB", MapSubAreaCriterion.Create },
            { "PE", EmoteCriterion.Create },
            { "PG", BreedCriterion.Create },
            { "Pg", AlignmentFeatCriterion.Create },
            { "PJ", JobCriterion.Create },
            { "Pj", JobCriterion.Create },
            { "PK", KamasCriterion.Create },
            { "PL", LevelCriterion.Create },
            { "PM", LookCriterion.Create },
            { "Pm", MapCriterion.Create },
            { "PN", NameCriterion.Create },
            { "Pn", SlotCriterion.Create },
            { "PO", ItemCriterion.Create },
            { "PP", AlignmentRankCriterion.Create },
            { "PR", MaritalStatusCriterion.Create },
            { "Pr", AlignmentSpecializationCriterion.Create },
            { "PS", GenderCriterion.Create },
            { "Ps", AlignmentCriterion.Create },
            { "PW", FreeWeightCriterion.Create },
            { "PX", PlayerRightsCriterion.Create },
            { "PZ", SubscribeCriterion.Create },
            { "Qa", QuestCriterion.Create },
            { "Qc", QuestAvailableCriterion.Create },
            { "Qo", QuestObjectiveCriterion.Create },
            { "Qs", QuestStepCriterion.Create },
            { "Sc", ServerContentCriterion.Create },
            { "SI", ServerCriterion.Create },
            { "SM", MinuteCriterion.Create }
        }.ToFrozenDictionary();

    public static ICriterion Create(string id, char @operator, params string[] parameters)
    {
        if (_factory.TryGetValue(id, out var builder))
        {
            var criterion = builder(id, @operator, parameters);
            if (criterion is not null)
            {
                return criterion;
            }

            var compressedCriterion = $"{id}{@operator}{string.Join(",", parameters)}";

            Log.Error("Failed to create Criterion from {CompressedCriterion}", id, compressedCriterion);
            return ErroredCriterion.Create(compressedCriterion);
        }

        return UntranslatedCriterion.Create(id, @operator, parameters);
    }

    public static ICriterion Create(string compressedCriterion)
    {
        if (string.IsNullOrEmpty(compressedCriterion) || compressedCriterion.Length < 4)
        {
            Log.Error("Failed to create Criterion from {CompressedCriterion}", compressedCriterion);
            return ErroredCriterion.Create(compressedCriterion);
        }

        var id = compressedCriterion[0..2];
        var @operator = compressedCriterion[2];
        var parameters = compressedCriterion[3..].Split(',');

        return Create(id, @operator, parameters);
    }

    public static CriteriaCollection CreateMany(string compressedCriteria)
    {
        List<ICriteriaElement> criteria = [];

        var index = 0;
        while (index < compressedCriteria.Length)
        {
            switch (compressedCriteria[index])
            {
                case '(':
                    (var token, index) = ExtractToken(compressedCriteria, index + 1, ')');

                    criteria.Add(CreateMany(token));
                    break;
                case '&' or '|':
                    criteria.Add(new CriteriaLogicalOperator(compressedCriteria[index]));
                    break;
                default:
                    (token, index) = ExtractToken(compressedCriteria, index, '&', '|', ')');
                    index--;

                    criteria.Add(Create(token));
                    break;
            }

            index++;
        }

        return new CriteriaCollection(criteria);
    }

    public static (string, int) ExtractToken(string value, int startIndex, params char[] endChars)
    {
        var endIndex = startIndex;
        var openParenthesesCount = 0;

        while (endIndex < value.Length)
        {
            switch (value[endIndex])
            {
                case '(':
                    openParenthesesCount++;
                    break;
                case ')':
                    if (openParenthesesCount == 0)
                    {
                        return (value[startIndex..endIndex], endIndex);
                    }

                    openParenthesesCount--;
                    break;
                default:
                    if (openParenthesesCount == 0 && endChars.Contains(value[endIndex]))
                    {
                        return (value[startIndex..endIndex], endIndex);
                    }

                    break;
            }

            endIndex++;
        }

        return (value[startIndex..endIndex], endIndex);
    }
}
