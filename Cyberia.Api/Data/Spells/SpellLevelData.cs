using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Values;

using System.Collections.ObjectModel;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellLevelData : IDofusData<int>
{
    public int Id { get; init; }

    public ReadOnlyCollection<IEffect> Effects { get; init; }

    public ReadOnlyCollection<IEffect> CriticalEffects { get; init; }

    public int ActionPointCost { get; init; }

    public int MinRange { get; init; }

    public int MaxRange { get; init; }

    public int CriticalHitRate { get; init; }

    public int CriticalFailureRate { get; init; }

    public bool LineOnly { get; init; }

    public bool LineOfSight { get; init; }

    public bool NeedFreeCell { get; init; }

    public bool CanBoostRange { get; init; }

    public SpellLevelCategory SpellLevelCategory { get; init; }

    public int LaunchCountByTurn { get; init; }

    public int LaunchCountByPlayerByTurn { get; init; }

    public int DelayBetweenLaunch { get; init; }

    public ReadOnlyCollection<int> RequiredStatesId { get; init; }

    public ReadOnlyCollection<int> ForbiddenStatesId { get; init; }

    public int NeededLevel { get; init; }

    public bool CricalFailureEndTheTurn { get; init; }

    public SpellData SpellData { get; internal set; }

    public int Rank { get; internal set; }

    internal SpellLevelData()
    {
        Effects = ReadOnlyCollection<IEffect>.Empty;
        CriticalEffects = ReadOnlyCollection<IEffect>.Empty;
        RequiredStatesId = ReadOnlyCollection<int>.Empty;
        ForbiddenStatesId = ReadOnlyCollection<int>.Empty;
        SpellData = new();
    }

    public ReadOnlyCollection<IEffect> GetTrapEffects()
    {
        foreach (var effect in Effects)
        {
            if (effect is TrapEffect trapEffect)
            {
                var trapSpellData = trapEffect.GetSpellData();
                if (trapSpellData is not null)
                {
                    var trapSpellLevelData = trapSpellData.GetSpellLevelData(trapEffect.Level);
                    if (trapSpellLevelData is not null)
                    {
                        return trapSpellLevelData.Effects;
                    }
                }
            }
        }

        return ReadOnlyCollection<IEffect>.Empty;
    }

    public ReadOnlyCollection<IEffect> GetGlyphEffects()
    {
        foreach (var effect in Effects)
        {
            if (effect is GlyphEffect glyphEffect)
            {
                var glyphSpellData = glyphEffect.GetSpellData();
                if (glyphSpellData is not null)
                {
                    var glyphSpellLevelData = glyphSpellData.GetSpellLevelData(glyphEffect.Level);
                    if (glyphSpellLevelData is not null)
                    {
                        return glyphSpellLevelData.Effects;
                    }
                }
            }
        }

        return ReadOnlyCollection<IEffect>.Empty;
    }

    public IEnumerable<StateData> GetRequiredStatesData()
    {
        foreach (var stateId in RequiredStatesId)
        {
            var stateData = DofusApi.Datacenter.StatesData.GetStateDataById(stateId);
            if (stateData is not null)
            {
                yield return stateData;
            }
        }
    }

    public IEnumerable<StateData> GetForbiddenStatesData()
    {
        foreach (var stateId in ForbiddenStatesId)
        {
            var stateData = DofusApi.Datacenter.StatesData.GetStateDataById(stateId);
            if (stateData is not null)
            {
                yield return stateData;
            }
        }
    }
}
