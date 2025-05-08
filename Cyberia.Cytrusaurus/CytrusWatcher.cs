using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus;

public interface ICytrusWatcher
{
    /// <summary>
    /// The current Cytrus data.
    /// </summary>
    Cytrus Cytrus { get; }

    /// <summary>
    /// The old Cytrus data.
    /// </summary>
    Cytrus OldCytrus { get; }

    /// <summary>
    /// Starts watching for updates of Cytrus.
    /// </summary>
    /// <param name="dueTime">The amount of time to delay before the first check.</param>
    /// <param name="interval">The interval between checks.</param>
    void Watch(TimeSpan dueTime, TimeSpan interval);

    /// <summary>
    /// Asynchronously checks for updates of Cytrus.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    ///     <item>Sends a GET request to the Cytrus file URL.</item>
    ///     <item>If the request is successful, it reads the response content as a string.</item>
    ///     <item>If a previous Cytrus file exists, it reads the file content as a string, otherwise it uses an empty JSON object string.</item>
    ///     <item>Calculates the difference between the new and old Cytrus data.</item>
    ///     <item>If there is no difference, it returns.</item>
    ///     <item>Replaces the old Cytrus data by the new Cytrus data.</item>
    ///     <item>Loads the new Cytrus data from the response content.</item>
    ///     <item>Triggers the <see cref="NewCytrusFileDetected"/> event with the difference.</item>
    /// </list>
    /// </remarks>
    Task CheckAsync();

    /// <summary>
    /// Delegate for the NewCytrusFileDetected event.
    /// </summary>
    delegate ValueTask NewCytrusFileDetectedEventHandler(ICytrusWatcher sender, NewCytrusFileDetectedEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when a new Cytrus file is detected.
    /// </summary>
    event NewCytrusFileDetectedEventHandler? NewCytrusFileDetected;
}

/// <summary>
/// Provides methods for watching updates of Cytrus.
/// </summary>
public sealed class CytrusWatcher : ICytrusWatcher
{
    public const string OutputPath = "cytrus";
    public const string CytrusFileName = "cytrus.json";
    public const string BaseUrl = "https://cytrus.cdn.ankama.com";

    public static readonly string CytrusPath = Path.Join(OutputPath, CytrusFileName);
    public static readonly string OldCytrusPath = Path.Join(OutputPath, $"old_{CytrusFileName}");

    public Cytrus Cytrus { get; internal set; }

    public Cytrus OldCytrus { get; internal set; }

    private readonly HttpClient _httpClient;
    private readonly HttpRetryPolicy _httpRetryPolicy;

#pragma warning disable IDE0052 // Remove unread private members
    private Timer? _timer;
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusWatcher"/> class.
    /// </summary>
    public CytrusWatcher()
    {
        Directory.CreateDirectory(OutputPath);

        Cytrus = Cytrus.LoadFromFile(CytrusPath);
        OldCytrus = Cytrus.LoadFromFile(OldCytrusPath);

        _httpClient = new()
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    public void Watch(TimeSpan dueTime, TimeSpan interval)
    {
        _timer = new(async _ => await CheckAsync(), null, dueTime, interval);
    }

    public async Task CheckAsync()
    {
        string json;
        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(CytrusFileName));
            response.EnsureSuccessStatusCode();

            json = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {CytrusUrl}", $"{BaseUrl}/{CytrusFileName}");
            return;
        }

        var modelJson = File.Exists(CytrusPath) ? File.ReadAllText(CytrusPath) : "{}";

        var diff = Json.Diff(json, modelJson);
        if (string.IsNullOrEmpty(diff))
        {
            return;
        }

        OldCytrus = Cytrus;
        Cytrus = Cytrus.Load(json);

        if (File.Exists(CytrusPath))
        {
            File.Move(CytrusPath, OldCytrusPath, true);
        }
        File.WriteAllText(CytrusPath, json);

        Log.Information("Cytrus update detected :\n{CytrusDiff}", diff);
        await OnNewCytrusFileDetected(new NewCytrusFileDetectedEventArgs(Cytrus, OldCytrus, diff));
    }

    #region Events

    public event ICytrusWatcher.NewCytrusFileDetectedEventHandler? NewCytrusFileDetected;

    /// <summary>
    /// Triggers the NewCytrusFileDetected event.
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    internal async ValueTask OnNewCytrusFileDetected(NewCytrusFileDetectedEventArgs eventArgs)
    {
        var handler = NewCytrusFileDetected;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    #endregion
}
