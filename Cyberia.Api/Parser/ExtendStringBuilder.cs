using System.Text;
using System.Text.Json;

namespace Cyberia.Api.Parser;

public static class ExtendStringBuilder
{
    public static StringBuilder AppendJsonProperty(this StringBuilder builder, ReadOnlySpan<char> name, ReadOnlySpan<char> value, JsonValueKind kind)
    {
        builder.Append('"');
        builder.Append(name);
        builder.Append("\":");

        if (kind == JsonValueKind.String)
        {
            builder.Append('"');
        }

        builder.Append(value);

        if (kind == JsonValueKind.String)
        {
            builder.Append('"');
        }

        return builder;
    }
}
