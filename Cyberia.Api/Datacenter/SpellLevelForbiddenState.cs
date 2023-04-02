namespace Cyberia.Api.Datacenter
{
    [Table("SpellLevelsForbiddenStates")]
    public sealed class SpellLevelForbiddenState : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int SpellId { get; set; }

        [NotNull]
        public int Level { get; set; }

        [NotNull]
        public int StateId { get; set; }

        public SpellLevelForbiddenState()
        {
            Id = string.Empty;
        }
    }
}
