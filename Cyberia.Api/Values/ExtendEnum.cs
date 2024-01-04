namespace Cyberia.Api.Values;

public static class ExtendEnum
{
    public static string GetDescription<T>(this T value)
        where T : Enum
    {
        var description = Resources.ResourceManager.GetString($"{typeof(T).Name}.{value}");
        if (description is not null)
        {
            return description;
        }

        return PatternDecoder.Description(Resources.Unknown_Data, value);
    }
}
