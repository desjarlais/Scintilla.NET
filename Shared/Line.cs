using Scintilla.NET.Abstractions;
using System.Drawing;
using Scintilla.NET.Abstractions.Collections;

namespace ScintillaNET;

/// <summary>
/// Represents a line of text in a <see cref="Scintilla" /> control.
/// </summary>
public class Line : LineBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Line" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this line.</param>
    /// <param name="index">The index of this line within the <see cref="LineCollection" /> that created it.</param>
    public Line(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, int index) : base(scintilla, index)
    {
    }

    #endregion Constructors
}