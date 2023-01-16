using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Collections;

namespace ScintillaNET;

/// <summary>
/// A multiple selection collection.
/// </summary>
public class SelectionCollection : SelectionCollectionBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>, IEnumerable<Selection>
{
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the <see cref="Selection" /> at the specified zero-based index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Selection" /> to get.</param>
    /// <returns>The <see cref="Selection" /> at the specified index.</returns>
    public override Selection this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Selection(scintilla, index);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionCollection" /> class.
    /// </summary>
    /// <param name="scintilla"></param>
    public SelectionCollection(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla) : base(scintilla)
    {
    }
}