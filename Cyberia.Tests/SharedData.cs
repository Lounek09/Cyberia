namespace Cyberia.Tests;

public static class SharedData
{
    public const string DATA_DIRECTORY = "tests";

    public static readonly HttpRetryPolicy HttpRetryPolicy = new(1, TimeSpan.FromMilliseconds(10));
}
