using Cyberia.Langzilla.Enums;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla.JsonConverters;

internal class LangDataCollectionConverter : JsonConverter<LangDataCollection>
{
    public override LangDataCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        LangDataCollection langDataCollection = [];

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                break;
            }

            var propertyName = reader.GetString();

            reader.Read();

            switch (propertyName)
            {
                case "type":
                    langDataCollection.Type = JsonSerializer.Deserialize<LangType>(ref reader, options);
                    break;
                case "language":
                    langDataCollection.Language = JsonSerializer.Deserialize<LangLanguage>(ref reader, options);
                    break;
                case "lastChange":
                    langDataCollection.LastChange = JsonSerializer.Deserialize<long>(ref reader, options);
                    break;
                case "langs":
                    langDataCollection.ItemsCore = JsonSerializer.Deserialize<List<LangData>>(ref reader, options) ?? [];
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        return langDataCollection;
    }

    public override void Write(Utf8JsonWriter writer, LangDataCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteNumber("type", (int)value.Type);
        writer.WriteNumber("language", (int)value.Language);
        writer.WriteNumber("lastChange", value.LastChange);
        writer.WritePropertyName("langs");
        JsonSerializer.Serialize(writer, value.Items, options);

        writer.WriteEndObject();
    }
}
