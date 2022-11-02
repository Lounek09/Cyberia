namespace Salamandra.Api.Datacenter
{
    [Table("MonsterGrades")]
    public sealed class MonsterGrade
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [PrimaryKey, NotNull]
        public int Grade { get; set; }

        [NotNull]
        public int Level { get; set; }

        [NotNull]
        public int NeutralResistance { get; set; }

        [NotNull]
        public int EarthResistance { get; set; }

        [NotNull]
        public int FireResistance { get; set; }

        [NotNull]
        public int WaterResistance { get; set; }

        [NotNull]
        public int AirResistance { get; set; }

        [NotNull]
        public int ActionPointDodge { get; set; }

        [NotNull]
        public int MovementPointDodge { get; set; }

        [NotNull]
        public int? LifePoint { get; set; }

        [NotNull]
        public int? ActionPoint { get; set; }

        [NotNull]
        public int? MovementPoint { get; set; }

        public MonsterGrade()
        {

        }
    }
}
