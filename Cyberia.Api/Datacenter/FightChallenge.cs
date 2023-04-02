namespace Cyberia.Api.Datacenter
{
    [Table("FightChallenges")]
    public sealed class FightChallenge : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        public FightChallenge()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
