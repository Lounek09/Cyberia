using Cyberia.Api;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Commands.Dofus.Breed;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "IS";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly ItemSetData _itemSetData;
    private readonly int _nbItemSelected;
    private readonly IEnumerable<ItemData> _itemsData;
    private readonly BreedData? _breedData;
    private readonly CultureInfo? _culture;

    public ItemSetMessageBuilder(EmbedBuilderService embedBuilderService, ItemSetData itemSetData, int nbItemSelected, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _itemSetData = itemSetData;
        _nbItemSelected = nbItemSelected;
        _itemsData = itemSetData.GetItemsData();
        _breedData = itemSetData.GetBreedData();
        _culture = culture;
    }

    public static ItemSetMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var itemSetId) &&
            int.TryParse(parameters[1], out var nbItemSelected))
        {
            var itemSetData = DofusApi.Datacenter.ItemSetsRepository.GetItemSetDataById(itemSetId);
            if (itemSetData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, itemSetData, nbItemSelected, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int itemSetId, int nbItemSelected)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, itemSetId, nbItemSelected);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = ButtonsBuilder(0);
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = ButtonsBuilder(Constant.MaxButtonPerRow);
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = OtherButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        if (_itemsData.Any())
        {
            message.AddComponents(ItemComponentsBuilder.ItemsSelectBuilder(0, _itemsData, _culture));
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.ItemSet.Author", _culture), _culture)
            .WithTitle($"{_itemSetData.Name.ToString(_culture)} ({_itemSetData.Id})")
            .AddField(Translation.Get<BotTranslations>("Embed.Field.Level.Title", _culture), _itemSetData.GetLevel().ToString());

        if (_itemsData.Any())
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Items.Title", _culture),
                string.Join('\n', _itemsData.Select(x =>
                {
                    return $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.Level} {Formatter.Bold(x.Name.ToString(_culture))}";
                }))
            );
        }

        var effects = _itemSetData.GetEffects(_nbItemSelected);
        embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), effects, true, _culture);

        return Task.FromResult(embed);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder(int startIndex)
    {

        var endIndex = Math.Min(startIndex + Constant.MaxButtonPerRow, _itemSetData.Effects.Count);
        for (var i = startIndex; i < endIndex; i++)
        {
            var y = i + 1;
            yield return new DiscordButtonComponent(
                DiscordButtonStyle.Primary,
                GetPacket(_itemSetData.Id, y),
                $"{y}/{_itemSetData.Effects.Count}",
                _nbItemSelected == y);
        }
    }

    private IEnumerable<DiscordButtonComponent> OtherButtonsBuilder()
    {
        if (_breedData is not null)
        {
            yield return BreedComponentsBuilder.BreedButtonBuilder(_breedData, _culture);
        }
    }
}
