namespace Cyberia.Api.Datacenter
{
    [Table("Maps")]
    public sealed class Map : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int XCoord { get; set; }

        [NotNull]
        public int YCoord { get; set; }

        [NotNull]
        public int MapSubAreaId { get; set; }

        [NotNull]
        public string FightPosition1 { get; set; }

        [NotNull]
        public string FightPosition2 { get; set; }

        [NotNull]
        public string Parameters { get; set; }

        [NotNull]
        public int MaxPlayerPerFight { get; set; }

        [NotNull]
        public int MaxPlayerPerTeam { get; set; }

        [NotNull]
        public int Episode { get; set; }

        public Map()
        {
            FightPosition1 = string.Empty;
            FightPosition2 = string.Empty;
            Parameters = string.Empty;
            MaxPlayerPerFight = 16;
            MaxPlayerPerTeam = 8;
        }
    }
}
