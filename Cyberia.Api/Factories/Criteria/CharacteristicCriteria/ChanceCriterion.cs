namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record ChanceCriterion : Criterion, ICriterion<ChanceCriterion>
    {
        public int Chance { get; init; }

        private ChanceCriterion(string id, char @operator, int chance) :
            base(id, @operator)
        {
            Chance = chance;
        }

        public static ChanceCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int chance))
            {
                return new(id, @operator, chance);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Chance.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Chance);
        }
    }
}
