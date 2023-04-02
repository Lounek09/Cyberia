namespace Cyberia.Api.Datacenter
{
    [Table("QuestStepsJobsReward")]
    public sealed class QuestStepJobReward : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int QuestStepId { get; set; }

        [NotNull]
        public int JobId { get; set; }

        public QuestStepJobReward()
        {
            Id = string.Empty;
        }
    }
}
