using Cyberia.Api;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Values;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class SpellMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "S";
    public const int PACKET_VERSION = 1;

    private readonly SpellData _spellData;
    private readonly int _selectedLevel;
    private readonly SpellLevelData? _spellLevelData;
    private readonly BreedData? _breedData;
    private readonly IncarnationData? _incarnationData;

    public SpellMessageBuilder(SpellData spell, int selectedLevel)
    {
        _spellData = spell;
        _selectedLevel = selectedLevel;
        _spellLevelData = spell.GetSpellLevelData(selectedLevel);
        _breedData = spell.GetBreedData();
        _incarnationData = spell.GetIncarnationData();
    }

    public static SpellMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var spellId) &&
            int.TryParse(parameters[1], out var selectedLevel))
        {
            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                return new SpellMessageBuilder(spellData, selectedLevel);
            }
        }

        return null;
    }

    public static string GetPacket(int spellId, int selectedLevel)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, spellId, selectedLevel);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = Buttons1Builder();
        if (buttons.Count > 0)
        {
            message.AddComponents(buttons);
        }

        buttons = Buttons2Builder();
        if (buttons.Count > 0)
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Spells, "Livre de sorts")
            .WithTitle($"{_spellData.Name} ({_spellData.Id}) - Rang {_selectedLevel}")
            .WithDescription(string.IsNullOrEmpty(_spellData.Description) ? "" : Formatter.Italic(_spellData.Description.Trim()))
            .WithThumbnail(await _spellData.GetImagePath());

        if (_spellLevelData is not null)
        {
            embed.AddField(Constant.ZERO_WIDTH_SPACE, $"Niveau requis : {Formatter.Bold(_spellLevelData.NeededLevel.ToString())}", true);

            var range = $"{Formatter.Bold(_spellLevelData.MinRange.ToString())}{(_spellLevelData.MinRange == _spellLevelData.MaxRange ? "" : $" à {Formatter.Bold(_spellLevelData.MaxRange.ToString())}")} PO";
            var apCost = $"{Formatter.Bold(_spellLevelData.ActionPointCost.ToString())} PA";
            embed.AddField(Constant.ZERO_WIDTH_SPACE, $"{range}\n{apCost}", true);

            embed.AddField("Catégorie :", _spellData.SpellCategory.GetDescription(), true);

            var requiredStatesData = _spellLevelData.GetRequiredStatesData().ToList();
            if (requiredStatesData.Count > 0)
            {
                embed.AddField("Etats requis :", string.Join(", ", requiredStatesData.Select(x => x.Name)), true);
            }

            var forbiddenStatesData = _spellLevelData.GetForbiddenStatesData().ToList();
            if (forbiddenStatesData.Count > 0)
            {
                embed.AddField("Etats interdits :", string.Join(", ", forbiddenStatesData.Select(x => x.Name)), true);
            }

            if (_spellData.Id == 101 || _spellData.Id == 2083)
            {
                embed.AddField("Effets :", "Fuck roulette");
            }
            else
            {
                var effects = _spellLevelData.Effects;
                var trapEffects = _spellLevelData.GetTrapEffects();
                var glyphEffects = _spellLevelData.GetGlyphEffects();
                var criticalEffects = _spellLevelData.CriticalEffects;

                if (effects.Count == 0 && trapEffects.Count == 0 && glyphEffects.Count == 0 && criticalEffects.Count == 0)
                {
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE);
                }
                else
                {
                    if (effects.Count > 0)
                    {
                        embed.AddEffectFields("Effets :", effects);
                    }

                    if (trapEffects.Count > 0)
                    {
                        embed.AddEffectFields("Effets piege :", trapEffects);
                    }

                    if (glyphEffects.Count > 0)
                    {
                        embed.AddEffectFields("Effets glyphe :", glyphEffects);
                    }

                    if (criticalEffects.Count > 0)
                    {
                        embed.AddEffectFields("Effets critiques :", criticalEffects);
                    }
                }
            }

            var caracteristics = $"""
                 Probabilité de coup critique : {Formatter.Bold(_spellLevelData.CriticalHitRate == 0 ? "-" : $"1/{_spellLevelData.CriticalHitRate}")}
                 Probabilité d'échec : {Formatter.Bold(_spellLevelData.CriticalFailureRate == 0 ? "-" : $"1/{_spellLevelData.CriticalFailureRate}")}
                 Nb. de lancers par tour : {Formatter.Bold(_spellLevelData.LaunchCountByTurn == 0 ? "-" : _spellLevelData.LaunchCountByTurn.ToString())}
                 Nb. de lancers par tour par joueur : {Formatter.Bold(_spellLevelData.LaunchCountByPlayerByTurn == 0 ? "-" : _spellLevelData.LaunchCountByPlayerByTurn.ToString())}
                 Nb. de tours entre deux lancers : {Formatter.Bold(_spellLevelData.DelayBetweenLaunch == 0 ? "-" : _spellLevelData.DelayBetweenLaunch == 63 ? "inf." : _spellLevelData.DelayBetweenLaunch.ToString())}
                 {(_spellData.GlobalInterval ? "Intervalle de relance global" : "")}
                 """;
            embed.AddField("Autres caractéristiques : ", caracteristics, true);

            caracteristics = $"""
                {Emojis.Bool(_spellLevelData.CanBoostRange)} Portée modifiable
                {Emojis.Bool(_spellLevelData.LineOfSight)} Ligne de vue
                {Emojis.Bool(_spellLevelData.LineOnly)} Lancer en ligne
                {Emojis.Bool(_spellLevelData.NeedFreeCell)} Cellules libres
                {Emojis.Bool(_spellLevelData.CricalFailureEndTheTurn)} EC fini le tour
                """;
            embed.AddField(Constant.ZERO_WIDTH_SPACE, caracteristics, true);
        }

        return embed;
    }

    private List<DiscordButtonComponent> Buttons1Builder()
    {
        List<DiscordButtonComponent> components = [];

        for (var i = 1; i < 6; i++)
        {
            if (_spellData.GetSpellLevelData(i) is not null)
            {
                components.Add(new(ButtonStyle.Primary, GetPacket(_spellData.Id, i), i.ToString(), _selectedLevel == i));
            }
        }

        return components;
    }

    private List<DiscordButtonComponent> Buttons2Builder()
    {
        List<DiscordButtonComponent> components = [];

        if (_spellData.GetSpellLevelData(6) is not null)
        {
            components.Add(new(ButtonStyle.Primary, GetPacket(_spellData.Id, 6), "6", _selectedLevel == 6));
        }

        if (_breedData is not null)
        {
            components.Add(BreedComponentsBuilder.BreedButtonBuilder(_breedData));
        }

        if (_incarnationData is not null)
        {
            components.Add(IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData));
        }

        return components;
    }
}
