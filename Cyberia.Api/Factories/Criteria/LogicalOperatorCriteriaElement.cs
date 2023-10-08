namespace Cyberia.Api.Factories.Criteria
{
    public sealed record LogicalOperatorCriteriaElement : ICriteriaElement
    {
        public char Operator { get; init; }

        internal LogicalOperatorCriteriaElement(char @operator)
        {
            Operator = @operator;
        }

        public string GetDescription()
        {
            return Operator switch
            {
                '&' => Resources.Criterion_And,
                '|' => Resources.Criterion_Or,
                _ => Operator.ToString()
            };
        }
    }
}
