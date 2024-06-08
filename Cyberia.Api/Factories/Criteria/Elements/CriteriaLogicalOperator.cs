namespace Cyberia.Api.Factories.Criteria;

public sealed record CriteriaLogicalOperator : ICriteriaElement
{
    public char Operator { get; init; }

    internal CriteriaLogicalOperator(char @operator)
    {
        Operator = @operator;
    }

    public string GetDescription()
    {
        return Operator switch
        {
            '&' => ApiTranslations.Criterion_And,
            '|' => ApiTranslations.Criterion_Or,
            _ => Operator.ToString()
        };
    }
}
