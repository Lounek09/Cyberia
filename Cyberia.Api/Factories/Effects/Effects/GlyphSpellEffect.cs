using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GlyphSpellEffect : Effect, IEffect<GlyphSpellEffect>
    {
        public int SpellId { get; init; }
        public int Level { get; init; }

        private GlyphSpellEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int spellId, int level) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            SpellId = spellId;
            Level = level;
        }

        public static GlyphSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
        }

        public SpellData? GetSpellData()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(SpellId);
        }

        public Description GetDescription()
        {
            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(SpellId);

            return GetDescription(spellName, Level);
        }
    }
}
