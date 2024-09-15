using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "M";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly MonsterData _monsterData;
    private readonly int _selectedGrade;
    private readonly MonsterGradeData? _monsterGradeData;
    private readonly MonsterRaceData? _monsterRaceData;
    private readonly MonsterSuperRaceData? _monsterSuperRaceData;
    private readonly AlignmentData? _alignmentData;

    public MonsterMessageBuilder(EmbedBuilderService embedBuilderService, MonsterData monster, int selectedGrade = 1)
    {
        _embedBuilderService = embedBuilderService;
        _monsterData = monster;
        _selectedGrade = selectedGrade;
        _monsterGradeData = _monsterData.GetMonsterGradeData(selectedGrade);
        _monsterRaceData = monster.GetMonsterRaceData();
        _monsterSuperRaceData = _monsterRaceData?.GetMonsterSuperRaceData();
        _alignmentData = _monsterData.GetAlignmentData();
    }

    public static MonsterMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var monsterId) &&
            int.TryParse(parameters[1], out var selectedGrade))
        {
            var monsterData = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(monsterId);
            if (monsterData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new MonsterMessageBuilder(embedBuilderService, monsterData, selectedGrade);
            }
        }

        return null;
    }

    public static string GetPacket(int monsterId, int selectedGrade = 1)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, monsterId, selectedGrade);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = GradeButtons1Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = GradeButtons2Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Bestiary, BotTranslations.Embed_Monster_Author)
            .WithTitle($"{_monsterData.Name} ({_monsterData.Id}) - {BotTranslations.Rank} {_selectedGrade}")
            .WithThumbnail(await _monsterData.GetBigImagePathAsync(CdnImageSize.Size128));

        if (_monsterSuperRaceData is not null)
        {
            embed.AddField(BotTranslations.Embed_Field_MonsterSuperRaceData_Title, _monsterSuperRaceData.Name, true);
        }

        if (_monsterRaceData is not null)
        {
            embed.AddField(BotTranslations.Embed_Field_MonsterRaceData_Title, _monsterRaceData.Name, true);
        }

        if (_monsterGradeData is not null)
        {
            if (_alignmentData is not null)
            {
                embed.AddField(BotTranslations.Embed_Field_Alignment_Title, _alignmentData.Name, true);
            }

            StringBuilder characBuilder = new();

            characBuilder.Append(BotTranslations.ShortLevel);
            characBuilder.Append(' ');
            characBuilder.Append(Formatter.Bold(_monsterGradeData.Level.ToString()));
            characBuilder.Append('\n');

            if (_monsterGradeData.LifePoint is not null)
            {
                characBuilder.Append(Emojis.EffectHealthPoint);
                characBuilder.Append(Formatter.Bold(_monsterGradeData.LifePoint.Value.ToFormattedString()));
                characBuilder.Append(' ');
            }

            if (_monsterGradeData.ActionPoint is not null)
            {
                characBuilder.Append(Emojis.EffectAp);
                characBuilder.Append(Formatter.Bold(_monsterGradeData.ActionPoint.Value.ToFormattedString()));
                characBuilder.Append(' ');
            }

            if (_monsterGradeData.MovementPoint is not null)
            {
                characBuilder.Append(Emojis.EffectMp);
                characBuilder.Append(Formatter.Bold(_monsterGradeData.MovementPoint.Value.ToFormattedString()));
                characBuilder.Append(' ');
            }

            characBuilder.Append(Emojis.EffectApResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetActionPointDodge().ToFormattedString()));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EffectMpResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetMovementPointDodge().ToFormattedString()));
            characBuilder.Append("%\n");

            characBuilder.Append(Emojis.EffectNeutralResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetNeutralResistance().ToFormattedString()));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EffectEarthResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetEarthResistance().ToFormattedString()));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EffectFireResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetFireResistance().ToFormattedString()));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EffectWaterResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetWaterResistance().ToFormattedString()));
            characBuilder.Append("% ");

            characBuilder.Append(Emojis.EffectAirResistance);
            characBuilder.Append(Formatter.Bold(_monsterGradeData.GetAirResistance().ToFormattedString()));
            characBuilder.Append('%');

            embed.AddField(BotTranslations.Embed_Field_Characteristics_Title, characBuilder.ToString());

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
                yield return new(DiscordButtonStyle.Primary, GetPacket(_monsterData.Id,y), y.ToString(), _selectedGrade == y);
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
                yield return new(DiscordButtonStyle.Primary, GetPacket(_monsterData.Id, y), y.ToString(), _selectedGrade == y);
            }
        }
    }
}
