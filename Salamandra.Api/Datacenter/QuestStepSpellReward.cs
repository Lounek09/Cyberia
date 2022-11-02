namespace Salamandra.Api.Datacenter
{
    [Table("QuestStepsSpellsRewards")]
    public sealed class QuestStepSpellReward
    {
        [PrimaryKey, NotNull]
        public int QuestStepId { get; set; }

        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        public QuestStepSpellReward()
        {

        }
    }
}
