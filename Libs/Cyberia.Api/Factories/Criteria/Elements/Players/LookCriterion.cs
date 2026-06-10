using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record LookCriterion : Criterion
{
    public string LookId { get; init; }

    private LookCriterion(string id, char @operator, string lookId)
        : base(id, @operator)
    {
        LookId = lookId;
    }

    internal static LookCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0]);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Look.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, LookId);
    }
}
