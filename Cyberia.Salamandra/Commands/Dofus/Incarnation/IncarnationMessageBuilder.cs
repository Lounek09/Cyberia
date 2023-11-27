using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class IncarnationMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "INCA";
    public const int PACKET_VERSION = 1;

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

    public static IncarnationMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
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
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, incarnationId);
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
        var embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Incarnations")
            .WithTitle($"{_incarnationData.Name} ({_incarnationData.Id})")
            .WithImageUrl(await _incarnationData.GetImgPath());

        if (_itemData is not null)
        {
            embed.WithDescription(string.IsNullOrEmpty(_itemData.Description) ? "" : Formatter.Italic(_itemData.Description))
                .WithThumbnail(await _itemData.GetImagePath())
                .AddField("Niveau :", _itemData.Level.ToString(), true)
                .AddField("Type :", DofusApi.Datacenter.ItemsData.GetItemTypeNameById(_itemData.ItemTypeId), true)
                .AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

            IEnumerable<IEffect> effects = _incarnationData.GetEffects();
            if (effects.Any())
            {
                embed.AddEffectFields("Effets :", effects);
            }

            if (_itemData.WeaponData is not null)
            {
                embed.AddWeaponInfosField(_itemData.WeaponData, _itemData.TwoHanded, _itemTypeData);
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
