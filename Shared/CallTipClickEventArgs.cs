using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Enumerations;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.CallTipClick" /> event.
/// </summary>
public class CallTipClickEventArgs: CallTipClickEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DwellEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// /// <param name="callTipClickType">Type of the call tip click.</param>
    public CallTipClickEventArgs(
        IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection,
            SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap,
            Color> scintilla, CallTipClickType callTipClickType) : base(scintilla, callTipClickType)
    {
    }
}