namespace Cyberia.Api.Datacenter
{
    [Table("SpellLevelsRequiredStates")]
    public sealed class SpellLevelRequiredState : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int SpellId { get; set; }

        [NotNull]
        public int Level { get; set; }

        [NotNull]
        public int StateId { get; set; }

        public SpellLevelRequiredState()
        {
            Id = string.Empty;
        }
    }
}
