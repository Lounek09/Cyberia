using Cyberia.Api.Data.Common;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="AccessoriesData"> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed <see cref="AccessoriesData"/> representation.<br />
/// - Deserializes the string into an <see cref="AccessoriesData"/> instance.
/// </remarks>
public sealed class AccessoriesDataConverter : JsonConverter<AccessoriesData>
{
    public override AccessoriesData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        const char separator = ',';

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        var value = reader.GetString().AsSpan();
        if (value.IsEmpty)
        {
            throw new JsonException("Expected a non-empty string.");
        }

        var count = value.Count(separator) + 1;
        Span<Range> ranges = stackalloc Range[count];

        value.Split(ranges, separator);

        return new AccessoriesData
        {
            WeaponItemId = int.Parse(value[ranges[0]], NumberStyles.HexNumber),
            HatItemId = int.Parse(value[ranges[1]], NumberStyles.HexNumber),
            CloakItemId = int.Parse(value[ranges[2]], NumberStyles.HexNumber),
            PetItemId = int.Parse(value[ranges[3]], NumberStyles.HexNumber),
            ShieldItemId = int.Parse(value[ranges[4]], NumberStyles.HexNumber),
        };
    }

    public override void Write(Utf8JsonWriter writer, AccessoriesData value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
