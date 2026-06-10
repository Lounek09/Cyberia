using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Other.Rgpd;

public sealed class RgpdInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "rgpd";
    public const string CommandDescription = "Allows you to manage your personal data.";

    public const string Get_CommandName = "get";
    public const string Get_CommandDescription = "Allows you to retrieve your personal data.";

    public const string Delete_CommandName = "delete";
    public const string Delete_CommandDescription = "Allows you to delete your personal data.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "rgpd" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de gérer vos données personnelles." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Get_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Get_CommandName },
                { DiscordLocale.en_GB, Get_CommandName },
                { DiscordLocale.fr, "recuperer" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Get_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Get_CommandDescription },
                { DiscordLocale.en_GB, Get_CommandDescription },
                { DiscordLocale.fr, "Permet de récupérer vos données personnelles." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Delete_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Delete_CommandName },
                { DiscordLocale.en_GB, Delete_CommandName },
                { DiscordLocale.fr, "supprimer" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Delete_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Delete_CommandDescription },
                { DiscordLocale.en_GB, Delete_CommandDescription },
                { DiscordLocale.fr, "Permet de supprimer vos données personnelles." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
