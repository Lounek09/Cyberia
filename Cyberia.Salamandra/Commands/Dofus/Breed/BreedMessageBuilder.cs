using Cyberia.Api;
using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class BreedMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "G";
    public const int PACKET_VERSION = 1;

    private readonly BreedData _breedData;
    private readonly List<SpellData> _spellsData;
    private readonly SpellData? _specialSpellData;
    private readonly ItemSetData? _itemSetData;

    public BreedMessageBuilder(BreedData breedData)
    {
        _breedData = breedData;
        _spellsData = breedData.GetSpellsData().ToList();
        _specialSpellData = breedData.GetSpecialSpellData();
        _itemSetData = breedData.GetItemSetData();
    }

    public static BreedMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var breedId))
        {
            var breedData = DofusApi.Datacenter.BreedsData.GetBreedDataById(breedId);
            if (breedData is not null)
            {
                return new(breedData);
            }
        }

        return null;
    }

    public static string GetPacket(int breedId)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, breedId);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        if (_spellsData.Count > 0)
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

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Breeds, "Classes")
            .WithTitle($"{_breedData.LongName} ({_breedData.Id})")
            .WithDescription(Formatter.Italic(_breedData.Description))
            .WithThumbnail(_breedData.GetIconImagePath())
            .WithImageUrl(_breedData.GetPreferenceWeaponsImagePath())
            .AddField("Caractérisques :", StatsBoostCostContent());

        if (_spellsData.Count > 0)
        {
            embed.AddField("Sorts :", string.Join('\n', _spellsData.Select(x => $"- Niv.{x.GetNeededLevel()} {Formatter.Bold(x.Name)}")));
        }

        if (DofusApi.Config.Temporis)
        {
            embed.AddField("Temporis :", $"{Formatter.Bold(_breedData.TemporisPassiveName)} :\n{_breedData.TemporisPassiveDescription}");
        }

        return Task.FromResult(embed);
    }

    private string StatsBoostCostContent()
    {
        StringBuilder builder = new();

        builder.Append("- ");
        builder.Append(Formatter.Bold("Vitalité"));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.VitalityBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold("Sagesse"));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.WisdomBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold("Force"));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.StrengthBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold("Intelligence"));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.IntelligenceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold("Chance"));
        builder.Append(" :\n");
        AppendStatBoostCost(builder, _breedData.ChanceBoostCost);

        builder.Append("- ");
        builder.Append(Formatter.Bold("Agilité"));
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
            builder.Append(" pour ");
            builder.Append(boostCost[i][1]);
            builder.Append(" à partir de ");
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
