using System.Collections;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// An immutable collection of style definitions in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class StyleCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> : IEnumerable<TStyle>
    where TMarkers : MarkerCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TStyles : StyleCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TIndicators : IEnumerable
    where TLines : LineCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TMargins : IEnumerable
    where TSelections : IEnumerable
    where TEventArgs : EventArgs
    where TMarker: MarkerBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TStyle : StyleBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
{
    public readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An object that contains all <see cref="StyleBase" /> objects within the <see cref="StyleCollectionBase" />.</returns>
    public virtual IEnumerator<TStyle> GetEnumerator()
    {
        int count = Count;
        for (int i = 0; i < count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the number of styles.
    /// </summary>
    /// <returns>The number of styles in the <see cref="StyleCollectionBase" />.</returns>
    public virtual int Count => (STYLE_MAX + 1);

    /// <summary>
    /// Gets a <typeparamref name="TStyle"/> object at the specified index.
    /// </summary>
    /// <param name="index">The style definition index.</param>
    /// <returns>An object representing the style definition at the specified <paramref name="index" />.</returns>
    /// <remarks>Styles 32 through 39 have special significance.</remarks>
    public abstract TStyle this[int index] { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleCollectionBase" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="IScintillaApi" /> control that created this collection.</param>
    public StyleCollectionBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla)
    {
        this.scintilla = scintilla;
    }
}