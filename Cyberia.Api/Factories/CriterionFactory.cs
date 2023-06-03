using Cyberia.Api.Factories.Criteria.CharacteristicCriteria;
using Cyberia.Api.Factories.Criteria.MapCriteria;
using Cyberia.Api.Factories.Criteria.OtherCriteria;
using Cyberia.Api.Factories.Criteria.PlayerCriteria;
using Cyberia.Api.Factories.Criteria.QuestCriteria;
using Cyberia.Api.Factories.Criteria.ServerCriteria;

namespace Cyberia.Api.Factories
{
    public static class CriterionFactory
    {
        private static readonly char[] _logicalOperators = new char[] { '&', '|' };
        private static readonly Dictionary<string, Func<char, string[], string?>> _factory = new()
        {
            { "BI", UnusableItemCriterion.GetValue },
            { "CA", AgilityCriterion.GetValue },
            { "Ca", BaseAgilityCriterion.GetValue },
            { "CC", ChanceCriterion.GetValue },
            { "Cc", BaseChanceCriterion.GetValue },
            { "CD", DisgracePointsCriterion.GetValue },
            { "CH", HonorPointsCriterion.GetValue },
            { "CI", IntelligenceCriterion.GetValue },
            { "Ci", BaseIntelligenceCriterion.GetValue },
            { "CM", MovementPointsCriterion.GetValue },
            { "CO", HouseCriterion.GetValue },
            { "CP", ActionPointsCriterion.GetValue },
            { "CS", StrengthCriterion.GetValue },
            { "Cs", BaseStrengthCriterion.GetValue },
            { "CV", VitalityCriterion.GetValue },
            { "Cv", BaseVitalityCriterion.GetValue },
            { "CW", WisdomCriterion.GetValue },
            { "Cw", BaseWisdomCriterion.GetValue },
            { "Cz", AvailableSummonCriterion.GetValue },
            { "Fs", StateFightCriterion.GetValue },
            { "Fz", MonsterPresentInFightCriterion.GetValue },
            { "MK", MapPlayerCriterion.GetValue },
            { "Pa", AlignmentLevelCriterion.GetValue },
            { "PB", MapSubAreaCriterion.GetValue },
            { "PE", EmoteCriterion.GetValue },
            { "PG", BreedCriterion.GetValue },
            { "Pg", AlignmentFeatCriterion.GetValue },
            { "PJ", JobCriterion.GetValue },
            { "Pj", JobCriterion.GetValue },
            { "PK", KamasCriterion.GetValue },
            { "PL", LevelCriterion.GetValue },
            { "PM", LookCriterion.GetValue },
            { "Pm", MapCriterion.GetValue },
            { "PN", NameCriterion.GetValue },
            { "Pn", SlotCriterion.GetValue },
            { "PO", ItemCriterion.GetValue },
            { "PP", AlignmentRankCriterion.GetValue },
            { "PR", MariedCriterion.GetValue },
            { "Pr", AlignmentSpecializationCriterion.GetValue },
            { "PS", SexCriterion.GetValue },
            { "Ps", AlignmentCriterion.GetValue },
            { "PW", FreeWeightCriterion.GetValue },
            { "PX", PlayerRightsCriterion.GetValue },
            { "PZ", SubscribeCriterion.GetValue },
            { "Qa", QuestCriterion.GetValue },
            { "Qo", QuestObjectiveCriterion.GetValue },
            { "Qs", QuestStepCriterion.GetValue },
            { "Sc", ServerContentCriterion.GetValue },
            { "SI", ServerCriterion.GetValue }
        };

        public static string GetCriterionParse(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 4)
            {
                Console.WriteLine($"Criterion value incorrect : {value}");
                return value;
            }

            string criterion = value[0..2];
            char @operator = TranslateOperator(value[2]);
            string args = value[3..];

            string? parsedCriterion = null;
            try
            {
                parsedCriterion = _factory[criterion](@operator, args.Split(','));
            }
            catch (KeyNotFoundException)
            {

            }

            return parsedCriterion ?? $"{criterion} {@operator} {args}";
        }

        public static List<string> GetCriteriaParse(string value, List<string>? result = null)
        {
            result ??= new();

            if (result.Count == 0)
                result.Add("");

            for (int i = 0; i < value.Length; i++)
                if (value[i].Equals('('))
                {
                    int parenthesisNumber = 0;
                    int indexOfEndParenthesis = -1;
                    for (int j = 0; j < value.Length; j++)
                        if (value[j].Equals('('))
                            parenthesisNumber++;
                        else if (value[j].Equals(')'))
                        {
                            parenthesisNumber--;
                            if (parenthesisNumber == 0)
                            {
                                indexOfEndParenthesis = j;
                                break;
                            }
                        }

                    string subValue = value[1..indexOfEndParenthesis];

                    result[^1] += "(";
                    result = GetCriteriaParse(subValue, result);
                    result[^1] += ")";

                    value = value[(indexOfEndParenthesis + 1)..];
                    i = -1;
                }
                else if (_logicalOperators.Contains(value[i]))
                {
                    string subValue = value[0..i];

                    result = GetCriteriaParse(subValue, result);
                    result.Add($"{TranslateLogicalOperator(value[i])} ");

                    value = value[(i + 1)..];
                    i = -1;
                }
                else if (i == value.Length - 1)
                    result[^1] += GetCriterionParse(value);

            if (string.IsNullOrEmpty(result[0]))
                return new();

            return result;
        }

        private static string TranslateLogicalOperator(char logicalOperator)
        {
            switch (logicalOperator)
            {
                case '&':
                    return "et";
                case '|':
                    return "ou";
                default:
                    return logicalOperator.ToString();
            }
        }

        private static char TranslateOperator(char @operator)
        {
            switch (@operator)
            {
                case '!':
                    return '≠';
                case '~':
                    return '=';
                default:
                    return @operator;
            }
        }
    }
}
