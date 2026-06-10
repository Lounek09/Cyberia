using Cyberia.Api.Data.Titles;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ITitileEffect
{
    int TitleId { get; }

    TitleData? GetTitleData();
}
