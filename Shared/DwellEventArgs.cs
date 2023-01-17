using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.DwellStart" /> and <see cref="Scintilla.DwellEnd" /> events.
/// </summary>
public class DwellEventArgs : DwellEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DwellEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where the mouse pointer was lingering.</param>
    /// <param name="x">The x-coordinate of the mouse pointer relative to the <see cref="Scintilla" /> control.</param>
    /// <param name="y">The y-coordinate of the mouse pointer relative to the <see cref="Scintilla" /> control.</param>
    public DwellEventArgs(
        IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection,
            SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap,
            Color> scintilla, int bytePosition, int x, int y) : base(scintilla, bytePosition, x, y)
    {
    }
}