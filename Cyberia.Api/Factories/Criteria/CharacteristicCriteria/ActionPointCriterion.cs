namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record ActionPointCriterion : Criterion, ICriterion<ActionPointCriterion>
    {
        public int ActionPoint { get; init; }

        private ActionPointCriterion(string id, char @operator, int actionPoint) :
            base(id, @operator)
        {
            ActionPoint = actionPoint;
        }

        public static ActionPointCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int actionPoint))
            {
                return new(id, @operator, actionPoint);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.ActionPoint.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(ActionPoint);
        }
    }
}
