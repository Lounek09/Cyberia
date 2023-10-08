namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record SubscribeCriterion : Criterion, ICriterion<SubscribeCriterion>
    {
        public bool Subscribed { get; init; }

        private SubscribeCriterion(string id, char @operator, bool subscribed) :
            base(id, @operator)
        {
            Subscribed = subscribed;
        }

        public static SubscribeCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0)
                return new(id, @operator, parameters[0].Equals("1"));

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Subscribe.{GetOperatorDescriptionName()}.{Subscribed}";
        }

        public Description GetDescription()
        {
            return GetDescription(Subscribed);
        }
    }
}
