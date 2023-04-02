namespace Cyberia.Api.Datacenter
{
    [Table("QuestSteps")]
    public sealed class QuestStep : ITable
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

        public int? OptimalLevel { get; set; }

        public int? QuestId { get; set; }

        public QuestStep()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
