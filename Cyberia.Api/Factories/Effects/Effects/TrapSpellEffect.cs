using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TrapSpellEffect : Effect, IEffect<TrapSpellEffect>
    {
        public int SpellId { get; init; }
        public int Level { get; init; }

        private TrapSpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int level) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            SpellId = spellId;
            Level = level;
        }

        public static TrapSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public Description GetDescription()
        {
            string spellName = DofusApi.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescription(spellName, Level);
        }
    }
}
