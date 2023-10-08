using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Criteria.CharacteristicCriteria;
using Cyberia.Api.Factories.Criteria.FightCriteria;
using Cyberia.Api.Factories.Criteria.MapCriteria;
using Cyberia.Api.Factories.Criteria.OtherCriteria;
using Cyberia.Api.Factories.Criteria.PlayerCriteria;
using Cyberia.Api.Factories.Criteria.QuestCriteria;
using Cyberia.Api.Factories.Criteria.ServerCriteria;

using System;
using System.Text;

namespace Cyberia.Api.Factories
{
    public static class CriterionFactory
    {
        private static readonly Dictionary<string, Func<string, char, string[], ICriterion?>> _factory = new()
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
            { "Qo", QuestObjectiveCriterion.Create },
            { "Qs", QuestStepCriterion.Create },
            { "Sc", ServerContentCriterion.Create },
            { "SI", ServerCriterion.Create },
            { "SM", MinuteCriterion.Create }
        };

        public static ICriterion? GetCriterion(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 4)
                return null;
                

            string id = value[0..2];
            char @operator = value[2];
            string[] args = value[3..].Split(',');

            if (_factory.TryGetValue(id, out Func<string, char, string[], ICriterion?>? builder))
            {
                ICriterion? criterion = builder(id, @operator, args);
                if (criterion is not null)
                    return criterion;

                return ErroredCriterion.Create(id, @operator, args);
            }

            return UntranslatedCriterion.Create(id, @operator, args);
        }

        public static IEnumerable<ICriteriaElement> GetCriteria(string value)
        {
            int index = 0;
            while (index < value.Length)
            {
                switch (value[index])
                {
                    case '(':
                        (string token, index) = ExtractToken(value, index + 1, ')');

                        yield return new CollectionCriteriaElement(GetCriteria(token));
                        break;
                    case '&' or '|':
                        yield return new LogicalOperatorCriteriaElement(value[index]);
                        break;
                    default:
                        (token, index) = ExtractToken(value, index, '&', '|', ')');
                        index--;

                        ICriteriaElement? criterion = GetCriterion(token.ToString());
                        if (criterion is null)
                            yield break;

                        yield return criterion;
                        break;
                }

                index++;
            }
        }

        private static (string token, int newIndex) ExtractToken(string value, int startIndex, params char[] endChars)
        {
            int endIndex = startIndex;
            int openParenthesesCount = 0;

            while (endIndex < value.Length)
            {
                switch (value[endIndex])
                {
                    case '(':
                        openParenthesesCount++;
                        break;
                    case ')':
                        if (openParenthesesCount == 0)
                            return (value[startIndex..endIndex], endIndex);

                        openParenthesesCount--;
                        break;
                    default:
                        if (openParenthesesCount == 0 && endChars.Contains(value[endIndex]))
                            return (value[startIndex..endIndex], endIndex);

                        break;
                }

                endIndex++;
            }

            return (value[startIndex..endIndex], endIndex);
        }
    }
}
