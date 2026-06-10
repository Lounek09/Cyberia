using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Escape;

public sealed class EscapeInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "escape";
    public const string CommandDescription = "Allows you to calculate your % to escape.";

    public const string Agility_ParameterName = "agility";
    public const string Agility_ParameterDescription = "Your agility.";

    public const string EnemyAgility_ParameterName = "enemy_agility";
    public const string EnemyAgility_ParameterDescription = "Enemy's agility at your contact.";

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

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de calculer son % de fuite." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Agility_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Agility_ParameterName },
                { DiscordLocale.en_GB, Agility_ParameterName },
                { DiscordLocale.fr, "agilite" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Agility_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Agility_ParameterDescription },
                { DiscordLocale.en_GB, Agility_ParameterDescription },
                { DiscordLocale.fr, "Votre agilité." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + EnemyAgility_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, EnemyAgility_ParameterName },
                { DiscordLocale.en_GB, EnemyAgility_ParameterName },
                { DiscordLocale.fr, "agilite_ennemi" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + EnemyAgility_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, EnemyAgility_ParameterDescription },
                { DiscordLocale.en_GB, EnemyAgility_ParameterDescription },
                { DiscordLocale.fr, "L'agilité de l'ennemi à votre contact." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
