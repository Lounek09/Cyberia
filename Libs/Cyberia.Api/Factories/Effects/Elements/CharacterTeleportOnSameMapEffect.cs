using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterTeleportOnSameMapEffect : Effect
{
    public int Distance { get; init; }

    private CharacterTeleportOnSameMapEffect(int id, int distance)
        : base(id)
    {
        Distance = distance;
    }

    internal static CharacterTeleportOnSameMapEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Distance);
    }
}
