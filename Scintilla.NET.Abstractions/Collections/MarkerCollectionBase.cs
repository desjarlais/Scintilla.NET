using System.Collections;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// An immutable collection of markers in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class MarkerCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> : IEnumerable<TMarker>
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
    protected readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An object for enumerating all <see cref="MarkerBase{TBitmap, TColor, TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs}" /> objects within the <see cref="MarkerCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker}" />.</returns>
    public IEnumerator<TMarker> GetEnumerator()
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
    /// Gets the number of markers in the <see cref="MarkerCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker}" />.
    /// </summary>
    /// <returns>This property always returns 32.</returns>
    public int Count => (MARKER_MAX + 1);

    /// <summary>
    /// Gets a <typeparamref name="TMarker"/> object at the specified index.
    /// </summary>
    /// <param name="index">The marker index.</param>
    /// <returns>An object representing the marker at the specified <paramref name="index" />.</returns>
    /// <remarks>Markers 25 through 31 are used by Scintilla for folding.</remarks>
    protected abstract TMarker this[int index] { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkerCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    protected MarkerCollectionBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla)
    {
        this.scintilla = scintilla;
    }
}