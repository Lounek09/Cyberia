using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="Criterion"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed <see cref="Criterion"/> representation.<br />
/// - Parses this string into a structured <see cref="Criterion"/> using <see cref="CriterionFactory.Create"/>.
/// </remarks>
public sealed class CriterionConverter : JsonConverter<Criterion>
{
    public override Criterion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        return CriterionFactory.Create(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, Criterion values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
