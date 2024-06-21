using Cyberia.Api.Factories.Criteria;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

public static class CriterionFactory
{
    private static readonly FrozenDictionary<string, Func<string, char, string[], ICriterion?>> s_factory =
        new Dictionary<string, Func<string, char, string[], ICriterion?>>()
        {
            { "BI", UnusableItemCriterion.Create },
            { "CA", AgilityCriterion.Create },
            { "Ca", BaseAgilityCriterion.Create },
            { "CB", BestElementCriterion.Create },
            { "CC", ChanceCriterion.Create },
            { "Cc", BaseChanceCriterion.Create },
            { "CD", DisgracePointCriterion.Create },
            { "CE", MaxEnergyPointsCriterion.Create },
            { "Ce", EnergyPointsCriterion.Create },
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
            { "Mt", DungeonCriterion.Create },
            { "Pa", AlignmentLevelCriterion.Create },
            { "PB", MapSubAreaCriterion.Create },
            { "PE", EmoteCriterion.Create },
            { "PG", BreedCriterion.Create },
            { "Pg", AlignmentFeatCriterion.Create },
            { "PJ", JobCriterion.Create },
            { "Pj", JobCriterion.Create },
            { "PK", KamasCriterion.Create },
            { "Pk", LevelModulationCriterion.Create },
            { "PL", LevelCriterion.Create },
            { "PM", LookCriterion.Create },
            { "Pm", MapCriterion.Create },
            { "PN", NameCriterion.Create },
            { "Pn", SlotCriterion.Create },
            { "Po", MapAreaCriterion.Create },
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
            { "Qf", QuestFinishedCriterion.Create },
            { "QF", QuestFinishedCountCriterion.Create },
            { "Qo", QuestObjectiveCriterion.Create },
            { "Qs", QuestStepCriterion.Create },
            { "Sc", ServerContentCriterion.Create },
            { "SI", ServerCriterion.Create },
            { "SM", MinuteCriterion.Create }
        }.ToFrozenDictionary();

    public static ICriterion Create(string id, char @operator, params string[] parameters)
    {
        string compressedCriterion;

        if (s_factory.TryGetValue(id, out var builder))
        {
            var criterion = builder(id, @operator, parameters);
            if (criterion is not null)
            {
                return criterion;
            }

            compressedCriterion = $"{id}{@operator}{string.Join(',', parameters)}";
            Log.Error("Failed to create Criterion from {CompressedCriterion}", compressedCriterion);
            return ErroredCriterion.Create(compressedCriterion);
        }

        compressedCriterion = $"{id}{@operator}{string.Join(',', parameters)}";
        Log.Warning("Unknown Criterion {CompressedCriterion}", compressedCriterion);
        return UntranslatedCriterion.Create(id, @operator, compressedCriterion, parameters);
    }

    public static ICriterion Create(string compressedCriterion)
    {
        if (compressedCriterion.Length < 4)
        {
            var compressedCriterionStr = compressedCriterion.ToString();
            Log.Error("Failed to create Criterion from {CompressedCriterion}", compressedCriterionStr);
            return ErroredCriterion.Create(compressedCriterionStr);
        }

        var id = compressedCriterion[0..2];
        var @operator = compressedCriterion[2];
        var parameters = compressedCriterion[3..].Split(',');

        return Create(id, @operator, parameters);
    }

    public static CriteriaReadOnlyCollection CreateMany(ReadOnlySpan<char> compressedCriteria)
    {
        List<ICriteriaElement> criteria = [];

        var nesting = 0;
        var startTokenIndex = 0;

        var length = compressedCriteria.Length;
        for (var i = 0; i < length; i++)
        {
            var currentChar = compressedCriteria[i];

            switch (currentChar)
            {
                case '(':
                    if (nesting == 0)
                    {
                        startTokenIndex = i + 1;
                    }

                    nesting++;
                    break;
                case ')':
                    nesting--;

                    if (nesting == 0)
                    {
                        var token = compressedCriteria[startTokenIndex..i];
                        criteria.Add(CreateMany(token));
                        startTokenIndex = i + 1;
                    }
                    break;
                case '&' or '|' when nesting == 0:
                    if (i > startTokenIndex)
                    {
                        var token = compressedCriteria[startTokenIndex..i];
                        criteria.Add(Create(token.ToString()));
                    }

                    criteria.Add(new CriteriaLogicalOperator(currentChar));
                    startTokenIndex = i + 1;
                    break;
                default:
                    if (i == length - 1)
                    {
                        var token = compressedCriteria[startTokenIndex..(i + 1)];
                        criteria.Add(Create(token.ToString()));
                    }
                    break;
            }
        }

        return new CriteriaReadOnlyCollection(criteria);
    }

    internal static string GetCriterionOperatorDescriptionName(char @operator)
    {
        return @operator switch
        {
            '=' => "Equal",
            '!' => "Different",
            '>' => "Superior",
            '<' => "Inferior",
            '~' => "SoftEqual",
            'E' => "Equiped",
            'X' => "NotEquiped",
            _ => @operator.ToString(),
        };
    }
}
