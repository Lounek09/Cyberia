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

    protected Description GetDescription<T>(T parameter)
    {
        return GetDescription(parameter?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1>(T0 parameter0, T1 parameter1)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    protected Description GetDescription(params string[] parameters)
    {
        var descriptionName = GetDescriptionName();

        var descriptionValue = Resources.ResourceManager.GetString(descriptionName);
        if (descriptionValue is null)
        {
            Log.Warning("Unknown {@Criterion}", this);
            return new(Resources.Criterion_Unknown,
                Id,
                $"{Id}{Operator}{string.Join(',', parameters)}");
        }

        return new(descriptionValue, parameters);
    }
}
