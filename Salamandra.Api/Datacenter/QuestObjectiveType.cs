namespace Salamandra.Api.Datacenter
{
    [Table("QuestObjectiveTypes")]
    public sealed class QuestObjectiveType
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
