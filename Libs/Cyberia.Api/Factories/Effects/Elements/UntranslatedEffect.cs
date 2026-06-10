using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record UntranslatedEffect : Effect
{
    public EffectParameters Parameters { get; init; }

    internal UntranslatedEffect(int id, EffectParameters parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Parameters.Param1, Parameters.Param2, Parameters.Param3, Parameters.Param4);
    }
}
