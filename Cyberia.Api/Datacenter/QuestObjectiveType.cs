namespace Cyberia.Api.Datacenter
{
    [Table("QuestObjectiveTypes")]
    public sealed class QuestObjectiveType : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Description { get; set; }

        public QuestObjectiveType()
        {
            Description = string.Empty;
        }
    }
}
