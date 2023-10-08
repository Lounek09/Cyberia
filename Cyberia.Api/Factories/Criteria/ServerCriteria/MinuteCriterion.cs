namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public sealed record MinuteCriterion : Criterion, ICriterion<MinuteCriterion>
    {
        public int Minute { get; init; }

        private MinuteCriterion(string id, char @operator, int minute) :
            base(id, @operator)
        {
            Minute = minute;
        }

        public static MinuteCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int minute))
                return new(id, @operator, minute);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Minute.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Minute);
        }
    }
}
