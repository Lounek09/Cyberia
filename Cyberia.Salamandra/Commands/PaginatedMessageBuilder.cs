﻿using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands
{
    public enum PaginatedAction
    {
        Nothing,
        Previous,
        Next
    }

    public abstract class PaginatedMessageBuilder<T> : ICustomMessageBuilder
    {
        private const int ROW_PER_PAGE = 25;

        private readonly DofusEmbedCategory _category;
        private readonly string _authorText;
        private readonly string _title;

        protected readonly int _selectedPageIndex;
        protected readonly int _totalPage;
        protected readonly List<T> _data;

        public PaginatedMessageBuilder(DofusEmbedCategory category, string authorText, string title, List<T> data, int selectedPageIndex)
        {
            _category = category;
            _authorText = authorText;
            _title = title;
            _selectedPageIndex = selectedPageIndex;
            _totalPage = (int)Math.Ceiling(data.Count / (double)ROW_PER_PAGE);

            int index = selectedPageIndex * ROW_PER_PAGE;
            int count = Math.Min(data.Count - index, ROW_PER_PAGE);
            _data = data.GetRange(index, count);
        }

        public async Task<T2> GetMessageAsync<T2>() where T2 : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T2()
                .AddEmbed(await EmbedBuilder())
                .AddComponents(PaginationButtonsBuilder())
                .AddComponents(SelectBuilder());

            return (T2)message;
        }

        protected abstract IEnumerable<string> GetContent();

        protected abstract DiscordSelectComponent SelectBuilder();

        protected int PreviousPageIndex()
        {
            return _selectedPageIndex - 1 < 0 ? _totalPage - 1 : _selectedPageIndex - 1;
        }

        protected abstract string PreviousPacketBuilder();

        protected int NextPageIndex()
        {
            return _selectedPageIndex + 1 == _totalPage ? 0 : _selectedPageIndex + 1;
        }

        protected abstract string NextPacketBuilder();

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(_category, _authorText)
                .WithTitle(_title)
                .WithDescription(string.Join('\n', GetContent()))
                .AddField(Constant.ZERO_WIDTH_SPACE, $"Page {_selectedPageIndex + 1}/{_totalPage}");

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> PaginationButtonsBuilder()
        {
            return new()
            {
                new(ButtonStyle.Primary, $"{PreviousPacketBuilder()}{InteractionManager.PACKET_PARAMETER_SEPARATOR}P", "Précédent", _totalPage == 1),
                new(ButtonStyle.Primary, $"{NextPacketBuilder()}{InteractionManager.PACKET_PARAMETER_SEPARATOR}N", "Suivant", _totalPage == 1)
            };
        }
    }
}