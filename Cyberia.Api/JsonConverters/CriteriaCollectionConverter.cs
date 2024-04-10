using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class CriteriaReadOnlyCollectionConverter
    : JsonConverter<CriteriaReadOnlyCollection>
{
    public override CriteriaReadOnlyCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return CriterionFactory.CreateMany(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, CriteriaReadOnlyCollection values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
