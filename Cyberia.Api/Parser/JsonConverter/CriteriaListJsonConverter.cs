using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class CriteriaListJsonConverter : JsonConverter<List<ICriteriaElement>>
    {
        public override List<ICriteriaElement> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            return CriterionFactory.GetCriteria(element.GetString() ?? "").ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<ICriteriaElement> values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
