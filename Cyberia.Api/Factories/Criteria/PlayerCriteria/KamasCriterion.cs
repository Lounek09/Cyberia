namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record KamasCriterion : Criterion, ICriterion<KamasCriterion>
    {
        public int Kamas { get; init; }

        private KamasCriterion(string id, char @operator, int kamas) :
            base(id, @operator)
        {
            Kamas = kamas;
        }

        public static KamasCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int kamasId))
            {
                return new(id, @operator, kamasId);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Kamas.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(Kamas);
        }
    }
}
