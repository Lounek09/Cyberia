namespace Cyberia.Api.Datacenter
{
    [Table("SpeakingItemMessages")]
    public sealed class SpeakingItemMessage : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Message { get; set; }

        [NotNull]
        public int SoundId { get; set; }

        [NotNull]
        public string ItemsIdCanUse { get; set; }

        [NotNull]
        public int RequiredLevel { get; set; }

        [NotNull]
        public decimal Probability { get; set; }

        public SpeakingItemMessage()
        {
            Message = string.Empty;
            ItemsIdCanUse = string.Empty;
        }
    }
}
