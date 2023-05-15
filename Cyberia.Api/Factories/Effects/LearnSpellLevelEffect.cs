using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class LearnSpellLevelEffect : BasicEffect
    {
        public int SpellId { get; init; }

        public LearnSpellLevelEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            SpellId = parameters.Param3;
        }

        public static new LearnSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Spell? GetSpell()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellById(SpellId);
        }

        public override string GetDescription()
        {
            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescriptionFromParameters(null, null, spellName);
        }
    }
}
