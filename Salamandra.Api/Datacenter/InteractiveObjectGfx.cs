namespace Salamandra.Api.Datacenter
{
    [Table("InteractiveObjectsGfx")]
    public sealed class InteractiveObjectGfx
    {
        [PrimaryKey, NotNull]
        public int InteractiveObjectId { get; set; }

        [PrimaryKey, NotNull]
        public int GfxId { get; set; }

        public InteractiveObjectGfx()
        {

        }
    }
}
