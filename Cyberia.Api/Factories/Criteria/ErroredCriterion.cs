namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public sealed record ErroredCriterion : Criterion, ICriterion<ErroredCriterion>
    {
        List<string> Parameters { get; init; }

        private ErroredCriterion(string id, char @operator, List<string> parameters) :
            base(id, @operator)
        {
            Parameters = parameters;
        }

        public static ErroredCriterion Create(string id, char @operator, params string[] parameters)
        {
            return new(id, @operator, parameters.ToList());
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Errored.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Parameters.ToArray());
        }
    }
}
