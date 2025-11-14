using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordEmbedBuilder"/>.
/// </summary>
public static class DiscordEmbedBuilderExtensions
{
    extension(DiscordEmbedBuilder embed)
    {
        /// <summary>
        /// Adds an empty field to the embed.
        /// </summary>
        /// <param name="inline">Whether the field is to be inline or not.</param>
        /// <returns>The embed builder.</returns>
        public DiscordEmbedBuilder AddEmptyField(bool inline = false)
        {
            return embed.AddField(Constant.ZeroWidthSpace, Constant.ZeroWidthSpace, inline);
        }

        /// <summary>
        /// Adds fields to the embed splitting the content if it exceeds 1024 characters.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="rows">The content of the field.</param>
        /// <param name="inline">Whether the field is to be inline or not.</param>
        /// <returns>The embed builder.</returns>
        /// <exception cref="ArgumentException">Thrown if one row exceeds 1024 characters.</exception>
        public DiscordEmbedBuilder AddFields(string name, IEnumerable<string> rows, bool inline = false)
        {
            StringBuilder builder = new();

            foreach (var row in rows)
            {
                if (row.Length > Constant.MaxEmbedFieldContentSize)
                {
                    throw new ArgumentException($"One row exceeds {Constant.MaxEmbedFieldContentSize} characters and embed field value cannot exceed this length.");
                }

                if (builder.Length + row.Length > 1024)
                {
                    embed.AddField(name, builder.ToString(), inline);
                    builder.Clear();
                }

                builder.Append(row);
                builder.Append('\n');
            }

            return embed.AddField(name, builder.ToString(), inline);
        }
    }
}
