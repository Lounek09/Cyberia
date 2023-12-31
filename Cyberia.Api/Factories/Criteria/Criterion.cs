namespace Cyberia.Api.Factories.Criteria;

public abstract record Criterion(string Id, char Operator)
{
    protected string GetOperatorDescriptionName()
    {
        return Operator switch
        {
            '=' => "Equal",
            '!' => "Different",
            '>' => "Superior",
            '<' => "Inferior",
            '~' => "SoftEqual",
            _ => Operator.ToString(),
        };
    }

    protected abstract string GetDescriptionName();

    protected Description GetDescription(params object[] parameters)
    {
        var descriptionName = GetDescriptionName();

        var descriptionValue = Resources.ResourceManager.GetString(descriptionName);
        if (descriptionValue is null)
        {
            var commaSeparatedParameters = string.Join(',', parameters);

            Log.Warning("No translation for {CriterionDescriptionName} ({RawCriterion})",
                descriptionName,
                $"{Id}{Operator}{commaSeparatedParameters}");

            return new($"{Id} {Operator} #1", commaSeparatedParameters);
        }

        return new(descriptionValue, Array.ConvertAll(parameters, x => x.ToString() ?? string.Empty));
    }
}
