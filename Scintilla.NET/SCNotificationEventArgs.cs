using System;

namespace ScintillaNET;

// For internal use only
internal sealed class SCNotificationEventArgs : EventArgs
{
    public SciApi.SCNotification SCNotification { get; private set; }

    public SCNotificationEventArgs(SciApi.SCNotification scn)
    {
        SCNotification = scn;
    }
}
