using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Extensions;
using Cyberia.Salamandra.Commands.Dofus.Breed;
using Cyberia.Salamandra.Commands.Dofus.Incarnation;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

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
    private readonly BreedData? _gladiatroolBreedData;
    private readonly IncarnationData? _incarnationData;
    private readonly CultureInfo? _culture;

    public SpellMessageBuilder(EmbedBuilderService embedBuilderService, SpellData spell, int selectedLevel, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _spellData = spell;
        _selectedLevel = selectedLevel;
        _spellLevelData = spell.GetSpellLevelData(selectedLevel);
        _breedData = spell.GetBreedData();
        _gladiatroolBreedData = spell.GetGladiatroolBreedData();
        _incarnationData = spell.GetIncarnationData();
        _culture = culture;
    }

    public static SpellMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
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

                return new SpellMessageBuilder(embedBuilderService, spellData, selectedLevel, culture);
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
            message.AddActionRowComponent(buttons);
        }

        buttons = Buttons2Builder();
        if (buttons.Any())
        {
            message.AddActionRowComponent(buttons);
        }

        buttons = OtherButtonsBuilder();
        if (buttons.Any())
        {
            message.AddActionRowComponent(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Spells, Translation.Get<BotTranslations>("Embed.Spell.Author", _culture), _culture)
            .WithTitle($"{_spellData.Name.ToString(_culture)} ({_spellData.Id}) - {Translation.Get<BotTranslations>("ShortLevel", _culture)} {_selectedLevel}")
            .WithThumbnail(await _spellData.GetIconImagePathAsync(CdnImageSize.Size128));

        var description = _spellData.Description.ToString(_culture);
        if (!string.IsNullOrEmpty(description))
        {
            embed.Description += Formatter.Italic(description);
        }

        if (_spellLevelData is not null)
        {
            embed.AddField(
                Constant.ZeroWidthSpace,
                $"{Translation.Get<BotTranslations>("Embed.Field.RequiredLevel.Title", _culture)} {Formatter.Bold(_spellLevelData.NeededLevel.ToString())}",
                true);

            var range = $"{Formatter.Bold(_spellLevelData.MinRange.ToString())}{(_spellLevelData.MinRange == _spellLevelData.MaxRange ? string.Empty : $"{Translation.Get<BotTranslations>("to", _culture)}{Formatter.Bold(_spellLevelData.MaxRange.ToString())}")} {Translation.Get<BotTranslations>("ShortRange", _culture)}";
            var apCost = $"{Formatter.Bold(_spellLevelData.ActionPointCost.ToString())} {Translation.Get<BotTranslations>("ShortActionPoint", _culture)}";
            embed.AddField(Constant.ZeroWidthSpace, $"{range}\n{apCost}", true);

            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Category.Title", _culture),
                _spellData.SpellCategory.GetDescription(_culture),
                true);

            var requiredStatesData = _spellLevelData.GetRequiredStatesData();
            if (requiredStatesData.Any())
            {
                embed.AddField(
                    Translation.Get<BotTranslations>("Embed.Field.RequiredStates.Title", _culture),
                    string.Join(", ", requiredStatesData.Select(x => x.Name.ToString(_culture))),
                    true);
            }

            var forbiddenStatesData = _spellLevelData.GetForbiddenStatesData();
            if (forbiddenStatesData.Any())
            {
                embed.AddField(
                    Translation.Get<BotTranslations>("Embed.Field.ForbiddenStates.Title", _culture),
                    string.Join(", ", forbiddenStatesData.Select(x => x.Name.ToString(_culture))),
                    true);
            }

            // Fuck Roulette
            if (_spellData.Id == 101 || _spellData.Id == 2083)
            {
                embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), "Fuck");
            }
            else
            {
                embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), _spellLevelData.Effects, false, _culture);

                var trapEffects = _spellLevelData.GetTrapEffects();
                if (trapEffects.Count > 0)
                {
                    embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.TrapEffects.Title", _culture), trapEffects, false, _culture);
                }

                var glyphEffects = _spellLevelData.GetGlyphEffects();
                if (glyphEffects.Count > 0)
                {
                    embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.GlyphEffects.Title", _culture), glyphEffects, false, _culture);
                }

                var criticalEffects = _spellLevelData.CriticalEffects;
                if (criticalEffects.Count > 0)
                {
                    embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.CriticalEffects.Title", _culture), criticalEffects, false, _culture);
                }
            }

            var caracteristics = $"""
                 {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.CriticalHit", _culture)} {Formatter.Bold(_spellLevelData.CriticalHitRate == 0 ? " - " : $"1/{_spellLevelData.CriticalHitRate}")}
                 {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.CriticalFailure", _culture)} {Formatter.Bold(_spellLevelData.CriticalFailureRate == 0 ? " - " : $"1/{_spellLevelData.CriticalFailureRate}")}
                 {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.CastPerTurn", _culture)} {Formatter.Bold(_spellLevelData.CastPerTurn == 0 ? " - " : _spellLevelData.CastPerTurn.ToString())}
                 {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.CastPerPlayer", _culture)} {Formatter.Bold(_spellLevelData.CastPerPlayer == 0 ? " - " : _spellLevelData.CastPerPlayer.ToString())}
                 {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.TurnsBetweenCast", _culture)} {Formatter.Bold(_spellLevelData.TurnsBetweenCast == 0 ? " - " : _spellLevelData.TurnsBetweenCast == 63 ? Translation.Get<BotTranslations>("ShortInfinity", _culture) : _spellLevelData.TurnsBetweenCast.ToString())}
                 {(_spellData.GlobalInterval ? Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.GlobalRecast", _culture) : string.Empty)}
                 """;
            embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Characteristics.Title", _culture), caracteristics, true);

            caracteristics = $"""
                {Emojis.Bool(_spellLevelData.AdjustableRange, _culture)} {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.AdjustableRange", _culture)}
                {Emojis.Bool(_spellLevelData.LineOfSight, _culture)} {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.LineOfSight", _culture)}
                {Emojis.Bool(_spellLevelData.Linear, _culture)} {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.Linear", _culture)}
                {Emojis.Bool(_spellLevelData.NeedFreeCell, _culture)} {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.NeedFreeCell", _culture)}
                {Emojis.Bool(_spellLevelData.CricalFailureEndTheTurn, _culture)} {Translation.Get<BotTranslations>("Embed.Field.Characteristics.Content.CriticalFailureEndTurn", _culture)}
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
            yield return BreedComponentsBuilder.BreedButtonBuilder(_breedData, _culture);
        }

        if (_gladiatroolBreedData is not null)
        {
            yield return BreedComponentsBuilder.GladiatroolBreedButtonBuilder(_gladiatroolBreedData, true, _culture);
        }

        if (_incarnationData is not null)
        {
            yield return IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData, _culture);
        }
    }
}
