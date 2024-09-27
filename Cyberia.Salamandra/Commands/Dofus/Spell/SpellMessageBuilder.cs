using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Values;
using Cyberia.Salamandra.Commands.Dofus.Breed;
using Cyberia.Salamandra.Commands.Dofus.Incarnation;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "S";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly SpellData _spellData;
    private readonly int _selectedLevel;
    private readonly SpellLevelData? _spellLevelData;
    private readonly BreedData? _breedData;
    private readonly IncarnationData? _incarnationData;

    public SpellMessageBuilder(EmbedBuilderService embedBuilderService, SpellData spell, int selectedLevel)
    {
        _embedBuilderService = embedBuilderService;
        _spellData = spell;
        _selectedLevel = selectedLevel;
        _spellLevelData = spell.GetSpellLevelData(selectedLevel);
        _breedData = spell.GetBreedData();
        _incarnationData = spell.GetIncarnationData();
    }

    public static SpellMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var spellId) &&
            int.TryParse(parameters[1], out var selectedLevel))
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new SpellMessageBuilder(embedBuilderService, spellData, selectedLevel);
            }
        }

        return null;
    }

    public static string GetPacket(int spellId, int selectedLevel)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, spellId, selectedLevel);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = Buttons1Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = Buttons2Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = OtherButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Spells, BotTranslations.Embed_Spell_Author)
            .WithTitle($"{_spellData.Name} ({_spellData.Id}) - {BotTranslations.ShortLevel} {_selectedLevel}")
            .WithDescription(string.IsNullOrEmpty(_spellData.Description) ? string.Empty : Formatter.Italic(_spellData.Description.ToString().Trim()))
            .WithThumbnail(await _spellData.GetIconImagePathAsync(CdnImageSize.Size128));

        if (_spellLevelData is not null)
        {
            embed.AddField(Constant.ZeroWidthSpace, $"{BotTranslations.Embed_Field_RequiredLevel_Title} {Formatter.Bold(_spellLevelData.NeededLevel.ToString())}", true);

            var range = $"{Formatter.Bold(_spellLevelData.MinRange.ToString())}{(_spellLevelData.MinRange == _spellLevelData.MaxRange ? string.Empty : $"{BotTranslations.to}{Formatter.Bold(_spellLevelData.MaxRange.ToString())}")} {BotTranslations.ShortRange}";
            var apCost = $"{Formatter.Bold(_spellLevelData.ActionPointCost.ToString())} {BotTranslations.ShortActionPoint}";
            embed.AddField(Constant.ZeroWidthSpace, $"{range}\n{apCost}", true);

            embed.AddField(BotTranslations.Embed_Field_Category_Title, _spellData.SpellCategory.GetDescription(), true);

            var requiredStatesData = _spellLevelData.GetRequiredStatesData().ToList();
            if (requiredStatesData.Count > 0)
            {
                embed.AddField(BotTranslations.Embed_Field_RequiredStates_Title, string.Join(", ", requiredStatesData.Select(x => x.Name)), true);
            }

            var forbiddenStatesData = _spellLevelData.GetForbiddenStatesData().ToList();
            if (forbiddenStatesData.Count > 0)
            {
                embed.AddField(BotTranslations.Embed_Field_ForbiddenStates_Title, string.Join(", ", forbiddenStatesData.Select(x => x.Name)), true);
            }

            if (_spellData.Id == 101 || _spellData.Id == 2083)
            {
                embed.AddField(BotTranslations.Embed_Field_Effects_Title, "Fuck");
            }
            else
            {
                var effects = _spellLevelData.Effects;
                if (effects.Count > 0)
                {
                    embed.AddEffectFields(BotTranslations.Embed_Field_Effects_Title, effects, false);
                }

                var trapEffects = _spellLevelData.GetTrapEffects();
                if (trapEffects.Count > 0)
                {
                    embed.AddEffectFields(BotTranslations.Embed_Field_TrapEffects_Title, trapEffects, false);
                }

                var glyphEffects = _spellLevelData.GetGlyphEffects();
                if (glyphEffects.Count > 0)
                {
                    embed.AddEffectFields(BotTranslations.Embed_Field_GlyphEffects_Title, glyphEffects, false);
                }

                var criticalEffects = _spellLevelData.CriticalEffects;
                if (criticalEffects.Count > 0)
                {
                    embed.AddEffectFields(BotTranslations.Embed_Field_CriticalEffects_Title, criticalEffects, false);
                }
            }

            var caracteristics = $"""
                 {BotTranslations.Embed_Field_Characteristics_Content_CriticalHit} {Formatter.Bold(_spellLevelData.CriticalHitRate == 0 ? "-" : $"1/{_spellLevelData.CriticalHitRate}")}
                 {BotTranslations.Embed_Field_Characteristics_Content_CriticalFailure} {Formatter.Bold(_spellLevelData.CriticalFailureRate == 0 ? "-" : $"1/{_spellLevelData.CriticalFailureRate}")}
                 {BotTranslations.Embed_Field_Characteristics_Content_CastPerTurn} {Formatter.Bold(_spellLevelData.CastPerTurn == 0 ? "-" : _spellLevelData.CastPerTurn.ToString())}
                 {BotTranslations.Embed_Field_Characteristics_Content_CastPerPlayer} {Formatter.Bold(_spellLevelData.CastPerPlayer == 0 ? "-" : _spellLevelData.CastPerPlayer.ToString())}
                 {BotTranslations.Embed_Field_Characteristics_Content_TurnsBetweenCast} {Formatter.Bold(_spellLevelData.TurnsBetweenCast == 0 ? "-" : _spellLevelData.TurnsBetweenCast == 63 ? BotTranslations.ShortInfinity : _spellLevelData.TurnsBetweenCast.ToString())}
                 {(_spellData.GlobalInterval ? BotTranslations.Embed_Field_Characteristics_Content_GlobalRecast : string.Empty)}
                 """;
            embed.AddField(BotTranslations.Embed_Field_Characteristics_Title, caracteristics, true);

            caracteristics = $"""
                {Emojis.Bool(_spellLevelData.AdjustableRange)} {BotTranslations.Embed_Field_Characteristics_Content_AdjustableRange}
                {Emojis.Bool(_spellLevelData.LineOfSight)} {BotTranslations.Embed_Field_Characteristics_Content_LineOfSight}
                {Emojis.Bool(_spellLevelData.Linear)} {BotTranslations.Embed_Field_Characteristics_Content_Linear}
                {Emojis.Bool(_spellLevelData.NeedFreeCell)} {BotTranslations.Embed_Field_Characteristics_Content_NeedFreeCell}
                {Emojis.Bool(_spellLevelData.CricalFailureEndTheTurn)} {BotTranslations.Embed_Field_Characteristics_Content_CriticalFailureEndTurn}
                """;
            embed.AddField(Constant.ZeroWidthSpace, caracteristics, true);
        }

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> Buttons1Builder()
    {
        for (var i = 0; i < Constant.MaxButtonPerRow; i++)
        {
            var y = i + 1;
            if (_spellData.GetSpellLevelData(y) is not null)
            {
                yield return new(DiscordButtonStyle.Primary, GetPacket(_spellData.Id, y), y.ToString(), _selectedLevel == y);
            }
        }
    }

    private IEnumerable<DiscordButtonComponent> Buttons2Builder()
    {
        if (_spellData.GetSpellLevelData(6) is not null)
        {
            yield return new(DiscordButtonStyle.Primary, GetPacket(_spellData.Id, 6), "6", _selectedLevel == 6);
        }
    }

    private IEnumerable<DiscordButtonComponent> OtherButtonsBuilder()
    {
        if (_breedData is not null)
        {
            yield return BreedComponentsBuilder.BreedButtonBuilder(_breedData);
        }

        if (_incarnationData is not null)
        {
            yield return IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData);
        }
    }
}
