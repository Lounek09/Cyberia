using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;

using System.Text;

namespace CsvGenerator.Extensions;

public static class StringBuilderExtensions
{
    private const string c_effectSeparator = " | ";

    public static StringBuilder AppendCsvString(this StringBuilder builder, string value)
    {
        if (value.Contains('"'))
        {
            value = value.Replace("\"", "\"\"");

            builder.Append('"');
            builder.Append(value);
            builder.Append('"');
        }
        else if (value.Contains(',') || value.Contains('\n'))
        {
            builder.Append('"');
            builder.Append(value);
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
}
