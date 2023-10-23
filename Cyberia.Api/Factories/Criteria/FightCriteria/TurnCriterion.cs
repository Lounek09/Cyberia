namespace Cyberia.Api.Factories.Criteria.FightCriteria
{
    public sealed record TurnCriterion : Criterion, ICriterion<TurnCriterion>
    {
        public string Turn { get; init; }

        private TurnCriterion(string id, char @operator, string turn) :
            base(id, @operator)
        {
            Turn = turn;
        }

        public static TurnCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0)
            {
                return new(id, @operator, parameters[0]);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            if (Operator is '%')
            {
                if (Turn.Equals("2:0"))
                {
                    return $"Criterion.Turn.Even";
                }
                if (Turn.Equals("2:1"))
                {
                    return $"Criterion.Turn.Odd";
                }
            }

            return $"Criterion.Turn.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Turn);
        }
    }
}
