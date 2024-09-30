using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;

using System.Buffers;
using System.Text;

namespace CsvGenerator.Extensions;

public static class StringBuilderExtensions
{
    private const string c_effectSeparator = " | ";
    private const string c_craftSeparator = " | ";
    private const string c_craftQuantitySeparator = " x ";

    private static readonly SearchValues<char> s_csvChars = SearchValues.Create(",\n\"");

    public static StringBuilder AppendCsvString(this StringBuilder builder, string value)
    {
        if (value.AsSpan().ContainsAny(s_csvChars))
        {
            StringBuilder valueBuilder = new(value.Length + 16);
            foreach (var character in value)
            {
                switch (character)
                {
                    case '"':
                        valueBuilder.Append('"').Append('"');
                        break;
                    case '\n':
                        valueBuilder.Append(' ');
                        break;
                    default:
                        valueBuilder.Append(character);
                        break;
                }
            }

            builder.Append('"');
            builder.Append(valueBuilder);
            builder.Append('"');
        }
        else
        {
            builder.Append(value);
        }

        return builder;
    }

    public static StringBuilder AppendEffects(this StringBuilder builder, IReadOnlyList<IEffect> effects)
    {
        if (effects.Count == 0)
        {
            return builder;
        }

        StringBuilder effectsBuilder = new();

        foreach (var effect in effects)
        {
            effectsBuilder.Append(effect.GetDescription());
            effectsBuilder.Append(c_effectSeparator);
        }

        effectsBuilder.Remove(effectsBuilder.Length - c_effectSeparator.Length, c_effectSeparator.Length);

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
        var ingredients = craftData.GetIngredients(1);
        if (ingredients.Count == 0)
        {
            return builder;
        }

        StringBuilder craftBuilder = new();

        foreach (var ingredient in ingredients)
        {
            craftBuilder.Append(ingredient.Value);
            craftBuilder.Append(c_craftQuantitySeparator);
            craftBuilder.Append(ingredient.Key.Name);
            craftBuilder.Append(c_craftSeparator);
        }

        craftBuilder.Remove(craftBuilder.Length - c_craftSeparator.Length, c_craftSeparator.Length);

        return builder.AppendCsvString(craftBuilder.ToString());
    }
}
