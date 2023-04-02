namespace Cyberia.Api.Datacenter
{
    [Table("QuestStepsSpellsReward")]
    public sealed class QuestStepSpellReward : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int QuestStepId { get; set; }

        [NotNull]
        public int SpellId { get; set; }

        public QuestStepSpellReward()
        {
            Id = string.Empty;
        }
    }
}
