using System.Collections;
using Scintilla.NET.Abstractions.Collections;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the <see cref="Scintilla.InsertCheck" /> event.
/// </summary>
public abstract class InsertCheckEventArgsBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> 
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
{
    private readonly int bytePosition;
    private readonly int byteLength;
    private readonly IntPtr textPtr;

    public virtual int? CachedPosition { get; set; }
    
    public virtual string? CachedText { get; set; }

    /// <summary>
    /// Gets the zero-based document position where text will be inserted.
    /// </summary>
    /// <returns>The zero-based character position within the document where text will be inserted.</returns>
    public virtual int Position
    {
        get
        {
            CachedPosition ??= scintilla.Lines.ByteToCharPosition(bytePosition);

            return (int)CachedPosition;
        }
    }

    /// <summary>
    /// Gets or sets the text being inserted.
    /// </summary>
    /// <returns>The text being inserted into the document.</returns>
    public virtual unsafe string? Text
    {
        get => CachedText ??= HelpersGeneral.GetString(textPtr, byteLength, scintilla.Encoding);
        set
        {
            CachedText = value ?? string.Empty;

            var bytes = HelpersGeneral.GetBytes(CachedText, scintilla.Encoding, zeroTerminated: false);
            fixed (byte* bp = bytes)
                scintilla.DirectMessage(SCI_CHANGEINSERTION, new IntPtr(bytes.Length), new IntPtr(bp));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InsertCheckEventArgsBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where text is being inserted.</param>
    /// <param name="byteLength">The length in bytes of the inserted text.</param>
    /// <param name="text">A pointer to the text being inserted.</param>
    protected InsertCheckEventArgsBase(
        IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle,
            TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, int bytePosition, int byteLength,
        IntPtr text) : base(scintilla)
    {
        this.bytePosition = bytePosition;
        this.byteLength = byteLength;
        textPtr = text;
    }
}