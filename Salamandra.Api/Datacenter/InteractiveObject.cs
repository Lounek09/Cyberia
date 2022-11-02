namespace Salamandra.Api.Datacenter
{
    [Table("InteractiveObjects")]
    public sealed class InteractiveObject
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Type { get; set; }

        public InteractiveObject()
        {
            Name = string.Empty;
        }
    }
}
