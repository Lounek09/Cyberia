namespace Cyberia.Api.Factories.Criteria;

public sealed record ServerContentCriterion : Criterion, ICriterion
{
    public int Number { get; init; }

    private ServerContentCriterion(string id, char @operator, int number)
        : base(id, @operator)
    {
        Number = number;
    }

    internal static ServerContentCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var number))
        {
            return new(id, @operator, number);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.ServerContent.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Number);
    }
}
