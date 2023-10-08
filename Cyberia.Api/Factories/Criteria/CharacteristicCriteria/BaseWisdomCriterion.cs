namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public sealed record BaseWisdomCriterion : Criterion, ICriterion<BaseWisdomCriterion>
    {
        public int Wisdom { get; init; }

        private BaseWisdomCriterion(string id, char @operator, int wisdom) :
            base(id, @operator)
        {
            Wisdom = wisdom;
        }

        public static BaseWisdomCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int wisdom))
                return new(id, @operator, wisdom);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.BaseWisdom.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Wisdom);
        }
    }
}
