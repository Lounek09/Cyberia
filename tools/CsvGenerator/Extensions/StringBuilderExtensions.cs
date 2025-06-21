using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.Effects;

using System.Buffers;
using System.Text;

namespace CsvGenerator.Extensions;

public static class StringBuilderExtensions
{
    private static readonly SearchValues<char> s_csvChars = SearchValues.Create(",\n\"");

    public static StringBuilder AppendCsvString(this StringBuilder builder, string value)
    {
        if (value.AsSpan().ContainsAny(s_csvChars))
        {
            builder.Append('"');

            foreach (var character in value)
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

    public static StringBuilder AppendEffects(this StringBuilder builder, IReadOnlyList<IEffect> effects)
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

    public static StringBuilder AppendCriteria(this StringBuilder builder, CriteriaReadOnlyCollection criteria)
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

    public static StringBuilder AppendCraft(this StringBuilder builder, CraftData craftData)
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
