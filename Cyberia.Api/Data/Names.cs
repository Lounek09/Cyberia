using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class TaxCollectorLastNameData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        [JsonConstructor]
        internal TaxCollectorLastNameData()
        {
            Name = string.Empty;
        }
    }

    public sealed class TaxCollectorFirstNameData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        [JsonConstructor]
        internal TaxCollectorFirstNameData()
        {
            Name = string.Empty;
        }
    }

    public sealed class TaxCollectorNamesData
    {
        private const string FILE_NAME = "names.json";

        [JsonPropertyName("NF.n")]
        public List<TaxCollectorLastNameData> TaxCollectorLastNames { get; init; }

        [JsonPropertyName("NF.f")]
        public List<TaxCollectorFirstNameData> TaxCollectorFirstNames { get; init; }

        [JsonConstructor]
        internal TaxCollectorNamesData()
        {
            TaxCollectorLastNames = [];
            TaxCollectorFirstNames = [];
        }

        internal static TaxCollectorNamesData Load()
        {
            return Datacenter.LoadDataFromFile<TaxCollectorNamesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public string GetRandomTaxCollectorName()
        {
            if (TaxCollectorLastNames.Count > 0 && TaxCollectorFirstNames.Count > 0)
            {
                int lastNameIndex = Random.Shared.Next(0, TaxCollectorLastNames.Count - 1);
                int firstNameIndex = Random.Shared.Next(0, TaxCollectorFirstNames.Count - 1);

                return (TaxCollectorFirstNames[firstNameIndex].Name + " " + TaxCollectorLastNames[lastNameIndex].Name).Replace("[wip] ", "");
            }

            return string.Empty;
        }
    }
}
