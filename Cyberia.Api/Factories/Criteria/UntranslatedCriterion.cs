namespace Cyberia.Api.Factories.Criteria
{
    public sealed record UntranslatedCriterion : Criterion, ICriterion<UntranslatedCriterion>
    {
        string[] Parameters { get; init; }

        private UntranslatedCriterion(string id, char @operator, string[] parameters) :
            base(id, @operator)
        {
            Parameters = parameters;
        }

        public static UntranslatedCriterion Create(string id, char @operator, params string[] parameters)
        {
            return new(id, @operator, parameters);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Untranslated.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Parameters);
        }
    }
}
