namespace Salamandra.Api.Datacenter
{
    [Table("QuestSteps")]
    public sealed class QuestStep
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int ExperienceReward { get; set; }

        [NotNull]
        public int KamasReward { get; set; }

        [NotNull]
        public int OptimalLevel { get; set; }

        [NotNull]
        public int QuestId { get; set; }

        public QuestStep()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
