using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Cyberia.Api.Factories.Criteria;

/// <summary>
/// Represents a criterion of an in game mechanic (to equip an item, if a spell effect is applicable, etc).
/// </summary>
public abstract record Criterion : ICriteriaElement
{
    /// <summary>
    /// Gets the unique identifier of the criterion.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the operator of the criterion.
    /// </summary>
    public char Operator { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Criterion"/> record.
    /// </summary>
    /// <param name="id">The unique identifier of the criterion.</param>
    /// <param name="operator">The operator of the criterion.</param>
    protected Criterion(string id, char @operator)
    {
        Id = id;
        Operator = @operator;
    }

    /// <summary>
    /// Generates a human-readable description of the criterion for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the criterion for the specified language.</returns>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates a human-readable description of the criterion for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the criterion for the specified culture.</returns>
    [OverloadResolutionPriority(2)]
    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="CriterionFactory.GetOperatorDescriptionKey"/>
    protected string GetOperatorDescriptionKey()
    {
        return CriterionFactory.GetOperatorDescriptionKey(Operator);
    }

    /// <summary>
    /// Gets the key of the description in the resource file.
    /// </summary>
    /// <returns>The key of the description in the resource file.</returns>
    protected abstract string GetDescriptionKey();

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T>(CultureInfo? culture, T parameter)
    {
        return GetDescription(culture, parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1>(CultureInfo? culture, T0 parameter0, T1 parameter1)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2, T3>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty,
            parameter3?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    [OverloadResolutionPriority(1)]
    protected DescriptionString GetDescription(CultureInfo? culture, params string[] parameters)
    {
        var descriptionKey = GetDescriptionKey();
        if (!Translation.TryGet<ApiTranslations>(descriptionKey, out var template, culture))
        {
            if (this is not UntranslatedCriterion)
            {
                Log.Warning("Unknown {@Criterion}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("Criterion.Unknown", culture),
                Id, $"{Id}{Operator}{string.Join(',', parameters)}");
        }

        return new DescriptionString(template, parameters);
    }
}
