namespace Salamandra.Api.Datacenter
{
    [Table("States")]
    public sealed class State
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int P { get; set; }

        public State()
        {
            Name = string.Empty;
        }
    }
}
