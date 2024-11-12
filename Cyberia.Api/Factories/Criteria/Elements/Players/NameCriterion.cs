using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record NameCriterion : Criterion
{
    public string Name { get; init; }

    private NameCriterion(string id, char @operator, string name)
        : base(id, @operator)
    {
        Name = name;
    }

    internal static NameCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Name.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Name);
    }
}
