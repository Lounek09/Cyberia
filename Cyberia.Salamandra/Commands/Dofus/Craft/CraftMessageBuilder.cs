using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.DSharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "CR";
    public const int PacketVersion = 1;
    public const int MaxQuantity = 99999;

    private readonly CraftData _craftData;
    private readonly ItemData? _itemData;
    private readonly int _quantity;
    private readonly bool _recursive;

    public CraftMessageBuilder(CraftData craftData, int quantity = 1, bool recursive = false)
    {
        _craftData = craftData;
        _itemData = craftData.GetItemData();
        _quantity = quantity;
        _recursive = recursive;
    }

    public static CraftMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var craftId) &&
            int.TryParse(parameters[1], out var quantity) &&
            bool.TryParse(parameters[2], out var recursive))
        {
            var craftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(craftId);
            if (craftData is not null)
            {
                return new(craftData, quantity, recursive);
            }
        }

        return null;
    }

    public static string GetPacket(int craftId, int quantity = 1, bool recursive = false)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, craftId, quantity, recursive);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(LessButtonsBuilder())
            .AddComponents(MoreButtonsBuilder());

        var buttons = ButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Jobs, BotTranslations.Embed_Craft_Author)
            .WithCraftDescription(_craftData, _quantity, _recursive);

        if (_itemData is not null)
        {
            embed.WithTitle($"{BotTranslations.Embed_Craft_Title}{_quantity.ToFormattedString()}x {Formatter.Sanitize(_itemData.Name)} ({_craftData.Id})")
                .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128));
        }

        var weight = _recursive ? _craftData.GetWeightWithSubCraft() : _craftData.GetWeight();
        var formattedWeight = Formatter.Bold(weight.ToFormattedString());

        var totalWeight = weight * _quantity;
        var formattedTotalWeight = _quantity > 1 ? Formatter.Bold(totalWeight.ToFormattedString()) : string.Empty;

        var time = _recursive ? _craftData.GetTimePerCraftWithSubCraft(1) : _craftData.GetTimePerCraft(1);
        var formattedTime = Formatter.Bold(time.ToString(@"mm\mss\sfff"));

        var formattedTotalTime = string.Empty;
        if (_quantity > 1)
        {
            var totalTime = _recursive ? _craftData.GetTimePerCraftWithSubCraft(_quantity) : _craftData.GetTimePerCraft(_quantity);
            var format = (totalTime.TotalDays < 1 ? string.Empty : @"%d\d") + (totalTime.TotalHours < 1 ? string.Empty : @"hh\h") + @"mm\mss\sfff";

            formattedTotalTime = Formatter.Bold(totalTime.ToString(format));
        }

        embed.AddField(BotTranslations.Embed_Field_Weight_Title, Translation.Format(BotTranslations.Embed_Field_Weight_Content, formattedWeight, formattedTotalWeight));
        embed.AddField(BotTranslations.Embed_Field_CraftTime_Title, Translation.Format(BotTranslations.Embed_Field_CraftTime_Content, formattedTime, formattedTotalTime));

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> LessButtonsBuilder()
    {
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 1000, _recursive), "-1000", (_quantity - 1000) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 100, _recursive), "-100", (_quantity - 100) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 10, _recursive), "-10", (_quantity - 10) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 1, _recursive), "-1", (_quantity - 1) < 1);
    }

    private IEnumerable<DiscordButtonComponent> MoreButtonsBuilder()
    {
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 1000, _recursive), "+1000", _quantity + 1000 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 100, _recursive), "+100", _quantity + 100 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 10, _recursive), "+10", _quantity + 10 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 1, _recursive), "+1", _quantity + 1 > MaxQuantity);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        var subCraftButtonlabel = _recursive ? BotTranslations.Button_SubCraft_Hide : BotTranslations.Button_SubCraft_Display;
        yield return new(DiscordButtonStyle.Primary, GetPacket(_craftData.Id, _quantity, !_recursive), subCraftButtonlabel, !_craftData.HasSubCraft());

        if (_itemData is not null)
        {
            yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _quantity);
        }
    }
}
