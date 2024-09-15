using Cyberia.Api;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Commands.Dofus.Breed;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

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

    public ItemSetMessageBuilder(EmbedBuilderService embedBuilderService, ItemSetData itemSetData, int nbItemSelected)
    {
        _embedBuilderService = embedBuilderService;
        _itemSetData = itemSetData;
        _nbItemSelected = nbItemSelected;
        _itemsData = itemSetData.GetItemsData();
        _breedData = itemSetData.GetBreedData();
    }

    public static ItemSetMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
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

                return new(embedBuilderService, itemSetData, nbItemSelected);
            }
        }

        return null;
    }

    public static string GetPacket(int itemSetId, int nbItemSelected)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, itemSetId, nbItemSelected);
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
            message.AddComponents(ItemComponentsBuilder.ItemsSelectBuilder(0, _itemsData));
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, BotTranslations.Embed_ItemSet_Author)
            .WithTitle($"{_itemSetData.Name} ({_itemSetData.Id})")
            .AddField(BotTranslations.Embed_Field_Level_Title, _itemSetData.GetLevel().ToString());

        if (_itemsData.Any())
        {
            embed.AddField(BotTranslations.Embed_Field_Items_Title,
                string.Join('\n', _itemsData.Select(x => $"- {BotTranslations.ShortLevel}{x.Level} {Formatter.Bold(x.Name)}")));
        }

        var effects = _itemSetData.GetEffects(_nbItemSelected);
        if (effects.Any())
        {
            embed.AddEffectFields(BotTranslations.Embed_Field_Effects_Title, effects, true);
        }

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
            yield return BreedComponentsBuilder.BreedButtonBuilder(_breedData);
        }
    }
}
