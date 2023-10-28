using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record ItemCriterion : Criterion, ICriterion<ItemCriterion>
    {
        public int ItemId { get; init; }

        private ItemCriterion(string id, char @operator, int emoteId) :
            base(id, @operator)
        {
            ItemId = emoteId;
        }

        public static ItemCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int itemId))
            {
                return new(id, @operator, itemId);
            }

            return null;
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Item.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(itemName);
        }
    }
}
