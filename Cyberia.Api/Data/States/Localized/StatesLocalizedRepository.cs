using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States.Localized;

internal sealed class StatesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => StatesRepository.FileName;

    [JsonPropertyName("ST")]
    public IReadOnlyList<StateData> States { get; init; }

    [JsonConstructor]
    internal StatesLocalizedRepository()
    {
        States = [];
    }
}
