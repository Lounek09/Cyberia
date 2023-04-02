namespace Cyberia.Api.Datacenter
{
    [Table("QuestObjectives")]
    public sealed class QuestObjective : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int QuestObjectiveTypeId { get; set; }

        [NotNull]
        public string Parameters { get; set; }

        public int? XCoord { get; set; }

        public int? YCoord { get; set; }

        public int? QuestStepId { get; set; }

        public QuestObjective()
        {
            Parameters = string.Empty;
        }
    }
}
