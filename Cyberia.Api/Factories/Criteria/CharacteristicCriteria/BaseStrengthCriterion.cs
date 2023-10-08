namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record BaseStrengthCriterion : Criterion, ICriterion<BaseStrengthCriterion>
    {
        public int Strength { get; init; }

        private BaseStrengthCriterion(string id, char @operator, int strength) :
            base(id, @operator)
        {
            Strength = strength;
        }

        public static BaseStrengthCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int strength))
                return new(id, @operator, strength);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.BaseStrength.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Strength);
        }
    }
}
