using Cyberia.Api.Data;
using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "M";
    public const int PacketVersion = 1;

    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly MonsterData _monsterData;
    private readonly int _selectedGrade;
    private readonly MonsterGradeData? _monsterGradeData;
    private readonly MonsterRaceData? _monsterRaceData;
    private readonly MonsterSuperRaceData? _monsterSuperRaceData;
    private readonly AlignmentData? _alignmentData;
    private readonly CultureInfo? _culture;

    public MonsterMessageBuilder(IEmbedBuilderService embedBuilderService, MonsterData monster, int selectedGrade, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _monsterData = monster;
        _selectedGrade = selectedGrade;
        _monsterGradeData = _monsterData.GetMonsterGradeData(selectedGrade);
        _monsterRaceData = monster.GetMonsterRaceData();
        _monsterSuperRaceData = _monsterRaceData?.GetMonsterSuperRaceData();
        _alignmentData = _monsterData.GetAlignmentData();
        _culture = culture;
    }

    public static MonsterMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var monsterId) &&
            int.TryParse(parameters[1], out var selectedGrade))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var monsterData = dofusDatacenter.MonstersRepository.GetMonsterDataById(monsterId);
            if (monsterData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new MonsterMessageBuilder(embedBuilderService, monsterData, selectedGrade, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int monsterId, int selectedGrade = 1)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, monsterId, selectedGrade);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = GradeButtons1Builder();
        if (buttons.Any())
        {
            message.AddActionRowComponent(buttons);
        }

        buttons = GradeButtons2Builder();
        if (buttons.Any())
        {
            message.AddActionRowComponent(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Bestiary, Translation.Get<BotTranslations>("Embed.Monster.Author", _culture), _culture)
            .WithTitle($"{_monsterData.Name.ToString(_culture)} ({_monsterData.Id}) - {Translation.Get<BotTranslations>("Rank", _culture)} {_selectedGrade}")
            .WithThumbnail(await _monsterData.GetBigImagePathAsync(CdnImageSize.Size128));

        if (_monsterSuperRaceData is not null)
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.MonsterSuperRaceData.Title", _culture),
                _monsterSuperRaceData.Name.ToString(_culture),
                true);
        }

        if (_monsterRaceData is not null)
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.MonsterRaceData.Title", _culture),
                _monsterRaceData.Name.ToString(_culture),
                true);
        }

        if (_alignmentData is not null)
        {
            embed.AddField(
                Translation.Get<BotTranslations>("Embed.Field.Alignment.Title", _culture),
                _alignmentData.Name.ToString(_culture),
                true);
        }

        if (_monsterGradeData is not null)
        {
            StringBuilder characBuilder = new();

            characBuilder.Append(Translation.Get<BotTranslations>("ShortLevel", _culture));
            characBuilder.Append(' ');
            characBuilder.Append(Formatter.Bold(_monsterGradeData.Level.ToString()));
            characBuilder.Append('\n');

            if (_monsterGradeData.LifePoint is not null)
            {
                characBuilder.Append(Emojis.HealthPoint(_culture));
                characBuilder.Append(Formatter.Bold(_monsterGradeData.LifePoint.Value.ToFormattedString(_culture)));
                characBuilder.Append(' ');
            }

            if (_monsterGradeData.ActionPoint is not null)
            {
                characBuilder.Append(Emojis.ActionPoint(_culture));
                characBuilder.Append(Formatter.Bold(_monsterGradeData.ActionPoint.Value.ToFormattedString(_culture)));
                characBuilder.Append(' ');
            }

            if (_monsterGradeData.MovementPoint is not null)
            {
                characBuilder.Append(Emojis.MovementPoint(_culture));
                characBuilder.Append(Formatter.Bold(_monsterGradeData.MovementPoint.Value.ToFormattedString(_culture)));
                characBuilder.Append(' ');
            }

            characBuilder.Append(Emojis.ActionPointResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetActionPointDodge().ToFormattedString(_culture)));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.MovementPointResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetMovementPointDodge().ToFormattedString(_culture)));
            characBuilder.Append("%\n");

            characBuilder.Append(Emojis.NeutralResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetNeutralResistance().ToFormattedString(_culture)));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EarthResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetEarthResistance().ToFormattedString(_culture)));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.FireResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetFireResistance().ToFormattedString(_culture)));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.WaterResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetWaterResistance().ToFormattedString(_culture)));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.AirResistance(_culture));
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetAirResistance().ToFormattedString(_culture)));
            characBuilder.Append('%');

            embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Characteristics.Title", _culture), characBuilder.ToString());

            if (!string.IsNullOrEmpty(_monsterData.TrelloUrl))
            {
                embed.AddField(Constant.ZeroWidthSpace, _monsterData.TrelloUrl);
            }
        }

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> GradeButtons1Builder()
    {
        for (var i = 0; i < Constant.MaxButtonPerRow; i++)
        {
            var y = i + 1;
            if (_monsterData.GetMonsterGradeData(y) is not null)
            {
                yield return new DiscordButtonComponent(
                    DiscordButtonStyle.Primary,
                    GetPacket(_monsterData.Id, y),
                    y.ToString(),
                    _selectedGrade == y);
            }
        }
    }

    private IEnumerable<DiscordButtonComponent> GradeButtons2Builder()
    {
        for (var i = Constant.MaxButtonPerRow; i < Constant.MaxButtonPerRow * 2; i++)
        {
            var y = i + 1;
            if (_monsterData.GetMonsterGradeData(y) is not null)
            {
                yield return new DiscordButtonComponent(
                    DiscordButtonStyle.Primary,
                    GetPacket(_monsterData.Id, y),
                    y.ToString(),
                    _selectedGrade == y);
            }
        }
    }
}
