using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record OpenGuildPropertiesUIEffect : Effect
{
    public GuildProperty GuildProperty { get; init; }

    private OpenGuildPropertiesUIEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, GuildProperty guildProperty)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        GuildProperty = guildProperty;
    }

    internal static OpenGuildPropertiesUIEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (GuildProperty)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GuildProperty.GetDescription(culture));
    }
}
