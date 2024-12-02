using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Commands.Dofus.ItemSet;
using Cyberia.Salamandra.Commands.Dofus.Spell;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "B";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly BreedData _breedData;
    private readonly IEnumerable<SpellData> _spellsData;
    private readonly SpellData? _specialSpellData;
    private readonly ItemSetData? _itemSetData;
    private readonly CultureInfo? _culture;

    public BreedMessageBuilder(EmbedBuilderService embedBuilderService, BreedData breedData, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _breedData = breedData;
        _spellsData = breedData.GetSpellsData();
        _specialSpellData = breedData.GetSpecialSpellData();
        _itemSetData = breedData.GetItemSetData();
        _culture = culture;
    }

    public static BreedMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var breedId))
        {
            var breedData = DofusApi.Datacenter.BreedsRepository.GetBreedDataById(breedId);
            if (breedData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, breedData, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int breedId)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, breedId);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        if (_spellsData.Any())
        {
            message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData, _culture));
        }

        var buttons = ButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Breeds, Translation.Get<BotTranslations>("Embed.Breed.Author", _culture))
            .WithTitle($"{_breedData.LongName.ToString(_culture)} ({_breedData.Id})")
            .WithDescription(Formatter.Italic(_breedData.Description.ToString(_culture)))
            .WithThumbnail(await _breedData.GetIconImagePathAsync(CdnImageSize.Size128))
            .WithImageUrl(await _breedData.GetWeaponsPreferenceImagePathAsync())
            .AddField(Translation.Get<BotTranslations>("Embed.Field.Characteristics.Title", _culture), CharacteristicsFieldContent());

        if (_spellsData.Any())
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Spells.Title", _culture),
                string.Join('\n', _spellsData.Select(x =>
                {
                    return $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.GetNeededLevel()} {Formatter.Bold(x.Name.ToString(_culture))}";
                }))
            );
        }

        return embed;
    }

    private string CharacteristicsFieldContent()
    {
        StringBuilder builder = new();

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Vitality", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.VitalityBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Wisdom", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.WisdomBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Strength", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.StrengthBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Intelligence", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.IntelligenceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Chance", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.ChanceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(Translation.Get<BotTranslations>("Agility", _culture)));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.AgilityBoostCost);

        return builder.ToString();
    }

    private void AppendStatBoostCost(StringBuilder builder, IReadOnlyList<IReadOnlyList<int>> boostCost)
    {
        for (var i = 0; i < boostCost.Count; i++)
        {
            if (boostCost[i].Count < 2)
            {
                continue;
            }

            builder.Append("  - ");
            builder.Append(boostCost[i].Count > 2 ? boostCost[i][2] : 1);
            builder.Append(Translation.Get<BotTranslations>("for", _culture));
            builder.Append(boostCost[i][1]);
            builder.Append(Translation.Get<BotTranslations>("from", _culture));
            builder.Append(boostCost[i][0]);
            builder.Append('\n');
        }
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        yield return BreedComponentsBuilder.GladiatroolBreedButtonBuilder(_breedData, false, _culture);

        if (_specialSpellData is not null)
        {
            yield return SpellComponentsBuilder.SpellButtonBuilder(_specialSpellData, _culture);
        }

        if (_itemSetData is not null)
        {
            yield return ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData, _culture);
        }
    }
}
