using Cyberia.Api.Managers;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class EffectAreaConverter : JsonConverter<EffectArea>
    {
        public override EffectArea Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return EffectAreaManager.GetEffectArea(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, EffectArea values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
