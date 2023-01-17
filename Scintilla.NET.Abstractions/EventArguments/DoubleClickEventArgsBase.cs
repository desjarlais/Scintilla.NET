using System.Collections;
using Scintilla.NET.Abstractions.Collections;

namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the <see cref="Scintilla.DoubleClick" /> event.
/// </summary>
public abstract class DoubleClickEventArgsBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor, TKeys> 
    : ScintillaEventArgs<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TMarkers : MarkerCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TStyles : StyleCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TIndicators :IndicatorCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TLines : LineCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TMargins : MarginCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TSelections : SelectionCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TEventArgs : EventArgs
    where TMarker: MarkerBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TStyle : StyleBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TIndicator : IndicatorBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TLine : LineBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TMargin : MarginBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TSelection : SelectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TBitmap: class
    where TColor: struct
    where TKeys: Enum
{
    private readonly int bytePosition;
    private int? position;

    /// <summary>
    /// Gets the line double clicked.
    /// </summary>
    /// <returns>The zero-based index of the double clicked line.</returns>
    public virtual int Line { get; private set; }

    /// <summary>
    /// Gets the modifier keys (SHIFT, CTRL, ALT) held down when double clicked.
    /// </summary>
    /// <returns>A bitwise combination of the Keys enumeration indicating the modifier keys.</returns>
    public virtual TKeys Modifiers { get; private set; }

    /// <summary>
    /// Gets the zero-based document position of the text double clicked.
    /// </summary>
    /// <returns>
    /// The zero-based character position within the document of the double clicked text;
    /// otherwise, -1 if not a document position.
    /// </returns>
    public int Position
    {
        get
        {
            if (position == null)
                position = scintilla.Lines.ByteToCharPosition(bytePosition);

            return (int)position;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleClickEventArgsBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor, TKeys}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="modifiers">The modifier keys that where held down at the time of the double click.</param>
    /// <param name="bytePosition">The zero-based byte position of the double clicked text.</param>
    /// <param name="line">The zero-based line index of the double clicked text.</param>
    protected DoubleClickEventArgsBase(
        IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle,
            TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, TKeys modifiers, int bytePosition,
        int line) : base(scintilla)
    {
        this.bytePosition = bytePosition;
        Modifiers = modifiers;
        Line = line;

        if (bytePosition == -1)
            position = -1;
    }
}