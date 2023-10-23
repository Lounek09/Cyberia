using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record ForgetSpellEffect : Effect, IEffect<ForgetSpellEffect>
    {
        public int SpellId { get; init; }

        private ForgetSpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            SpellId = spellId;
        }

        public static ForgetSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public Description GetDescription()
        {
            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescription(null, null, spellName);
        }
    }
}
