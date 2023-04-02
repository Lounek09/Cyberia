namespace Cyberia.Api.Datacenter
{
    [Table("Jobs")]
    public sealed class Job : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int JobSpecializationId { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        public Job()
        {
            Name = string.Empty;
        }
    }
}
