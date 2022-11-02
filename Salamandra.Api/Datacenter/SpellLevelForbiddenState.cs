namespace Salamandra.Api.Datacenter
{
    [Table("SpellLevelsForbiddenStates")]
    public sealed class SpellLevelForbiddenState
    {
        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        [PrimaryKey, NotNull]
        public int Level { get; set; }

        [PrimaryKey, NotNull]
        public int StateId { get; set; }

        public SpellLevelForbiddenState()
        {

        }
    }
}
