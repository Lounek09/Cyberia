using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands;

public abstract class PaginatedMessageBuilder<T> : ICustomMessageBuilder
{
    private const int c_rowPerPage = 25;

    private readonly DiscordEmbedBuilder _baseEmbedBuilder;
    private readonly string _title;

    protected readonly string _search;
    protected readonly int _selectedPageIndex;
    protected readonly int _totalPage;
    protected readonly CultureInfo? _culture;
    protected readonly IReadOnlyList<T> _data;

    public PaginatedMessageBuilder(DiscordEmbedBuilder baseEmbedBuilder, string title, List<T> data, string search, CultureInfo? culture, int selectedPageIndex)
    {
        _baseEmbedBuilder = baseEmbedBuilder;
        _title = title;
        _search = search;
        _culture = culture;
        _selectedPageIndex = selectedPageIndex;
        _totalPage = (int)Math.Ceiling(data.Count / (double)c_rowPerPage);

        var index = selectedPageIndex * c_rowPerPage;
        var count = Math.Min(data.Count - index, c_rowPerPage);
        _data = data.GetRange(index, count);
    }

    public async Task<T2> BuildAsync<T2>()
        where T2 : IDiscordMessageBuilder, new()
    {
        var message = new T2()
            .AddEmbed(await EmbedBuilder())
            .AddActionRowComponent(PaginationButtonsBuilder())
            .AddActionRowComponent(SelectBuilder());

        return (T2)message;
    }

    protected abstract IEnumerable<string> GetContent();

    protected abstract DiscordSelectComponent SelectBuilder();

    protected int PreviousPageIndex()
    {
        return _selectedPageIndex - 1 < 0
            ? _totalPage - 1
            : _selectedPageIndex - 1;
    }

    protected int NextPageIndex()
    {
        return _selectedPageIndex + 1 == _totalPage
            ? 0
            : _selectedPageIndex + 1;
    }

    protected abstract string PreviousPacketBuilder();

    protected abstract string NextPacketBuilder();

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _baseEmbedBuilder.WithTitle(_title)
            .WithDescription(string.Join('\n', GetContent()))
            .AddField(Constant.ZeroWidthSpace, $"{Translation.Get<BotTranslations>("Embed.Field.Page.Content", _culture)} {_selectedPageIndex + 1}/{_totalPage}");

        return Task.FromResult(embed);
    }

    private IEnumerable<DiscordButtonComponent> PaginationButtonsBuilder()
    {
        var disabled = _totalPage == 1;

        yield return new(DiscordButtonStyle.Primary,
            $"{PreviousPacketBuilder()}{PacketFormatter.Separator}P",
            Translation.Get<BotTranslations>("Button.Previous", _culture),
            disabled);

        yield return new(DiscordButtonStyle.Primary,
            $"{NextPacketBuilder()}{PacketFormatter.Separator}N",
            Translation.Get<BotTranslations>("Button.Next", _culture),
            disabled);
    }
}
