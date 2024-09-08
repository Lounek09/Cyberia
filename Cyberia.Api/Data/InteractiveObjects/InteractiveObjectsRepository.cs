using Cyberia.Api.Data.InteractiveObjects.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects;

public sealed class InteractiveObjectsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "interactiveobjects.json";

    [JsonPropertyName("IO.g")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, InteractiveObjectGfxData>))]
    [JsonInclude]
    internal FrozenDictionary<int, InteractiveObjectGfxData> InteractiveObjectsGfx { get; init; }

    [JsonPropertyName("IO.d")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, InteractiveObjectData>))]
    public FrozenDictionary<int, InteractiveObjectData> InteractiveObjects { get; init; }

    [JsonConstructor]
    internal InteractiveObjectsRepository()
    {
        InteractiveObjectsGfx = FrozenDictionary<int, InteractiveObjectGfxData>.Empty;
        InteractiveObjects = FrozenDictionary<int, InteractiveObjectData>.Empty;
    }

    internal InteractiveObjectGfxData? GetInteractiveObjectGfxDataById(int id)
    {
        InteractiveObjectsGfx.TryGetValue(id, out var interactiveObjectGfxData);
        return interactiveObjectGfxData;
    }

    public InteractiveObjectData? GetInteractiveObjectDataById(int id)
    {
        InteractiveObjects.TryGetValue(id, out var interactiveObjectData);
        return interactiveObjectData;
    }

    protected override void LoadCustomData()
    {
        foreach (var interactiveObjectData in InteractiveObjects.Values)
        {
            var interactiveObjectGfxData = GetInteractiveObjectGfxDataById(interactiveObjectData.GfxId);
            if (interactiveObjectGfxData is not null)
            {
                interactiveObjectData.GfxId = interactiveObjectGfxData.GfxId;
            }
        }
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<InteractiveObjectsLocalizedRepository>(type, language);

        foreach (var interactiveObjectLocalizedData in localizedRepository.InteractiveObjects)
        {
            var interactiveObjectData = GetInteractiveObjectDataById(interactiveObjectLocalizedData.Id);
            interactiveObjectData?.Name.Add(twoLetterISOLanguageName, interactiveObjectLocalizedData.Name);
        }
    }
}
