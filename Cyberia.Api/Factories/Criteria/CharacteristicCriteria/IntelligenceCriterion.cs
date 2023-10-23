namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record IntelligenceCriterion : Criterion, ICriterion<IntelligenceCriterion>
    {
        public int Intelligence { get; init; }

        private IntelligenceCriterion(string id, char @operator, int intelligence) :
            base(id, @operator)
        {
            Intelligence = intelligence;
        }

        public static IntelligenceCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int intelligence))
            {
                return new(id, @operator, intelligence);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Intelligence.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Intelligence);
        }
    }
}
