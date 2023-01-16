using Scintilla.NET.Abstractions;
using System.Drawing;
using Scintilla.NET.Abstractions.Collections;
using Scintilla.NET.Abstractions.UtilityClasses;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace ScintillaNET;

/// <summary>
/// An immutable collection of lines of text in a <see cref="Scintilla" /> control.
/// </summary>
public class LineCollection : LineCollectionBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    #region Methods
    
    public override void scintilla_SCNotification(object sender, SCNotificationEventArgs e)
    {
        var scn = e.SCNotification;
        switch (scn.nmhdr.code)
        {
            case SCN_MODIFIED:
                ScnModified(scn);
                break;
        }
    }
    #endregion Methods

    #region Properties
    
    /// <summary>
    /// Gets the <see cref="Line" /> at the specified zero-based index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Line" /> to get.</param>
    /// <returns>The <see cref="Line" /> at the specified index.</returns>
    public override Line this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Line(scintilla, index);
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LineCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    public LineCollection(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla) : base(scintilla)
    {
        this.scintilla.SCNotification += scintilla_SCNotification;

        perLineData = new GapBuffer<PerLine>
        {
            new() { Start = 0 },
            new() { Start = 0 }, // Terminal
        };
    }

    #endregion Constructors
}