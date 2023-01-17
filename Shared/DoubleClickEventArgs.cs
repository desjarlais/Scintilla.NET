using System.Drawing;
using System.Windows.Forms;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.DoubleClick" /> event.
/// </summary>
public class DoubleClickEventArgs : DoubleClickEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color, Keys>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleClickEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="modifiers">The modifier keys that where held down at the time of the double click.</param>
    /// <param name="bytePosition">The zero-based byte position of the double clicked text.</param>
    /// <param name="line">The zero-based line index of the double clicked text.</param>
    public DoubleClickEventArgs(
        IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection,
            SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap,
            Color> scintilla, Keys modifiers, int bytePosition, int line) : base(scintilla, modifiers, bytePosition,
        line)
    {
    }
}