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

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "G";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly BreedData _breedData;
    private readonly IEnumerable<SpellData> _spellsData;
    private readonly SpellData? _specialSpellData;
    private readonly ItemSetData? _itemSetData;

    public BreedMessageBuilder(EmbedBuilderService embedBuilderService, BreedData breedData)
    {
        _embedBuilderService = embedBuilderService;
        _breedData = breedData;
        _spellsData = breedData.GetSpellsData();
        _specialSpellData = breedData.GetSpecialSpellData();
        _itemSetData = breedData.GetItemSetData();
    }

    public static BreedMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var breedId))
        {
            var breedData = DofusApi.Datacenter.BreedsRepository.GetBreedDataById(breedId);
            if (breedData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, breedData);
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
            message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData));
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
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Breeds, BotTranslations.Embed_Breed_Author)
            .WithTitle($"{_breedData.LongName} ({_breedData.Id})")
            .WithDescription(Formatter.Italic(_breedData.Description))
            .WithThumbnail(await _breedData.GetIconImagePathAsync(CdnImageSize.Size128))
            .WithImageUrl(await _breedData.GetWeaponsPreferenceImagePathAsync())
            .AddField(BotTranslations.Embed_Field_Characteristics_Title, CharacteristicsFieldContent());

        if (_spellsData.Any())
        {
            embed.AddField(BotTranslations.Embed_Field_Spells_Title,
                string.Join('\n', _spellsData.Select(x => $"- {BotTranslations.ShortLevel}{x.GetNeededLevel()} {Formatter.Bold(x.Name)}")));
        }

        return embed;
    }

    private string CharacteristicsFieldContent()
    {
        StringBuilder builder = new();

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Vitality));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.VitalityBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Wisdom));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.WisdomBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Strength));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.StrengthBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Intelligence));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.IntelligenceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Chance));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.ChanceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold(BotTranslations.Agility));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.AgilityBoostCost);

        return builder.ToString();
    }

    private static void AppendStatBoostCost(StringBuilder builder, IReadOnlyList<IReadOnlyList<int>> boostCost)
    {
        for (var i = 0; i < boostCost.Count; i++)
        {
            if (boostCost[i].Count < 2)
            {
                continue;
            }

            builder.Append(" - ");
            builder.Append(boostCost[i].Count > 2 ? boostCost[i][2] : 1);
            builder.Append(BotTranslations._for);
            builder.Append(boostCost[i][1]);
            builder.Append(BotTranslations.from);
            builder.Append(boostCost[i][0]);
            builder.Append('\n');
        }
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        if (_specialSpellData is not null)
        {
            yield return SpellComponentsBuilder.SpellButtonBuilder(_specialSpellData);
        }

        if (_itemSetData is not null)
        {
            yield return ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData);
        }
    }
}
