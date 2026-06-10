using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntTargetNameEffect : Effect
{
    public string Name { get; init; }

    private HuntTargetNameEffect(int id, string name)
        : base(id)
    {
        Name = name;
    }

    internal static HuntTargetNameEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, parameters.Param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, string.Empty, Name);
    }
}
