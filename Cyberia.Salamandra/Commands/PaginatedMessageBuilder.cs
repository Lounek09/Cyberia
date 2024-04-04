using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands;

public enum PaginatedAction
{
    None,
    Previous,
    Next
}

public abstract class PaginatedMessageBuilder<T> : ICustomMessageBuilder
{
    private const int c_rowPerPage = 25;

    private readonly EmbedCategory _category;
    private readonly string _authorText;
    private readonly string _title;

    protected readonly int _selectedPageIndex;
    protected readonly int _totalPage;
    protected readonly List<T> _data;

    public PaginatedMessageBuilder(EmbedCategory category, string authorText, string title, List<T> data, int selectedPageIndex)
    {
        _category = category;
        _authorText = authorText;
        _title = title;
        _selectedPageIndex = selectedPageIndex;
        _totalPage = (int)Math.Ceiling(data.Count / (double)c_rowPerPage);

        var index = selectedPageIndex * c_rowPerPage;
        var count = Math.Min(data.Count - index, c_rowPerPage);
        _data = data.GetRange(index, count);
    }

    public async Task<T2> GetMessageAsync<T2>() where T2 : IDiscordMessageBuilder, new()
    {
        var message = new T2()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(PaginationButtonsBuilder())
            .AddComponents(SelectBuilder());

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
        var embed = EmbedManager.CreateEmbedBuilder(_category, _authorText)
            .WithTitle(_title)
            .WithDescription(string.Join('\n', GetContent()))
            .AddField(Constant.ZeroWidthSpace, $"Page {_selectedPageIndex + 1}/{_totalPage}");

        return Task.FromResult(embed);
    }

    private IEnumerable<DiscordButtonComponent> PaginationButtonsBuilder()
    {
        yield return new(ButtonStyle.Primary, $"{PreviousPacketBuilder()}{InteractionManager.PacketParameterSeparator}P", "Précédent", _totalPage == 1);
        yield return new(ButtonStyle.Primary, $"{NextPacketBuilder()}{InteractionManager.PacketParameterSeparator}N", "Suivant", _totalPage == 1);
    }
}
