namespace Cyberia.Api.Factories.Criteria
{
    public sealed record CollectionCriteriaElement : ICriteriaElement
    {
        public IEnumerable<ICriteriaElement> Criteria { get; init; }

        internal CollectionCriteriaElement(IEnumerable<ICriteriaElement> criteria)
        {
            Criteria = criteria;
        }
    }
}
