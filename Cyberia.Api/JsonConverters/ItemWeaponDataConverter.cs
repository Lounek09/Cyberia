using Cyberia.Api.Data;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class ItemWeaponDataConverter : JsonConverter<ItemWeaponData>
    {
        public override ItemWeaponData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement[] elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

            return new ItemWeaponData
            {
                CriticalBonus = elements[0].GetInt32(),
                ActionPointCost = elements[1].GetInt32(),
                MinRange = elements[2].GetInt32(),
                MaxRange = elements[3].GetInt32(),
                CriticalHitRate = elements[4].GetInt32(),
                CriticalFailureRate = elements[5].GetInt32(),
                LineOnly = elements[6].GetBoolean(),
                LineOfSight = elements[7].GetBoolean()
            };
        }

        public override void Write(Utf8JsonWriter writer, ItemWeaponData value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            writer.WriteNumberValue(value.CriticalBonus);
            writer.WriteNumberValue(value.ActionPointCost);
            writer.WriteNumberValue(value.MinRange);
            writer.WriteNumberValue(value.MaxRange);
            writer.WriteNumberValue(value.CriticalHitRate);
            writer.WriteNumberValue(value.CriticalFailureRate);
            writer.WriteBooleanValue(value.LineOnly);
            writer.WriteBooleanValue(value.LineOfSight);

            writer.WriteEndArray();
        }
    }
}
