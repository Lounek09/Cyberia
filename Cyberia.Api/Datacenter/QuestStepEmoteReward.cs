namespace Cyberia.Api.Datacenter
{
    [Table("QuestStepsEmotesReward")]
    public sealed class QuestStepEmoteReward : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int QuestStepId { get; set; }

        [NotNull]
        public int EmoteId { get; set; }

        public QuestStepEmoteReward()
        {
            Id = string.Empty;
        }
    }
}
