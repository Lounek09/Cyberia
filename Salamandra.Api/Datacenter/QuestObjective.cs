namespace Salamandra.Api.Datacenter
{
    [Table("QuestObjectives")]
    public sealed class QuestObjective
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int QuestObjectiveTypeId { get; set; }

        [NotNull]
        public string Parameters { get; set; }

        [NotNull]
        public int XCoord { get; set; }

        [NotNull]
        public int YCoord { get; set; }

        [NotNull]
        public int QuestStep { get; set; }

        public QuestObjective()
        {
            Parameters = string.Empty;
        }
    }
}
