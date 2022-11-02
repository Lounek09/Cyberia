namespace Salamandra.Api.Datacenter
{
    [Table("SpeakingItemTriggers")]
    public sealed class SpeakingItemTrigger
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public SpeakingItemTrigger()
        {
            Name = string.Empty;
        }
    }
}
