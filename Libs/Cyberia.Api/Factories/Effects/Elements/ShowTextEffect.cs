using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ShowTextEffect : Effect
{
    public string Value { get; init; }

    private ShowTextEffect(int id, string value)
        : base(id)
    {
        Value = value;
    }

    internal static ShowTextEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, parameters.Param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, string.Empty, Value);
    }
}
