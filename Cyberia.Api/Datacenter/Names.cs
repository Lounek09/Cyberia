using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class TaxCollectorLastName
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public TaxCollectorLastName()
        {
            Name = string.Empty;
        }
    }

    public sealed class TaxCollectorFirstName
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public TaxCollectorFirstName()
        {
            Name = string.Empty;
        }
    }

    public sealed class TaxCollectorNamesData
    {
        private const string FILE_NAME = "names.json";

        [JsonPropertyName("NF.n")]
        public List<TaxCollectorLastName> TaxCollectorLastNames { get; init; }
        
        [JsonPropertyName("NF.f")]
        public List<TaxCollectorFirstName> TaxCollectorFirstNames { get; init; }

        public TaxCollectorNamesData()
        {
            TaxCollectorLastNames = new();
            TaxCollectorFirstNames = new();
        }

        internal static TaxCollectorNamesData Build()
        {
            return Json.LoadFromFile<TaxCollectorNamesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public string GetRandomTaxCollectorName()
        {
            if (TaxCollectorLastNames.Count > 0 && TaxCollectorFirstNames.Count > 0)
            {
                int lastNameIndex = Random.Shared.Next(0, TaxCollectorLastNames.Count - 1);
                int firstNameIndex = Random.Shared.Next(0, TaxCollectorFirstNames.Count - 1);

                return (TaxCollectorFirstNames[firstNameIndex].Name + " " + TaxCollectorLastNames[lastNameIndex].Name).Replace("[wip] ", "");
            }

            return "";
        }
    }
}
