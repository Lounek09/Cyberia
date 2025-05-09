using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Crit;

public sealed class CritInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "crit";
    public const string CommandDescription = "Allows you to calculate your critical rate.";

    public const string BaseRate_ParameterName = "base_rate";
    public const string BaseRate_ParameterDescription = "Base critical hit rate of the spell or weapon.";

    public const string CriticalHitBonus_ParameterName = "critical_hit";
    public const string CriticalHitBonus_ParameterDescription = "Your number of critical hits.";

    public const string Agility_ParameterName = "agility";
    public const string Agility_ParameterDescription = "Your agility.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "crit" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de calculer votre taux de coup critique." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + BaseRate_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, BaseRate_ParameterName },
                { DiscordLocale.en_GB, BaseRate_ParameterName },
                { DiscordLocale.fr, "taux_de_base" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + BaseRate_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, BaseRate_ParameterDescription },
                { DiscordLocale.en_GB, BaseRate_ParameterDescription },
                { DiscordLocale.fr, "Taux de coup critique de base du sort ou de l'arme." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + CriticalHitBonus_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CriticalHitBonus_ParameterName },
                { DiscordLocale.en_GB, CriticalHitBonus_ParameterName },
                { DiscordLocale.fr, "coup_critique" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + CriticalHitBonus_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CriticalHitBonus_ParameterDescription },
                { DiscordLocale.en_GB, CriticalHitBonus_ParameterDescription },
                { DiscordLocale.fr, "Votre nombre de coups critiques." }
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

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
