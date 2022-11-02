namespace Salamandra.Api.Datacenter
{
    [Table("QuestsStepsJobsReward")]
    public sealed class QuestStepJobReward
    {
        [PrimaryKey, NotNull]
        public int QuestStepId { get; set; }

        [PrimaryKey, NotNull]
        public int JobId { get; set; }

        public QuestStepJobReward()
        {

        }
    }
}
