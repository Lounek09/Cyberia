namespace Salamandra.Api.Datacenter
{
    [Table("SpellLevelsRequiredStates")]
    public sealed class SpellLevelRequiredState
    {
        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        [PrimaryKey, NotNull]
        public int Level { get; set; }

        [PrimaryKey, NotNull]
        public int StateId { get; set; }

        public SpellLevelRequiredState()
        {

        }
    }
}
