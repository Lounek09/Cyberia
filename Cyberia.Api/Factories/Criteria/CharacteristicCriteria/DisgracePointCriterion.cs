namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record DisgracePointCriterion : Criterion, ICriterion<DisgracePointCriterion>
    {
        public int DisgracePoint { get; init; }

        private DisgracePointCriterion(string id, char @operator, int disgracePoint) :
            base(id, @operator)
        {
            DisgracePoint = disgracePoint;
        }

        public static DisgracePointCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int disgracePoint))
            {
                return new(id, @operator, disgracePoint);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.DisgracePoint.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(DisgracePoint);
        }
    }
}
