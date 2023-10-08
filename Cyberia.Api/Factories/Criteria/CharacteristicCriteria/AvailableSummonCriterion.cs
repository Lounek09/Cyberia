namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record AvailableSummonCriterion : Criterion, ICriterion<AvailableSummonCriterion>
    {
        public int AvailableSummon { get; init; }

        private AvailableSummonCriterion(string id, char @operator, int availableSummon) :
            base(id, @operator)
        {
            AvailableSummon = availableSummon;
        }

        public static AvailableSummonCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int availableSummon))
                return new(id, @operator, availableSummon);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.AvailableSummon.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(AvailableSummon);
        }
    }
}
