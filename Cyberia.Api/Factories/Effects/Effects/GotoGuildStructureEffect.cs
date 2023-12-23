using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record GotoGuildStructureEffect : Effect, IEffect<GotoGuildStructureEffect>
{
    public GuildStructure GuildStructure { get; init; }

    private GotoGuildStructureEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, GuildStructure guildStructure)
        : base(id, duration, probability, criteria, effectArea)
    {
        GuildStructure = guildStructure;
    }

    public static GotoGuildStructureEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (GuildStructure)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, GuildStructure.GetDescription());
    }
}
