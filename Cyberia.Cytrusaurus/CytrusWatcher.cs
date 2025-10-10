using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Database.Models;
using Cyberia.Database.Repositories;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Cyberia.Cytrusaurus;

/// <summary>
/// Provides methods for watching updates of Cytrus.
/// </summary>
public interface ICytrusWatcher
{
    /// <summary>
    /// Gets the current Cytrus data.
    /// </summary>
    Cytrus Cytrus { get; }

    /// <summary>
    /// Gets the old Cytrus data.
    /// </summary>
    Cytrus OldCytrus { get; }

    /// <summary>
    /// Gets when the Cytrus data was last modified.
    /// </summary>
    DateTime LastModified { get; }

    /// <summary>
    /// Initializes the data of the <see cref="ICytrusWatcher"/> instance.
    /// </summary>
    /// <remarks>
    /// This need to be called before everything else.
    /// </remarks>
    Task InitializeAsync();

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
    /// Delegate for the <see cref="NewCytrusFileDetected"/> event.
    /// </summary>
    delegate ValueTask NewCytrusFileDetectedEventHandler(ICytrusWatcher sender, NewCytrusFileDetectedEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when a new Cytrus file is detected.
    /// </summary>
    event NewCytrusFileDetectedEventHandler? NewCytrusFileDetected;

    /// <summary>
    /// Delegate for the <see cref="CytrusErrored"/> event.
    /// </summary>
    delegate ValueTask CytrusErroredEventHandler(ICytrusWatcher sender, CytrusErroredEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when an error occurs while checking for Cytrus updates.
    /// </summary>
    event CytrusErroredEventHandler? CytrusErrored;
}

public sealed class CytrusWatcher : ICytrusWatcher
{
    /// <summary>
    /// The root output directory.
    /// </summary>
    public const string OutputPath = "cytrus";

    /// <summary>
    /// The name of the Cytrus file.
    /// </summary>
    public const string CytrusFileName = "cytrus.json";

    /// <summary>
    /// The base URL of Cytrus.
    /// </summary>
    // TODO: Make it configurable
    public const string BaseUrl = "https://cytrus.cdn.ankama.com";

    private const string c_onlineMonitoredFileId = "cytrus";

    public static readonly string CytrusPath = Path.Join(OutputPath, CytrusFileName);
    public static readonly string OldCytrusPath = Path.Join(OutputPath, $"old_{CytrusFileName}");
    public static readonly string CytrusUrl = $"{BaseUrl}/{CytrusFileName}";

    public Cytrus Cytrus { get; private set; } = default!;

    public Cytrus OldCytrus { get; private set; } = default!;

    public DateTime LastModified { get; private set; }

    private readonly OnlineMonitoredFileRepository _onlineMonitoredFileRepository;
    private readonly HttpClient _httpClient;
    private readonly HttpRetryPolicy _httpRetryPolicy;

    private Timer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusWatcher"/> class.
    /// </summary>
    /// <param name="onlineMonitoredFileRepository">The repository for online monitored files.</param>
    public CytrusWatcher(OnlineMonitoredFileRepository onlineMonitoredFileRepository)
    {
        Directory.CreateDirectory(OutputPath);

        _onlineMonitoredFileRepository = onlineMonitoredFileRepository;
        _httpClient = new()
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    public async Task InitializeAsync()
    {
        Cytrus = await LoadCytrusAsync(CytrusPath) ?? new();
        OldCytrus = await LoadCytrusAsync(OldCytrusPath) ?? new();
        LastModified = await _onlineMonitoredFileRepository.GetLastModifiedByIdAsync(c_onlineMonitoredFileId);
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

            var lastModified = response.Content.Headers.LastModified?.UtcDateTime;
            if (lastModified is null || lastModified.Value <= LastModified)
            {
                return;
            }

            LastModified = lastModified.Value;
            await _onlineMonitoredFileRepository.UpsertAsync(new OnlineMonitoredFile
            {
                Id = c_onlineMonitoredFileId,
                LastModified = LastModified
            });

            json = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending a GET request to {CytrusUrl}", $"{BaseUrl}/{CytrusFileName}");
            await OnCytrusErroredAsync(new CytrusErroredEventArgs(e, "An error occurred while sending a GET request to the Cytrus URL, see the logs for more details."));

            return;
        }

        var modelJson = File.Exists(CytrusPath) ? File.ReadAllText(CytrusPath) : "{}";

        var diff = Json.Diff(json, modelJson);
        if (string.IsNullOrEmpty(diff))
        {
            return;
        }

        var cytrus = DeserializeCytrus(json);
        if (cytrus is null)
        {
            await OnCytrusErroredAsync(new CytrusErroredEventArgs("Failed to deserialize the Cytrus data from the response content, see the logs for more details."));

            return;
        }

        OldCytrus = Cytrus;
        Cytrus = cytrus;

        if (File.Exists(CytrusPath))
        {
            File.Move(CytrusPath, OldCytrusPath, true);
        }
        File.WriteAllText(CytrusPath, json);

        Log.Information("Cytrus update detected :\n{CytrusDiff}", diff);
        await OnNewCytrusFileDetectedAsync(new NewCytrusFileDetectedEventArgs(Cytrus, OldCytrus, diff));
    }

    /// <summary>
    /// Loads the Cytrus data from the specified path.
    /// </summary>
    /// <param name="path">The path to the Cytrus file.</param>
    /// <returns>The loaded Cytrus data, or <see langword="null"/> if the file does not exist.</returns>
    private static async Task<Cytrus?> LoadCytrusAsync(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        using var stream = File.OpenRead(path);
        return await DeserializeCytrusAsync(stream);
    }

    /// <summary>
    /// Deserializes the Cytrus data from the specified stream.
    /// </summary>
    /// <param name="stream">The stream containing the Cytrus data.</param>
    /// <returns>The deserialized Cytrus data, or <see langword="null"/> if the deserialization fails.</returns>
    private static async Task<Cytrus?> DeserializeCytrusAsync(Stream stream)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<Cytrus>(stream);
        }
        catch (JsonException e)
        {
            Log.Error(e, "Failed to deserialize Cytrus from stream");
        }

        return null;
    }

    /// <summary>
    /// Deserializes the Cytrus data from the specified JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing the Cytrus data.</param>
    /// <returns>The deserialized Cytrus data, or <see langword="null"/> if the deserialization fails.</returns>
    private static Cytrus? DeserializeCytrus([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        try
        {
            return JsonSerializer.Deserialize<Cytrus>(json);
        }
        catch (JsonException e)
        {
            Log.Error(e, "Failed to deserialize Cytrus from JSON string");
        }

        return null;
    }

    #region Events

    public event ICytrusWatcher.NewCytrusFileDetectedEventHandler? NewCytrusFileDetected;

    /// <summary>
    /// Triggers the <see cref="NewCytrusFileDetected"/> event.
    /// </summary>
    internal async ValueTask OnNewCytrusFileDetectedAsync(NewCytrusFileDetectedEventArgs eventArgs)
    {
        var handler = NewCytrusFileDetected;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    public event ICytrusWatcher.CytrusErroredEventHandler? CytrusErrored;

    /// <summary>
    /// Triggers the <see cref="CytrusErrored"/> event.
    /// </summary>
    internal async ValueTask OnCytrusErroredAsync(CytrusErroredEventArgs eventArgs)
    {
        var handler = CytrusErrored;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    #endregion
}
