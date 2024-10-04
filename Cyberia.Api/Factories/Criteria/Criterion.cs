namespace Cyberia.Api.Factories.Criteria;

/// <inheritdoc cref="ICriterion"/>
public abstract record Criterion : ICriterion
{
    public string Id { get; init; }
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

    public abstract DescriptionString GetDescription();

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

    /// <inheritdoc cref="ICriterion.GetDescription"/>
    protected DescriptionString GetDescription<T>(T parameter)
    {
        return GetDescription(parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="ICriterion.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1>(T0 parameter0, T1 parameter1)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="ICriterion.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="ICriterion.GetDescription"/>
    protected DescriptionString GetDescription(params string[] parameters)
    {
        var descriptionKey = GetDescriptionKey();

        var descriptionValue = ApiTranslations.ResourceManager.GetString(descriptionKey);
        if (descriptionValue is null)
        {
            if (this is not UntranslatedCriterion)
            {
                Log.Warning("Unknown {@Criterion}", this);
            }

            return new(ApiTranslations.Criterion_Unknown,
                Id,
                $"{Id}{Operator}{string.Join(',', parameters)}");
        }

        return new(descriptionValue, parameters);
    }
}
