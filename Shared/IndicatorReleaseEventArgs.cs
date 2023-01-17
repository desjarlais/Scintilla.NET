using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.IndicatorRelease" /> event.
/// </summary>
public class IndicatorReleaseEventArgs : IndicatorReleaseEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IndicatorReleaseEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position of the clicked text.</param>
    public IndicatorReleaseEventArgs(
        IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection,
            SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap,
            Color> scintilla, int bytePosition) : base(scintilla, bytePosition)
    {
    }
}