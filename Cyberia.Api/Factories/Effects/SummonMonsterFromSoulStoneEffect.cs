using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class SummonMonsterFromSoulStoneEffect : BasicEffect
    {
        public List<int> MonstersId { get; init; }

        public SummonMonsterFromSoulStoneEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            MonstersId = parameters.Param4.Split(":").Select(x => int.Parse(x, NumberStyles.HexNumber)).ToList();
        }

        public static new SummonMonsterFromSoulStoneEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public List<Monster> GetMonsters()
        {
            List<Monster> monsters = new();

            foreach (int monsterId in MonstersId)
            {
                Monster? monster = DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(monsterId);

                if (monster is not null)
                    monsters.Add(monster);
            }

            return monsters;
        }

        public override string GetDescription()
        {
            HashSet<string> monstersName = new();
            foreach (int monsterId in MonstersId)
                monstersName.Add(DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(monsterId));

            return GetDescriptionFromParameters(string.Join(", ", monstersName));
        }
    }
}
