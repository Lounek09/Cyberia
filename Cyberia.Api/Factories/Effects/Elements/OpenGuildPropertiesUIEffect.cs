using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record OpenGuildPropertiesUIEffect : Effect
{
    public GuildProperty GuildProperty { get; init; }

    private OpenGuildPropertiesUIEffect(int id, GuildProperty guildProperty)
        : base(id)
    {
        GuildProperty = guildProperty;
    }

    internal static OpenGuildPropertiesUIEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (GuildProperty)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GuildProperty.GetDescription(culture));
    }
}
