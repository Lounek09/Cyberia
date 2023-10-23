using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.FightCriteria
{
    public sealed record MonsterSummonCriterion : Criterion, ICriterion<MonsterSummonCriterion>
    {
        public int MonsterId { get; init; }

        public MonsterSummonCriterion(string id, char @operator, int monsterId) :
            base(id, @operator)
        {
            MonsterId = monsterId;
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public static MonsterSummonCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int monsterId))
            {
                return new(id, @operator, monsterId);
            }

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.MonsterSummon.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(monsterName);
        }
    }
}
