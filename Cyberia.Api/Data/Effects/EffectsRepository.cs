using Cyberia.Api.Data.Effects.Custom;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsRepository : IDofusRepository
{
    private const string c_fileName = "effects.json";

    [JsonPropertyName("E")]
    [JsonInclude]
    internal List<EffectData> EffectsCore { get; init; }

    [JsonIgnore]
    public FrozenDictionary<int, EffectData> Effects { get; internal set; }

    [JsonConstructor]
    internal EffectsRepository()
    {
        EffectsCore = [];
        Effects = FrozenDictionary<int, EffectData>.Empty;
    }

    internal static EffectsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<EffectsRepository>(filePath);
        var customData = Datacenter.LoadRepository<EffectsCustomRepository>(customFilePath);

        foreach (var effectCustomData in customData.Effects)
        {
            var effectData = data.EffectsCore.Find(x => x.Id == effectCustomData.Id);
            if (effectData is null)
            {
                data.EffectsCore.Add(new EffectData()
                {
                    Id = effectCustomData.Id,
                    Description = effectCustomData.Description,
                    CharacteristicId = effectCustomData.CharacteristicId,
                    Operator = effectCustomData.Operator,
                    VisibleInTooltips = effectCustomData.ShowInTooltip,
                    DisplayableInDiceMode = effectCustomData.ShowInDiceModePossible,
                    Element = effectCustomData.Element
                });

                continue;
            }

            effectData.Description = effectCustomData.Description;
        }

        data.Effects = data.EffectsCore.ToFrozenDictionary(x => x.Id, x => x);
        return data;
    }

    public EffectData? GetEffectDataById(int id)
    {
        Effects.TryGetValue(id, out var effectData);
        return effectData;
    }
}
