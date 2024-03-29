using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class CraftMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "CR";
    public const int PACKET_VERSION = 1;
    public const int MAX_QTE = 99999;

    private readonly CraftData _craftData;
    private readonly ItemData? _itemData;
    private readonly int _qte;
    private readonly bool _recursive;

    public CraftMessageBuilder(CraftData craftData, int qte = 1, bool recursive = false)
    {
        _craftData = craftData;
        _itemData = craftData.GetItemData();
        _qte = qte;
        _recursive = recursive;
    }

    public static CraftMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var craftId) &&
            int.TryParse(parameters[1], out var qte) &&
            bool.TryParse(parameters[2], out var recursive))
        {
            var craftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(craftId);
            if (craftData is not null)
            {
                return new(craftData, qte, recursive);
            }
        }

        return null;
    }

    public static string GetPacket(int craftId, int qte = 1, bool recursive = false)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, craftId, qte, recursive);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
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
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Jobs, "Calculateur de crafts")
            .WithCraftDescription(_craftData, _qte, _recursive);

        if (_itemData is not null)
        {
            embed.WithTitle($"Craft : {_qte.ToStringThousandSeparator()}x {Formatter.Sanitize(_itemData.Name)} ({_craftData.Id})")
                .WithThumbnail(await _itemData.GetImagePath());
        }

        var weight = _recursive ? _craftData.GetWeightWithSubCraft() : _craftData.GetWeight();
        embed.AddField("Poids :", $"{Formatter.Bold(weight.ToStringThousandSeparator())} pod{(weight > 1 ? "s" : string.Empty)} par craft" + (_qte > 1 ? $", {Formatter.Bold((weight * _qte).ToStringThousandSeparator())} au total" : string.Empty));

        var time = $"{Formatter.Bold((_recursive ? _craftData.GetTimePerCraftWithSubCraft(1) : _craftData.GetTimePerCraft(1)).ToString(@"mm\mss\sfff"))} par craft";
        if (_qte > 1)
        {
            var totalTime = _recursive ? _craftData.GetTimePerCraftWithSubCraft(_qte) : _craftData.GetTimePerCraft(_qte);
            var format = (totalTime.TotalDays < 1 ? string.Empty : @"%d\d") + (totalTime.TotalHours < 1 ? string.Empty : @"hh\h") + @"mm\mss\sfff";
            time += $"\n{Formatter.Bold(totalTime.ToString(format))} au total";
        }
        embed.AddField("Temps de craft approximatif:", time);

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> LessButtonsBuilder()
    {
        yield return new(ButtonStyle.Danger, GetPacket(_craftData.Id, _qte - 1000, _recursive), "-1000", (_qte - 1000) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_craftData.Id, _qte - 100, _recursive), "-100", (_qte - 100) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_craftData.Id, _qte - 10, _recursive), "-10", (_qte - 10) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_craftData.Id, _qte - 1, _recursive), "-1", (_qte - 1) < 1);
    }

    private IEnumerable<DiscordButtonComponent> MoreButtonsBuilder()
    {
        yield return new(ButtonStyle.Success, GetPacket(_craftData.Id, _qte + 1000, _recursive), "+1000", _qte + 1000 > MAX_QTE);
        yield return new(ButtonStyle.Success, GetPacket(_craftData.Id, _qte + 100, _recursive), "+100", _qte + 100 > MAX_QTE);
        yield return new(ButtonStyle.Success, GetPacket(_craftData.Id, _qte + 10, _recursive), "+10", _qte + 10 > MAX_QTE);
        yield return new(ButtonStyle.Success, GetPacket(_craftData.Id, _qte + 1, _recursive), "+1", _qte + 1 > MAX_QTE);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {

        yield return new(ButtonStyle.Primary, GetPacket(_craftData.Id, _qte, !_recursive), $"{(_recursive ? "Masquer" : "Afficher")} les sous crafts", !_craftData.HasSubCraft());

        if (_itemData is not null)
        {
            yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _qte);
        }
    }
}
