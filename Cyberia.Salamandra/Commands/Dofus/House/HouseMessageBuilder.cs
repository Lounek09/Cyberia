using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class HouseMessageBuilder : CustomMessageBuilder
    {
        private readonly House _house;
        private readonly Map? _outdoorMap;
        private readonly List<Map> _roomsMap;
        private Map? _currentMap;
        private int _selectedIndex;

        public HouseMessageBuilder(House house) :
            base()
        {
            _house = house;
            _outdoorMap = house.GetOutdoorMap();
            _roomsMap = house.GetMaps();
            _currentMap = _outdoorMap is not null ? _outdoorMap : _roomsMap.Count > 0 ? _roomsMap[0] : null;
            _selectedIndex = 0;
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Houses, "Agence immobilière")
                .WithTitle($"{_house.Name}{(string.IsNullOrEmpty(_house.GetCoordinate()) ? "" : $" {_house.GetCoordinate()}")} ({_house.Id})")
                .WithDescription(string.IsNullOrEmpty(_house.Description) ? "" : Formatter.Italic(_house.Description));

            if (_currentMap is not null)
                embed.ImageUrl = _currentMap.GetImgPath();

            embed.AddField("Pièce :", _house.RoomNumber == 0 ? "?" : _house.RoomNumber.ToString(), true);
            embed.AddField("Coffre :", _house.ChestNumber == 0 ? "?" : _house.ChestNumber.ToString(), true);
            embed.AddField("Prix :", _house.Price == 0 ? "?" : _house.Price.ToStringThousandSeparator(), true);

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            if (_outdoorMap is not null)
                options.Add(new("Extérieur", $"{_outdoorMap.Id}|0", isDefault: _selectedIndex == 0));

            for (int i = 1; i < _roomsMap.Count + 1; i++)
            {
                int index = _outdoorMap is null ? i - 1 : i;
                options.Add(new($"Pièce {i}", $"{_roomsMap[i - 1].Id}|{index}", isDefault: index == _selectedIndex));
            }

            return new("select", "Sélectionne une pièce pour l'afficher", options);
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                response.AddComponents(select);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                followupMessage.AddComponents(select);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (e.Id.Equals("select"))
            {
                string[] args = e.Interaction.Data.Values.First().Split("|");

                if (args.Length > 1 && int.TryParse(args[0], out int id) && int.TryParse(args[1], out _selectedIndex))
                {
                    _currentMap = Bot.Instance.Api.Datacenter.MapsData.GetMapById(id);
                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
