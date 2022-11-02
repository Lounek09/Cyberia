namespace Salamandra.Api.Datacenter
{
    [Table("QuestsStepsEmotesReward")]
    public sealed class QuestStepEmoteReward
    {
        [PrimaryKey, NotNull]
        public int QuestStepId { get; set; }

        [PrimaryKey, NotNull]
        public int EmoteId { get; set; }

        public QuestStepEmoteReward()
        {

        }
    }
}
