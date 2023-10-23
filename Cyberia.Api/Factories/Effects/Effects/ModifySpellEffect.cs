using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record ModifySpellEffect : Effect, IEffect<ModifySpellEffect>
    {
        public int SpellId { get; init; }
        public int Value { get; init; }

        private ModifySpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int value) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            SpellId = spellId;
            Value = value;
        }

        public static ModifySpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public Description GetDescription()
        {
            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescription(spellName, null, Value);
        }
    }
}
