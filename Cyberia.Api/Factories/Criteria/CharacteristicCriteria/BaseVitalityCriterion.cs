namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record BaseVitalityCriterion : Criterion, ICriterion<BaseVitalityCriterion>
    {
        public int Vitality { get; init; }

        private BaseVitalityCriterion(string id, char @operator, int vitality) :
            base(id, @operator)
        {
            Vitality = vitality;
        }

        public static BaseVitalityCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int vitality))
            {
                return new(id, @operator, vitality);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.BaseVitality.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Vitality);
        }
    }
}
