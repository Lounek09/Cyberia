namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record LevelCriterion : Criterion, ICriterion<LevelCriterion>
    {
        public int Level { get; init; }

        private LevelCriterion(string id, char @operator, int level) :
            base(id, @operator)
        {
            Level = level;
        }

        public static LevelCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int level))
            {
                return new(id, @operator, level);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Level.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Level);
        }
    }
}
