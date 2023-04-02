namespace Cyberia.Scripts
{
    public readonly struct DatabaseProperty
    {
        public LangObject LangProperty { get; init; }
        public string Value { get; init; }

        public DatabaseProperty(LangObject langProperty, string value)
        {
            LangProperty = langProperty;
            Value = value;
        }
    }
}
