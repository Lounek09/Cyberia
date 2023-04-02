namespace Cyberia.Api.Datacenter
{
    [Table("SpeakingItemMoods")]
    public sealed class SpeakingItemMood : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public SpeakingItemMood()
        {
            Name = string.Empty;
        }
    }
}
