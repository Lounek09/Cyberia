using DSharpPlus.Entities;

namespace Salamandra.Bot.DsharpPlus
{
    public static class ExtendDiscordEmbedBuilder
    {
        public static DiscordEmbedBuilder AddFields(this DiscordEmbedBuilder embed, string name, IEnumerable<string> rows, bool inline = false)
        {
            string content = "";

            foreach (string row in rows)
            {
                if (row.Length > 1024)
                    throw new ArgumentException("One row exceed 1024 characters and embed field value cannot exceed this length.");

                if (content.Length + row.Length > 1024)
                {
                    embed.AddField(name, content[..^1], inline);
                    content = "";
                }

                content += $"{row}\n";
            }

            return embed.AddField(name, content[..^1], inline);
        }

        /*public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<AbstractEffect> effects, bool inline = false)
        {
            List<string> effectsParse = new();

            foreach (AbstractEffect effect in effects)
            {
                string emoji;
                if (effect is AddStateEffect addStateEffect)
                    emoji = Emojis.State(addStateEffect.StateId);
                else if (effect is RemoveStateEffect removeStateEffect)
                    emoji = Emojis.State(removeStateEffect.StateId);
                else
                    emoji = Emojis.Effect(effect.EffectId);

                string value = emoji + " " + effect.GetDescription();

                if (effect.Area.Symbol != 'P')
                    value += " - " + Emojis.Area(effect.Area.Symbol) + " " + effect.Area.GetDescription();

                effectsParse.Add(value);
            }

            return embed.AddFields(name, effectsParse, inline);
        }

        public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, string name, IEnumerable<AbstractQuestObjective> questObjectives, bool inline = false)
        {
            List<string> questObjectivesParse = new();

            foreach (AbstractQuestObjective questObjective in questObjectives)
                questObjectivesParse.Add(questObjective.GetDescription());

            return embed.AddFields(name, questObjectivesParse, inline);
        }*/
    }
}
