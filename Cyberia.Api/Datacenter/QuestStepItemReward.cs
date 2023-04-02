namespace Cyberia.Api.Datacenter
{
    [Table("QuestStepsItemsReward")]
    public sealed class QuestStepItemReward : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int QuestStepId { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int Quantity { get; set; }

        public QuestStepItemReward()
        {
            Id = string.Empty;
        }
    }
}
