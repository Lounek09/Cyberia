using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record GotoGuildStructureEffect : Effect, IEffect
{
    public GuildStructure GuildStructure { get; init; }

    private GotoGuildStructureEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, GuildStructure guildStructure)
        : base(id, duration, probability, criteria, effectArea)
    {
        GuildStructure = guildStructure;
    }

    internal static GotoGuildStructureEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (GuildStructure)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GuildStructure.GetDescription());
    }
}
