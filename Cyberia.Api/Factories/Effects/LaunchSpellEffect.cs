using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class LaunchSpellEffect : BasicEffect
    {
        public int SpellId { get; init; }
        public int Level { get; init; }

        public LaunchSpellEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            SpellId = parameters.Param1;
            Level = parameters.Param2;
        }

        public static new LaunchSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Spell? GetSpell()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellById(SpellId);
        }

        public override string GetDescription()
        {
            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescriptionFromParameters(spellName, Level.ToString());
        }
    }
}
