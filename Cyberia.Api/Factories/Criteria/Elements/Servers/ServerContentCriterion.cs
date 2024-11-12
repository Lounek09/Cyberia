using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Servers;

public sealed record ServerContentCriterion : Criterion
{
    public int Number { get; init; }

    private ServerContentCriterion(string id, char @operator, int number)
        : base(id, @operator)
    {
        Number = number;
    }

    internal static ServerContentCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var number))
        {
            return new(id, @operator, number);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.ServerContent.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Number);
    }
}
