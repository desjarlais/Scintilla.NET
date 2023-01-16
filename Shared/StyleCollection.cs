using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Collections;

namespace ScintillaNET;

/// <summary>
/// An immutable collection of style definitions in a <see cref="Scintilla" /> control.
/// </summary>
public class StyleCollection : StyleCollectionBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{

    /// <summary>
    /// Gets a <see cref="Style" /> object at the specified index.
    /// </summary>
    /// <param name="index">The style definition index.</param>
    /// <returns>An object representing the style definition at the specified <paramref name="index" />.</returns>
    /// <remarks>Styles 32 through 39 have special significance.</remarks>
    public override Style this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Style(scintilla, index);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="IScintillaApi" /> control that created this collection.</param>
    public StyleCollection(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla) : base(scintilla)
    {
    }
}