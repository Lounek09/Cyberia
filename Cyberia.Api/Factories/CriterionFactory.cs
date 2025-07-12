using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.Criteria.Elements.Characteristics;
using Cyberia.Api.Factories.Criteria.Elements.Fights;
using Cyberia.Api.Factories.Criteria.Elements.Maps;
using Cyberia.Api.Factories.Criteria.Elements.Others;
using Cyberia.Api.Factories.Criteria.Elements.Players;
using Cyberia.Api.Factories.Criteria.Elements.Quests;
using Cyberia.Api.Factories.Criteria.Elements.Servers;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

/// <summary>
/// Provides factory methods for creating <see cref="ICriterion"/> or <see cref="CriteriaReadOnlyCollection"/>.
/// </summary>
public static class CriterionFactory
{
    /// <summary>
    /// A dictionary mapping criterion identifiers to their factory methods.
    /// </summary>
    private static readonly FrozenDictionary<string, Func<string, char, ReadOnlySpan<string>, ICriterion?>> s_factories =
        new Dictionary<string, Func<string, char, ReadOnlySpan<string>, ICriterion?>>()
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
            { "CL", RemainingVitalityCriterion.Create },
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
            { "Sd", DayOfMonthCriterion.Create },
            { "SG", MonthCriterion.Create },
            { "SI", ServerCriterion.Create },
            { "SM", MinuteCriterion.Create }
        }.ToFrozenDictionary();

    /// <summary>
    /// A dictionary mapping criterion operators to their value used in the criterion description key.
    /// </summary>
    private static readonly FrozenDictionary<char, string> s_operators =
        new Dictionary<char, string>()
        {
            { '=', "Equal" },
            { '!', "Different" },
            { '>', "Superior" },
            { '<', "Inferior" },
            { '~', "SoftEqual" },
            { 'E', "Equiped" },
            { 'X', "NotEquiped" }
        }.ToFrozenDictionary();

    /// <summary>
    /// Creates an <see cref="ICriterion"/> from the specified criterion id.
    /// </summary>
    /// <param name="id">The id of the criterion to create.</param>
    /// <param name="operator">The operator character indicating the type of comparison or operation.</param>
    /// <param name="parameters">The parameters of the criterion.</param>
    /// <returns>The created <see cref="ICriterion"/> if successful; otherwise, an <see cref="ErroredCriterion"/> or <see cref="UntranslatedCriterion"/> instance.</returns>
    public static ICriterion Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        string compressedCriterion;
        string[] arrayParameters;

        if (!s_factories.TryGetValue(id, out var builder))
        {
            arrayParameters = parameters.ToArray();
            compressedCriterion = $"{id}{@operator}{string.Join(',', arrayParameters)}";
            Log.Warning("Unknown Criterion {CompressedCriterion}", compressedCriterion);

            return new UntranslatedCriterion(id, @operator, arrayParameters, compressedCriterion);
        }

        var criterion = builder(id, @operator, parameters);
        if (criterion is null)
        {
            arrayParameters = parameters.ToArray();
            compressedCriterion = $"{id}{@operator}{string.Join(',', arrayParameters)}";
            Log.Error("Failed to create Criterion from {CompressedCriterion}", compressedCriterion);

            return new ErroredCriterion(id, @operator, arrayParameters, compressedCriterion);
        }

        return criterion;
    }

    /// <summary>
    /// Creates an <see cref="ICriterion"/> from a compressed string representation.
    /// </summary>
    /// <param name="compressedCriterion">The compressed string representation of the criterion.</param>
    /// <returns>The created <see cref="ICriterion"/> if successful; otherwise, an <see cref="ErroredCriterion"/> or <see cref="UntranslatedCriterion"/> instance.</returns>
    public static ICriterion Create(ReadOnlySpan<char> compressedCriterion)
    {
        const char separator = ',';

        if (compressedCriterion.Length < 4)
        {
            var compressedCriterionStr = compressedCriterion.ToString();
            Log.Error("Failed to create Criterion from {CompressedCriterion}", compressedCriterionStr);

            return new ErroredCriterion(compressedCriterionStr);
        }

        var id = compressedCriterion[0..2].ToString();
        var @operator = compressedCriterion[2];

        compressedCriterion = compressedCriterion[3..];
        if (compressedCriterion.IsEmpty)
        {
            return Create(id, @operator);
        }

        var parameterCount = compressedCriterion.Count(separator) + 1;
        if (parameterCount == 1)
        {
            return Create(id, @operator, compressedCriterion.ToString());
        }

        Span<string> parameters = new string[parameterCount];
        var index = 0;
        foreach (var range in compressedCriterion.Split(separator))
        {
            parameters[index++] = compressedCriterion[range].ToString();
        }

        return Create(id, @operator, parameters);
    }

    /// <summary>
    /// Creates a <see cref="CriteriaReadOnlyCollection"/> from a compressed string representation.
    /// </summary>
    /// <param name="compressedCriteria">The compressed string representation of the criteria.</param>
    /// <returns>The <see cref="CriteriaReadOnlyCollection"/> containing the parsed <see cref="ICriteriaElement"/>.</returns>
    public static CriteriaReadOnlyCollection CreateMany(ReadOnlySpan<char> compressedCriteria)
    {
        if (compressedCriteria.IsEmpty)
        {
            return CriteriaReadOnlyCollection.Empty;
        }

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
                        criteria.Add(Create(token));
                    }

                    criteria.Add(new CriteriaLogicalOperator(currentChar));
                    startTokenIndex = i + 1;
                    break;
                default:
                    if (i == length - 1)
                    {
                        var token = compressedCriteria[startTokenIndex..(i + 1)];
                        criteria.Add(Create(token));
                    }
                    break;
            }
        }

        return new CriteriaReadOnlyCollection(criteria);
    }

    /// <summary>
    /// Gets the value of the operator used in the criterion description key.
    /// </summary>
    /// <param name="operator">The operator character.</param>
    /// <returns>The human-readable description of the operator.</returns>
    internal static string GetOperatorDescriptionKey(char @operator)
    {
        return s_operators.TryGetValue(@operator, out var description)
            ? description
            : @operator.ToString();
    }
}
