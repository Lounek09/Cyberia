using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.DsharpPlus
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

        public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool inline = false)
        {
            List<string> effectsParse = new();

            foreach (IEffect effect in effects)
            {
                string emoji;
                if (effect is AddStateEffect addStateEffect)
                    emoji = Emojis.State(addStateEffect.StateId);
                else if (effect is RemoveStateEffect removeStateEffect)
                    emoji = Emojis.State(removeStateEffect.StateId);
                else
                    emoji = Emojis.Effect(effect.EffectId);

                string effectParse = $"{emoji} {effect.GetDescription()}{(effect.Area.Size == EffectAreaManager.BaseArea.Size ? "" : $" - {Emojis.Area(effect.Area.Id)} {effect.Area.GetSize()}")}";
                if (!string.IsNullOrEmpty(effect.Criteria))
                    effectParse += " " + Formatter.InlineCode(Formatter.Strip(string.Join(' ', CriterionFactory.GetCriteriaParse(effect.Criteria))));

                effectsParse.Add(effectParse);
            }

            return embed.AddFields(name, effectsParse, inline);
        }

        public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IQuestObjective> questObjectives, bool inline = false)
        {
            List<string> questObjectivesParse = new();

            foreach (IQuestObjective questObjective in questObjectives)
                questObjectivesParse.Add(questObjective.GetDescription());

            return embed.AddFields(name, questObjectivesParse, inline);
        }
    }
}
