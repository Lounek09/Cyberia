using Cyberia.Api.Data;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemStats;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Commands.Dofus.Spell;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class GladiatroolBreedMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "BG";
    public const int PacketVersion = 1;

    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly BreedData _breedData;
    private readonly ItemData? _weaponItemData;
    private readonly ItemTypeData? _weaponItemTypeData;
    private readonly ItemStatsData? _weaponItemStatsData;
    private readonly IEnumerable<SpellData> _spellsData;
    private readonly CultureInfo? _culture;

    public GladiatroolBreedMessageBuilder(IEmbedBuilderService embedBuilderService, BreedData breedData, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _breedData = breedData;
        _weaponItemData = breedData.GetGladiatroolWeaponItemData();
        _weaponItemTypeData = _weaponItemData?.GetItemTypeData();
        _weaponItemStatsData = _weaponItemData?.GetItemStatsData();
        _spellsData = breedData.GetGladiatroolSpellsData();
        _culture = culture;
    }

    public static GladiatroolBreedMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var breedId))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var breedData = dofusDatacenter.BreedsRepository.GetBreedDataById(breedId);
            if (breedData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

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
            message.AddActionRowComponent(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData, _culture));
        }

        var buttons = ButtonsBuilder();
        if (buttons.Any())
        {
            message.AddActionRowComponent(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Breeds, Translation.Get<BotTranslations>("Embed.Breed.Author", _culture), _culture)
            .WithTitle($"{_breedData.LongName.ToString(_culture)} - {Translation.Get<BotTranslations>("Gladiatrool", _culture)} ({_breedData.Id})");

        if (_weaponItemData is not null)
        {
            embed.WithThumbnail(await _weaponItemData.GetImagePathAsync(CdnImageSize.Size128));

            if (_weaponItemStatsData is not null)
            {
                _embedBuilderService.AddEffectFields(
                    embed, Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), _weaponItemStatsData.Effects, true, _culture);
            }

            if (_weaponItemData.WeaponData is not null)
            {
                _embedBuilderService.AddWeaponInfosField(embed, _weaponItemData.WeaponData, _weaponItemData.TwoHanded, _weaponItemTypeData, _culture);
            }
        }

        if (_spellsData.Any())
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Spells.Title", _culture),
                string.Join('\n', _spellsData.Select(x => $"- {Formatter.Bold(x.Name.ToString(_culture))}"))
            );
        }

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        yield return BreedComponentsBuilder.BreedButtonBuilder(_breedData, _culture);

        if (_weaponItemData is not null)
        {
            yield return ItemComponentsBuilder.ItemButtonBuilder(_weaponItemData, 1, _culture);
        }
    }
}
