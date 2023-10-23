namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record HonorPointCriterion : Criterion, ICriterion<HonorPointCriterion>
    {
        public int HonorPoint { get; init; }

        private HonorPointCriterion(string id, char @operator, int honorPoint) :
            base(id, @operator)
        {
            HonorPoint = honorPoint;
        }

        public static HonorPointCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int honorPoint))
            {
                return new(id, @operator, honorPoint);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.HonorPoint.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(HonorPoint);
        }
    }
}
