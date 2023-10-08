namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record VitalityCriterion : Criterion, ICriterion<VitalityCriterion>
    {
        public int Vitality { get; init; }

        private VitalityCriterion(string id, char @operator, int vitality) :
            base(id, @operator)
        {
            Vitality = vitality;
        }

        public static VitalityCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int vitality))
                return new(id, @operator, vitality);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Vitality.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Vitality);
        }
    }
}
