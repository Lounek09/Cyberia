using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus;

/// <summary>
/// Provides methods for watching updates of Cytrus.
/// </summary>
public static class CytrusWatcher
{
    internal const string OUTPUT_PATH = "cytrus";
    internal const string CYTRUS_FILE_NAME = "cytrus.json";
    internal static readonly string CYTRUS_PATH = Path.Join(OUTPUT_PATH, CYTRUS_FILE_NAME);
    internal static readonly string OLD_CYTRUS_PATH = Path.Join(OUTPUT_PATH, $"old_{CYTRUS_FILE_NAME}");
    internal const string BASE_URL = "https://cytrus.cdn.ankama.com";

    /// <summary>
    /// The current Cytrus data.
    /// </summary>
    public static Cytrus Cytrus { get; internal set; } = default!;

    /// <summary>
    /// The old Cytrus data.
    /// </summary>
    public static Cytrus OldCytrus { get; internal set; } = default!;


    internal static HttpClient HttpClient { get; set; } = default!;
    internal static HttpRetryPolicy HttpRetryPolicy { get; set; } = default!;


#pragma warning disable IDE0052 // Remove unread private members
    private static Timer? _timer;
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// Initializes the CytrusWatcher.
    /// </summary>
    public static void Initialize()
    {
        Directory.CreateDirectory(OUTPUT_PATH);

        Cytrus = Cytrus.LoadFromFile(CYTRUS_PATH);
        OldCytrus = Cytrus.LoadFromFile(OLD_CYTRUS_PATH);

        HttpClient = new()
        {
            BaseAddress = new Uri(BASE_URL)
        };
        HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Event that is triggered when a new Cytrus is detected.
    /// </summary>
    public static event EventHandler<NewCytrusDetectedEventArgs>? NewCytrusDetected;

    /// <summary>
    /// Starts watching for updates of Cytrus.
    /// </summary>
    /// <param name="dueTime">The amount of time to delay before the first check.</param>
    /// <param name="interval">The interval between checks.</param>
    public static void Watch(TimeSpan dueTime, TimeSpan interval)
    {
        _timer = new(async _ => await CheckAsync(), null, dueTime, interval);
    }

    /// <summary>
    /// Asynchronously checks for updates of Cytrus.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Sends a GET request to the Cytrus file URL.
    /// 2. If the request is successful, it reads the response content as a string.
    /// 3. If a previous Cytrus file exists, it reads the file content as a string, otherwise it uses an empty JSON object string.
    /// 4. Calculates the difference between the new and old Cytrus data.
    /// 5. If there is no difference, it returns.
    /// 6. Replaces the old Cytrus data by the new Cytrus data.
    /// 7. Loads the new Cytrus data from the response content.
    /// 8. Triggers the <see cref="NewCytrusDetected"/> event.
    /// </remarks>
    public static async Task CheckAsync()
    {
        string json;
        try
        {
            using var response = await HttpRetryPolicy.ExecuteAsync(() => HttpClient.GetAsync(CYTRUS_FILE_NAME));
            response.EnsureSuccessStatusCode();

            json = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {CytrusUrl}", $"{BASE_URL}/{CYTRUS_FILE_NAME}");
            return;
        }

        var modelJson = File.Exists(CYTRUS_PATH) ? File.ReadAllText(CYTRUS_PATH) : "{}";

        var diff = Json.Diff(json, modelJson);
        if (string.IsNullOrEmpty(diff))
        {
            return;
        }

        OldCytrus = Cytrus;
        Cytrus = Cytrus.Load(json);

        Log.Information("Cytrus update detected :\n{CytrusDiff}", diff);
        NewCytrusDetected?.Invoke(null, new NewCytrusDetectedEventArgs(Cytrus, OldCytrus, diff));

        if (File.Exists(CYTRUS_PATH))
        {
            File.Move(CYTRUS_PATH, OLD_CYTRUS_PATH, true);
        }
        File.WriteAllText(CYTRUS_PATH, json);
    }
}
