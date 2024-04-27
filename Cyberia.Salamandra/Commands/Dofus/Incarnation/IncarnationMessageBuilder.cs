using Cyberia.Api;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Commands.Dofus.Spell;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "INCA";
    public const int PacketVersion = 1;

    private readonly IncarnationData _incarnationData;
    private readonly ItemData? _itemData;
    private readonly ItemTypeData? _itemTypeData;
    private readonly List<SpellData> _spellsData;

    public IncarnationMessageBuilder(IncarnationData incarnationData)
    {
        _incarnationData = incarnationData;
        _itemData = incarnationData.GetItemData();
        _itemTypeData = _itemData?.GetItemTypeData();
        _spellsData = incarnationData.GetSpellsData().ToList();
    }

    public ItemTypeData? ItemTypeData => _itemTypeData;

    public static IncarnationMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var incarnationId))
        {
            var incarnartionData = DofusApi.Datacenter.IncarnationsData.GetIncarnationDataByItemId(incarnationId);
            if (incarnartionData is not null)
            {
                return new(incarnartionData);
            }
        }

        return null;
    }

    public static string GetPacket(int incarnationId)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, incarnationId);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        if (_spellsData.Count > 0)
        {
            message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData));
        }

        if (_itemData is not null)
        {
            message.AddComponents(ItemComponentsBuilder.ItemButtonBuilder(_itemData));
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Inventory, "Incarnations")
            .WithTitle($"{_incarnationData.Name} ({_incarnationData.Id})")
            .WithImageUrl(await _incarnationData.GetImgPath());

        if (_itemData is not null)
        {
            embed.WithDescription(string.IsNullOrEmpty(_itemData.Description) ? string.Empty : Formatter.Italic(_itemData.Description))
                .WithThumbnail(await _itemData.GetImagePath())
                .AddField("Niveau :", _itemData.Level.ToString(), true)
                .AddField("Type :", DofusApi.Datacenter.ItemsData.GetItemTypeNameById(_itemData.ItemTypeId), true)
                .AddEmptyField(true);

            IEnumerable<IEffect> effects = _incarnationData.GetRealEffects();
            if (effects.Any())
            {
                embed.AddEffectFields("Effets :", effects);
            }

            if (_itemData.WeaponData is not null)
            {
                embed.AddWeaponInfosField(_itemData.WeaponData, _itemData.TwoHanded, ItemTypeData);
            }
        }
        else
        {
            embed.WithDescription(Formatter.Italic("Incarnation non existante dans les données du jeu"))
                .WithThumbnail($"{DofusApi.Config.CdnUrl}/images/items/unknown.png");
        }

        if (_spellsData.Count > 0)
        {
            embed.AddField("Sorts :", string.Join('\n', _spellsData.Select(x => $"- {Formatter.Bold(x.Name)}")));
        }

        return embed;
    }
}
