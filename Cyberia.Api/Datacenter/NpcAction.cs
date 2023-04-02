namespace Cyberia.Api.Datacenter
{
    [Table("NpcActions")]
    public sealed class NpcAction
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public NpcAction()
        {
            Name = string.Empty;
        }
    }
}
