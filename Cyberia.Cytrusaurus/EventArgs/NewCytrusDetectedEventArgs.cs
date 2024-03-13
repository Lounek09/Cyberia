using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus.EventArgs;

public sealed class NewCytrusDetectedEventArgs
    : System.EventArgs
{
    public CytrusData CytrusData { get; init; }
    public CytrusData OldCytrusData { get; init; }
    public string Diff { get; init; }

    public NewCytrusDetectedEventArgs(CytrusData cytrusData, CytrusData oldCytrusData, string diff)
    {
        CytrusData = cytrusData;
        OldCytrusData = oldCytrusData;
        Diff = diff;
    }
}
