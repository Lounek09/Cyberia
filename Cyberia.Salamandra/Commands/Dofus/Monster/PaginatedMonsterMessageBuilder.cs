using Cyberia.Api.Data;
using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class PaginatedMonsterMessageBuilder : PaginatedMessageBuilder<MonsterData>
{
    public const string PacketHeader = "PM";
    public const int PacketVersion = 2;

    public PaginatedMonsterMessageBuilder(
        EmbedBuilderService embedBuilderService,
        List<MonsterData> monstersData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Bestiary, Translation.Get<BotTranslations>("Embed.Monster.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedMonster.Title", culture),
        monstersData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedMonsterMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var monstersData = dofusDatacenter.MonstersRepository.GetMonstersDataByName(parameters[1], culture).ToList();
            if (monstersData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new PaginatedMonsterMessageBuilder(embedBuilderService, monstersData, parameters[1], culture, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var monsterData in _data)
        {
            var minLevel = monsterData.GetMinLevel();
            var maxLevel = monsterData.GetMaxLevel();

            yield return $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{minLevel}{(minLevel == maxLevel ? string.Empty : $"-{maxLevel}")} " +
                $"{Formatter.Bold($"{monsterData.Name.ToString(_culture)}{(monsterData.BreedSummon ? $" ({Translation.Get<BotTranslations>("Summon", _culture)})" : string.Empty)}")} ({monsterData.Id})";
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return MonsterComponentsBuilder.MonstersSelectBuilder(0, _data, _culture);
    }

    protected override string PreviousPacketBuilder()
    {
        return GetPacket(_search, PreviousPageIndex());
    }

    protected override string NextPacketBuilder()
    {
        return GetPacket(_search, NextPageIndex());
    }
}
