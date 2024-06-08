namespace Cyberia.Api;

public readonly record struct Description(string Value, params string[] Parameters)
{
    public static readonly Description Empty = new();

    public override string ToString()
    {
        return Translation.Format(Value, Parameters);
    }

    public string ToString(Func<string, string> decorator)
    {
        return Translation.Format(Value, Array.ConvertAll(Parameters, x => string.IsNullOrEmpty(x) ? x : decorator(x)));
    }

    public static implicit operator string(Description description)
    {
        return description.ToString();
    }
}
