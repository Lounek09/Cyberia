namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record CurrentMovementPointCriterion : Criterion, ICriterion<CurrentMovementPointCriterion>
    {
        public int MovementPoint { get; init; }

        private CurrentMovementPointCriterion(string id, char @operator, int movementPoint) :
            base(id, @operator)
        {
            MovementPoint = movementPoint;
        }

        public static CurrentMovementPointCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int movementPoint))
                return new(id, @operator, movementPoint);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.CurrentMovementPoint.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(MovementPoint);
        }
    }
}
