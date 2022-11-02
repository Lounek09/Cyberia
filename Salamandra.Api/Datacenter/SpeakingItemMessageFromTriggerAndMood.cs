namespace Salamandra.Api.Datacenter
{
    [Table("SpeakingItemMessagesFromTriggersAndMoods")]
    public sealed class SpeakingItemMessageFromTriggerAndMood
    {
        [PrimaryKey, NotNull]
        public int SpeakingItemMessageId { get; set; }

        [PrimaryKey, NotNull]
        public int SpeakingItemTriggerId { get; set; }

        [PrimaryKey, NotNull]
        public int SpeakingItemMoodId { get; set; }

        public SpeakingItemMessageFromTriggerAndMood()
        {

        }
    }
}
