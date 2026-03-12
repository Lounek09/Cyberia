using Cyberia.Api.Data.Effects;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface EffectEffect
{
    int EffectId { get; }

    EffectData? GetEffectData();
}
