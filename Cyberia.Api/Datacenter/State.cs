namespace Cyberia.Api.Datacenter
{
    [Table("States")]
    public sealed class State : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int P { get; set; }

        [NotNull]
        public bool Display { get; set; }

        [NotNull]
        public string ShortName { get; set; }

        public State()
        {
            Name = string.Empty;
            ShortName = string.Empty;
        }
    }
}
