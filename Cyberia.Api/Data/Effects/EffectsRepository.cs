using Cyberia.Api.Data.Effects.Custom;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "effects.json";

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

    public EffectData? GetEffectDataById(int id)
    {
        Effects.TryGetValue(id, out var effectData);
        return effectData;
    }

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<EffectsCustomRepository>();

        foreach (var effectCustomData in customRepository.Effects)
        {
            var effectData = EffectsCore.Find(x => x.Id == effectCustomData.Id);
            if (effectData is null)
            {
                EffectsCore.Add(new EffectData()
                {
                    Id = effectCustomData.Id,
                    Description = new LocalizedString(effectCustomData.Description),
                    CharacteristicId = effectCustomData.CharacteristicId,
                    Operator = effectCustomData.Operator,
                    VisibleInTooltips = effectCustomData.ShowInTooltip,
                    DisplayableInDiceMode = effectCustomData.ShowInDiceModePossible,
                    Element = effectCustomData.Element
                });

                continue;
            }

            effectData.Description = new LocalizedString(effectCustomData.Description);
        }

        Effects = EffectsCore.GroupBy(x => x.Id).ToFrozenDictionary(x => x.Key, x => x.First());
    }
}
