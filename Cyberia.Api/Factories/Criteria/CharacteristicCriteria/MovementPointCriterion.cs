namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record MovementPointCriterion : Criterion, ICriterion<MovementPointCriterion>
    {
        public int MovementPoint { get; init; }

        private MovementPointCriterion(string id, char @operator, int movementPoint) :
            base(id, @operator)
        {
            MovementPoint = movementPoint;
        }

        public static MovementPointCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int movementPoint))
                return new(id, @operator, movementPoint);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.MovementPoint.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(MovementPoint);
        }
    }
}
