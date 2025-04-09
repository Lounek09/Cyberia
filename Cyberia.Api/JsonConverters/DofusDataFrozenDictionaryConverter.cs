using Cyberia.Api.Data;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="FrozenDictionary{TKey, TValue}"/> 
/// where values implement <see cref="IDofusData{TKey}"/>.
/// </summary>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array of objects that implement <see cref="IDofusData{TKey}"/></item>
///   <item>Converts these objects to a <see cref="FrozenDictionary{TKey, TValue}"/> keyed by their <see cref="IDofusData{TKey}.Id"/> property</item>
///   <item>If duplicate keys exist, only the first occurrence is preserved</item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Serializes the dictionary values as a JSON array</item>
/// </list>
/// </remarks>
/// <typeparam name="TKey">The type of key used in the dictionary, matching the <see cref="IDofusData{TKey}.Id"/> property type.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary, must implement <see cref="IDofusData{TKey}"/>.</typeparam>
public sealed class DofusDataFrozenDictionaryConverter<TKey, TValue> : JsonConverter<FrozenDictionary<TKey, TValue>>
    where TKey : notnull
    where TValue : IDofusData<TKey>
{
    public override FrozenDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        var values = JsonSerializer.Deserialize<TValue[]>(ref reader, options);
        if (values is null || values.Length == 0)
        {
            return FrozenDictionary<TKey, TValue>.Empty;
        }

        Dictionary<TKey, TValue> dictionary = new(values.Length);
        foreach (var value in values)
        {
            dictionary.TryAdd(value.Id, value);
        }

        return dictionary.ToFrozenDictionary();
    }

    public override void Write(Utf8JsonWriter writer, FrozenDictionary<TKey, TValue> values, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var value in values.Values)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        writer.WriteEndArray();
    }
}
