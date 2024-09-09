using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Other.Help;

public sealed class HelpInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "help";
    public const string CommandDescription = "List the bot commands.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, CommandName }
            };
        }

        if (fullSymbolName.EndsWith(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Liste les commandes du bot." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
