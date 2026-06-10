using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Servers;

public sealed record MonthCriterion : Criterion
{
    public int MonthId { get; init; }

    private MonthCriterion(string id, char @operator, int monthId)
        : base(id, @operator)
    {
        MonthId = monthId;
    }

    internal static MonthCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var monthId))
        {
            return new(id, @operator, monthId);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Month.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monthName = DofusApi.Datacenter.TimeZonesRepository.GetMonthNameById(MonthId, culture);

        return GetDescription(culture, monthName);
    }
}
