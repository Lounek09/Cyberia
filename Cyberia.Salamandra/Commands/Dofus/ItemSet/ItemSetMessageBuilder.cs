using Cyberia.Api;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class ItemSetMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "IS";
    public const int PACKET_VERSION = 1;

    private readonly ItemSetData _itemSetData;
    private readonly int _nbItemSelected;
    private readonly List<ItemData> _itemsData;
    private readonly BreedData? _breedData;

    public ItemSetMessageBuilder(ItemSetData itemSetData, int nbItemSelected)
    {
        _itemSetData = itemSetData;
        _nbItemSelected = nbItemSelected;
        _itemsData = itemSetData.GetItemsData().ToList();
        _breedData = itemSetData.GetBreedData();
    }

    public static ItemSetMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var itemSetId) &&
            int.TryParse(parameters[1], out var nbItemSelected))
        {
            var itemSetData = DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(itemSetId);
            if (itemSetData is not null)
            {
                return new(itemSetData, nbItemSelected);
            }
        }

        return null;
    }

    public static string GetPacket(int itemSetId, int nbItemSelected = 2)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, itemSetId, nbItemSelected);
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

        if (_itemsData.Count > 0)
        {
            message.AddComponents(ItemComponentsBuilder.ItemsSelectBuilder(0, _itemsData));
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Inventory, "Panoplies")
            .WithTitle($"{_itemSetData.Name} ({_itemSetData.Id})")
            .AddField("Niveau :", _itemSetData.GetLevel().ToString());

        if (_itemsData.Count > 0)
        {
            embed.AddField("Items :", string.Join('\n', _itemsData.Select(x => $"- Niv.{x.Level} {Formatter.Bold(x.Name)}")));
        }

        var effects = _itemSetData.GetEffects(_nbItemSelected);
        if (effects.Any())
        {
            embed.AddEffectFields("Effets :", effects);
        }

        return Task.FromResult(embed);
    }

    private List<DiscordButtonComponent> Buttons1Builder()
    {
        List<DiscordButtonComponent> components = [];

        for (var i = 1; i < _itemSetData.ItemsId.Count + 1 && i < 6; i++)
        {
            components.Add(new(ButtonStyle.Primary, GetPacket(_itemSetData.Id, i), $"{i}/{_itemSetData.ItemsId.Count}", _nbItemSelected == i));
        }

        return components;
    }

    private List<DiscordButtonComponent> Buttons2Builder()
    {
        List<DiscordButtonComponent> components = [];

        for (var i = 6; i < _itemSetData.ItemsId.Count + 1 && i < 10; i++)
        {
            components.Add(new(ButtonStyle.Primary, GetPacket(_itemSetData.Id, i), $"{i}/{_itemSetData.ItemsId.Count}", _nbItemSelected == i));
        }

        if (_breedData is not null)
        {
            components.Add(BreedComponentsBuilder.BreedButtonBuilder(_breedData));
        }

        return components;
    }
}
