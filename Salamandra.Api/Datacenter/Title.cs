namespace Salamandra.Api.Datacenter
{
    [Table("Titles")]
    public sealed class Title
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Color { get; set; }

        [NotNull]
        public int ParametersTypeId { get; set; }

        public Title()
        {
            Name = string.Empty;
        }
    }
}
