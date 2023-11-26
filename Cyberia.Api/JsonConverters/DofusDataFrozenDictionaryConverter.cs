using Cyberia.Api.Data;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    internal sealed class DofusDataFrozenDictionaryConverter<TKey, TValue> : JsonConverter<FrozenDictionary<TKey, TValue>>
        where TKey : notnull
        where TValue : IDofusData<TKey>
    {
        public override FrozenDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TValue[]? values = JsonSerializer.Deserialize<TValue[]>(ref reader, options) ?? [];
            return values.GroupBy(x => x.Id).ToFrozenDictionary(x => x.Key, x => x.ElementAt(0));
        }

        public override void Write(Utf8JsonWriter writer, FrozenDictionary<TKey, TValue> values, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (KeyValuePair<TKey, TValue> pair in values)
            {
                JsonSerializer.Serialize(writer, pair.Value, options);
            }

            writer.WriteEndArray();
        }
    }
}
