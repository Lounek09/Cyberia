using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record GotoGuildPropertyEffect : Effect, IEffect
{
    public GuildProperty GuildProperty { get; init; }

    private GotoGuildPropertyEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, GuildProperty guildProperty)
        : base(id, duration, probability, criteria, effectArea)
    {
        GuildProperty = guildProperty;
    }

    internal static GotoGuildPropertyEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (GuildProperty)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GuildProperty.GetDescription());
    }
}
