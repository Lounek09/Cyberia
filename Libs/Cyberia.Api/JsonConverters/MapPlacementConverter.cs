using Cyberia.Api.Utils;

using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing map placement cells.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed map placement representation.<br />
/// - Parses this string to extract the placement cells using <see cref="PatternDecoder.DecodeMapPlacement"/>.<br />
/// </remarks>
public sealed class MapPlacementConverter : JsonConverter<IReadOnlyList<int>>
{
    public override IReadOnlyList<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        try
        {
            return PatternDecoder.DecodeMapPlacement(reader.GetString() ?? string.Empty).ToList().AsReadOnly();
        }
        catch (ArgumentException ex)
        {
            Log.Error(ex, "Failed to decode map placement.");
        }

        return ReadOnlyCollection<int>.Empty;
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<int> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
