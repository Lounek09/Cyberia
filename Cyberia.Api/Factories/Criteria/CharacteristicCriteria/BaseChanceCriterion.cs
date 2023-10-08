namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record BaseChanceCriterion : Criterion, ICriterion<BaseChanceCriterion>
    {
        public int Chance { get; init; }

        private BaseChanceCriterion(string id, char @operator, int chance) :
            base(id, @operator)
        {
            Chance = chance;
        }

        public static BaseChanceCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int chance))
                return new(id, @operator, chance);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.BaseChance.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Chance);
        }
    }
}
