using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record SetCrafterEffect : Effect
{
    public string Name { get; init; }

    private SetCrafterEffect(int id, string name)
        : base(id)
    {
        Name = name;
    }

    internal static SetCrafterEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, parameters.Param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, string.Empty, Name);
    }
}
