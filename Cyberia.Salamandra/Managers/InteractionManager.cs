﻿using Cyberia.Salamandra.Commands;
using Cyberia.Salamandra.Commands.Data;
using Cyberia.Salamandra.Commands.Dofus;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.Managers
{
    public static class InteractionManager
    {
        public const char PACKET_PARAMETER_SEPARATOR = '|';

        private static readonly Regex _selectComponentPacketRegex = new(@"SELECT\d+", RegexOptions.Compiled);
        private static readonly Dictionary<string, Func<int, string[], ICustomMessageBuilder?>> _factory = new()
        {
            { BreedMessageBuilder.PACKET_HEADER, BreedMessageBuilder.Create },
            { CraftMessageBuilder.PACKET_HEADER, CraftMessageBuilder.Create },
            { PaginatedCraftMessageBuilder.PACKET_HEADER, PaginatedCraftMessageBuilder.Create },
            { HouseMessageBuilder.PACKET_HEADER, HouseMessageBuilder.Create },
            { PaginatedHouseMessageBuilder.PACKET_HEADER, PaginatedHouseMessageBuilder.Create },
            { IncarnationMessageBuilder.PACKET_HEADER, IncarnationMessageBuilder.Create },
            { PaginatedIncarnationMessageBuilder.PACKET_HEADER, PaginatedIncarnationMessageBuilder.Create },
            { ItemMessageBuilder.PACKET_HEADER, ItemMessageBuilder.Create },
            { PaginatedItemMessageBuilder.PACKET_HEADER, PaginatedItemMessageBuilder.Create },
            { ItemSetMessageBuilder.PACKET_HEADER, ItemSetMessageBuilder.Create },
            { PaginatedItemSetMessageBuilder.PACKET_HEADER, PaginatedItemSetMessageBuilder.Create },
            { LangsMessageBuilder.PACKET_HEADER, LangsMessageBuilder.Create },
            { MapMessageBuilder.PACKET_HEADER, MapMessageBuilder.Create },
            { PaginatedMapMessageBuilder.PACKET_HEADER, PaginatedMapMessageBuilder.Create },
            { PaginatedMapSubAreaMessageBuilder.PACKET_HEADER, PaginatedMapSubAreaMessageBuilder.Create },
            { PaginatedMapAreaMessageBuilder.PACKET_HEADER, PaginatedMapAreaMessageBuilder.Create },
            { MonsterMessageBuilder.PACKET_HEADER, MonsterMessageBuilder.Create },
            { PaginatedMonsterMessageBuilder.PACKET_HEADER, PaginatedMonsterMessageBuilder.Create },
            { QuestMessageBuilder.PACKET_HEADER, QuestMessageBuilder.Create },
            { PaginatedQuestMessageBuilder.PACKET_HEADER, PaginatedQuestMessageBuilder.Create },
            { SpellMessageBuilder.PACKET_HEADER, SpellMessageBuilder.Create },
            { PaginatedSpellMessageBuilder.PACKET_HEADER, PaginatedSpellMessageBuilder.Create }
        };

        public static string ComponentPacketBuilder(string header, int version, params object[] parameters)
        {
            StringBuilder packet = new();

            packet.Append(header);
            packet.Append(PACKET_PARAMETER_SEPARATOR);
            packet.Append(version);

            if (parameters.Length > 0)
            {
                packet.Append(PACKET_PARAMETER_SEPARATOR);
                packet.Append(string.Join(PACKET_PARAMETER_SEPARATOR, parameters));
            }

            return packet.ToString();
        }

        public static string SelectComponentPacketBuilder(int uniqueIndex)
        {
            return $"SELECT{uniqueIndex}";
        }

        public static async Task OnComponentInteractionCreated(DiscordClient _, ComponentInteractionCreateEventArgs e)
        {
            if (e.User.IsBot || string.IsNullOrEmpty(e.Id))
                return;

            DiscordInteractionResponseBuilder response = new();

            string[] decomposedPacket = (_selectComponentPacketRegex.IsMatch(e.Id) ? e.Values[0] : e.Id).Split(PACKET_PARAMETER_SEPARATOR);

            if (decomposedPacket.Length < 2)
            {
                response.WithContent("Le message est trop ancien").AsEphemeral();
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                return;
            }

            string header = decomposedPacket[0];
            int version = int.Parse(decomposedPacket[1]);
            string[] parameters = decomposedPacket.Length > 2 ? decomposedPacket[2..] : Array.Empty<string>();

            if (!_factory.TryGetValue(header, out Func<int, string[], ICustomMessageBuilder?>? builder))
            {
                response.WithContent("Bouton inconnu, le message est probablement trop ancien").AsEphemeral();
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                return;
            }

            ICustomMessageBuilder? message = builder(version, parameters);
            if (message is null)
            {
                response.WithContent("Un problème a eu lieu lors de la création de la réponse, le message est probablement trop ancien").AsEphemeral();
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                return;
            }

            response = await message.GetMessageAsync<DiscordInteractionResponseBuilder>();
            await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, response);
        }
    }
}