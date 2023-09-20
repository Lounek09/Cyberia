using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LearnSpellLevelEffect : BasicEffect
    {
        public int SpellLevelId { get; init; }

        public LearnSpellLevelEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            SpellLevelId = parameters.Param3;
        }

        public static new LearnSpellLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public SpellLevel? GetSpellLevel()
        {
            return DofusApi.Instance.Datacenter.SpellsData.GetSpellLevelById(SpellLevelId);
        }

        public override string GetDescription()
        {
            SpellLevel? spellLevel = GetSpellLevel();
            if (spellLevel is null)
                return GetDescriptionFromParameters($"SpellLevel inconnu ({SpellLevelId})", "X");

            string spellName = DofusApi.Instance.Datacenter.SpellsData.GetSpellNameById(spellLevel.SpellId);

            return GetDescriptionFromParameters(spellName, spellLevel.Level.ToString());
        }
    }
}
