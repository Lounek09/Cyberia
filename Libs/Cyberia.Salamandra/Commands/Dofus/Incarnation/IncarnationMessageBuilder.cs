using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Commands.Dofus.Spell;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "INCA";
    public const int PacketVersion = 1;

    private readonly DofusApiConfig _dofusApiConfig;
    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly IncarnationData _incarnationData;
    private readonly ItemData? _itemData;
    private readonly string _itemName;
    private readonly ItemTypeData? _itemTypeData;
    private readonly IEnumerable<SpellData> _spellsData;
    private readonly CultureInfo? _culture;

    public IncarnationMessageBuilder(DofusApiConfig dofusApiConfig, IEmbedBuilderService embedBuilderService, IncarnationData incarnationData, CultureInfo? culture)
    {
        _dofusApiConfig = dofusApiConfig;
        _embedBuilderService = embedBuilderService;
        _incarnationData = incarnationData;
        _itemData = incarnationData.GetItemData();
        _itemName = incarnationData.GetItemName(culture);
        _itemTypeData = _itemData?.GetItemTypeData();
        _spellsData = incarnationData.GetSpellsData();
        _culture = culture;
    }

    public static IncarnationMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var incarnationId))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var incarnartionData = dofusDatacenter.IncarnationsRepository.GetIncarnationDataByItemId(incarnationId);
            if (incarnartionData is not null)
            {
                var dofusApiConfig = provider.GetRequiredService<DofusApiConfig>();
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(dofusApiConfig, embedBuilderService, incarnartionData, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int incarnationId)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, incarnationId);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        if (_spellsData.Any())
        {
            message.AddActionRowComponent(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData, _culture));
        }

        if (_itemData is not null)
        {
            message.AddActionRowComponent(ItemComponentsBuilder.ItemButtonBuilder(_itemData, 1, _culture));
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.Incarnation.Author", _culture), _culture)
            .WithTitle($"{_itemName} ({_incarnationData.Id})")
            .WithImageUrl(await _incarnationData.GetBigImagePathAsync(CdnImageSize.Size512));

        if (_itemData is not null)
        {
            embed.WithDescription(string.IsNullOrEmpty(_itemData.Description) ? string.Empty : Formatter.Italic(_itemData.Description.ToString(_culture)))
                .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128))
                .AddField(Translation.Get<BotTranslations>("Embed.Field.Level.Title", _culture), _itemData.Level.ToString(), true)
                .AddField(Translation.Get<BotTranslations>("Embed.Field.ItemType.Title", _culture), _itemData.GetItemTypeName(_culture), true)
                .AddEmptyField(true);

            var effects = _incarnationData.GetRealEffects();
            _embedBuilderService.AddEffectFields(embed, Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), effects, true, _culture);

            if (_itemData.WeaponData is not null)
            {
                _embedBuilderService.AddWeaponInfosField(embed, _itemData.WeaponData, _itemData.TwoHanded, _itemTypeData, _culture);
            }
        }
        else
        {
            embed.WithDescription(Formatter.Italic(Translation.Get<BotTranslations>("Embed.Incarnation.Description.NotFound", _culture)))
                .WithThumbnail($"{_dofusApiConfig.CdnUrl}/images/items/unknown.png");
        }

        if (_spellsData.Any())
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Spells.Title", _culture),
                string.Join('\n', _spellsData.Select(x => $" - {Formatter.Bold(x.Name.ToString(_culture))}")));
        }

        return embed;
    }
}
