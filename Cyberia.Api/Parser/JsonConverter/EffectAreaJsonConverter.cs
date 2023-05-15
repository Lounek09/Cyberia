using Cyberia.Api.Managers;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class EffectAreaJsonConverter : JsonConverter<Area>
    {
        public override Area Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            return EffectAreaManager.GetArea(element.GetString() ?? "");
        }

        public override void Write(Utf8JsonWriter writer, Area values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
