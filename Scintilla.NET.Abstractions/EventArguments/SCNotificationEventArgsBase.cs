using static Scintilla.NET.Abstractions.ScintillaApiStructs;

namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// A base class for events for the Scintilla native control signals.
/// Notifications are sent (fired) from the Scintilla control to its container when an event has occurred that may interest the container.
/// Implements the <see cref="System.EventArgs" />
/// </summary>
/// <seealso cref="System.EventArgs" />
// ReSharper disable twice InconsistentNaming, part of the API
public abstract class SCNotificationEventArgsBase : EventArgs
{
    /// <summary>
    /// Gets the Scintilla notification data structure.
    /// </summary>
    /// <value>The Scintilla notification data structure.</value>
    public virtual SCNotification SCNotification { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SCNotificationEventArgsBase"/> class.
    /// </summary>
    /// <param name="scn">The Scintilla notification data structure.</param>
    protected SCNotificationEventArgsBase(SCNotification scn)
    {
        SCNotification = scn;
    }
}