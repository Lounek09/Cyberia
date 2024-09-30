using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record OpenGuildPropertiesUIEffect : Effect
{
    public GuildProperty GuildProperty { get; init; }

    private OpenGuildPropertiesUIEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, GuildProperty guildProperty)
        : base(id, duration, probability, criteria, effectArea)
    {
        GuildProperty = guildProperty;
    }

    internal static OpenGuildPropertiesUIEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (GuildProperty)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GuildProperty.GetDescription());
    }
}
