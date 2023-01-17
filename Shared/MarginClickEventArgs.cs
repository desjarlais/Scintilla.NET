using System.Drawing;
using System.Windows.Forms;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.MarginClick" /> event.
/// </summary>
public class MarginClickEventArgs : MarginClickEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color, Keys>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarginClickEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="modifiers">The modifier keys that where held down at the time of the margin click.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where the line adjacent to the clicked margin starts.</param>
    /// <param name="margin">The zero-based index of the clicked margin.</param>
    public MarginClickEventArgs(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, Keys modifiers, int bytePosition, int margin) : base(scintilla, modifiers, bytePosition, margin)
    {
    }
}