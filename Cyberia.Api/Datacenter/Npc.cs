namespace Cyberia.Api.Datacenter
{
    [Table("Npcs")]
    public sealed class Npc
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public Npc()
        {
            Name = string.Empty;
        }
    }
}
