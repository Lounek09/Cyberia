using Cyberia.Api.Data.Emotes;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IEmoteEffect
{
    int EmoteId { get; }

    EmoteData? GetEmoteData();
}
