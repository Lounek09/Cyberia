using Cyberia.Api.Data.TTG;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface ITTGFamilyEffect
{
    int TTGFamilyId { get; }

    TTGFamilyData? GetTTGFamilyData();
}
