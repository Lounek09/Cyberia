namespace Salamandra.Api.Datacenter
{
    [Table("QuestStepsItemsReward")]
    public sealed class QuestStepItemReward
    {
        [PrimaryKey, NotNull]
        public int QuestStepId { get; set; }

        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int Quantity { get; set; }

        public QuestStepItemReward()
        {

        }
    }
}
