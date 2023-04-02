namespace Cyberia.Api.Datacenter
{
    [Table("SpeakingItemMessagesFromTriggersAndMoods")]
    public sealed class SpeakingItemMessageFromTriggerAndMood : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int SpeakingItemMessageId { get; set; }

        [NotNull]
        public int SpeakingItemTriggerId { get; set; }

        [NotNull]
        public int SpeakingItemMoodId { get; set; }

        public SpeakingItemMessageFromTriggerAndMood()
        {
            Id = string.Empty;
        }
    }
}
