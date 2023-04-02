namespace Cyberia.Api.Datacenter
{
    [Table("InteractiveObjectsGfx")]
    public sealed class InteractiveObjectGfx : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int InteractiveObjectId { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        public InteractiveObjectGfx()
        {
            Id = string.Empty;
        }
    }
}
