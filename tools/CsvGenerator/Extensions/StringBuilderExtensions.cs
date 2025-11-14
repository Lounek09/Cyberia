using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.Effects;

using System.Buffers;
using System.Text;

namespace CsvGenerator.Extensions;

public static class StringBuilderExtensions
{
    private static readonly SearchValues<char> s_csvEscapedChars = SearchValues.Create(",\n\"");

    extension(StringBuilder builder)
    {
        public StringBuilder AppendCsvString(string value)
        {
            var valueSpan = value.AsSpan();

            if (valueSpan.ContainsAny(s_csvEscapedChars))
            {
                builder.Append('"');

                foreach (var character in valueSpan)
                {
                    switch (character)
                    {
                        case '"':
                            builder.Append('"').Append('"');
                            break;
                        case '\n':
                            builder.Append(' ');
                            break;
                        default:
                            builder.Append(character);
                            break;
                    }
                }

                return builder.Append('"');
            }

            return builder.Append(value);
        }

        public StringBuilder AppendEffects(IReadOnlyList<IEffect> effects)
        {
            const string separator = " | ";
            const int separatorLength = 3;

            if (effects.Count == 0)
            {
                return builder;
            }

            StringBuilder effectsBuilder = new();

            foreach (var effect in effects)
            {
                effectsBuilder.Append(effect.GetDescription());
                effectsBuilder.Append(separator);
            }

            effectsBuilder.Remove(effectsBuilder.Length - separatorLength, separatorLength);

            return builder.AppendCsvString(effectsBuilder.ToString());
        }

        public StringBuilder AppendCriteria(CriteriaReadOnlyCollection criteria)
        {
            if (criteria.Count == 0)
            {
                return builder;
            }

            StringBuilder criteriaBuilder = new();

            foreach (var element in criteria)
            {
                switch (element)
                {
                    case CriteriaLogicalOperator criteriaOperator:
                        criteriaBuilder.Append(criteriaOperator.GetDescription());
                        break;
                    case CriteriaReadOnlyCollection subCriteria:
                        criteriaBuilder.Append('(');
                        criteriaBuilder.AppendCriteria(subCriteria);
                        criteriaBuilder.Append(')');
                        break;
                    case ICriterion criterion:
                        criteriaBuilder.Append(criterion.GetDescription());
                        break;
                }

                criteriaBuilder.Append(' ');
            }

            criteriaBuilder.Remove(criteriaBuilder.Length - 1, 1);

            return builder.AppendCsvString(criteriaBuilder.ToString());
        }

        public StringBuilder AppendCraft(CraftData craftData)
        {
            const string separator = " | ";
            const int separatorLength = 3;
            const string quantitySeparator = " x ";

            var ingredients = craftData.GetIngredients(1);
            if (ingredients.Count == 0)
            {
                return builder;
            }

            StringBuilder craftBuilder = new();

            foreach (var ingredient in ingredients)
            {
                craftBuilder.Append(ingredient.Value);
                craftBuilder.Append(quantitySeparator);
                craftBuilder.Append(ingredient.Key.Name);
                craftBuilder.Append(separator);
            }

            craftBuilder.Remove(craftBuilder.Length - separatorLength, separatorLength);

            return builder.AppendCsvString(craftBuilder.ToString());
        }
    }
}
