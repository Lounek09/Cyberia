using Cyberia.Api.Data.Effects.Custom;
using Cyberia.Api.Data.Effects.Localized;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "effects.json";

    [JsonPropertyName("E")]
    [JsonInclude]
    private List<EffectData> EffectsCore { get; init; }

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

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<EffectsLocalizedRepository>(type, language);

        foreach (var effectLocalizedData in localizedRepository.Effects)
        {
            var effectData = GetEffectDataById(effectLocalizedData.Id);
            effectData?.Description.Add(twoLetterISOLanguageName, effectLocalizedData.Description);
        }
    }
}
