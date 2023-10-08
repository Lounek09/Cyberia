namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public sealed record UntranslatedCriterion : Criterion, ICriterion<UntranslatedCriterion>
    {
        List<string> Parameters { get; init; }

        private UntranslatedCriterion(string id, char @operator, List<string> parameters) :
            base(id, @operator)
        {
            Parameters = parameters;
        }

        public static UntranslatedCriterion Create(string id, char @operator, params string[] parameters)
        {
            return new(id, @operator, parameters.ToList());
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Untranslated.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Parameters.ToArray());
        }
    }
}
