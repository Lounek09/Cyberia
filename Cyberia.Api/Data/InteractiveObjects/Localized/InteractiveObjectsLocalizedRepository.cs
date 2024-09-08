using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects.Localized;

internal sealed class InteractiveObjectsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => InteractiveObjectsRepository.FileName;

    [JsonPropertyName("IO.d")]
    public IReadOnlyList<InteractiveObjectLocalizedData> InteractiveObjects { get; init; }

    [JsonConstructor]
    internal InteractiveObjectsLocalizedRepository()
    {
        InteractiveObjects = [];
    }
}
