using System.Collections;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// Represents a selection when there are multiple active selections in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class SelectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> 
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
    /// <summary>
    /// A reference to the Scintilla control interface.
    /// </summary>
    protected readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;

    /// <summary>
    /// Gets or sets the anchor position of the selection.
    /// </summary>
    /// <returns>The zero-based document position of the selection anchor.</returns>
    public virtual int Anchor
    {
        get
        {
            var pos = scintilla.DirectMessage(SCI_GETSELECTIONNANCHOR, new IntPtr(Index)).ToInt32();
            if (pos <= 0)
                return pos;

            return scintilla.Lines.ByteToCharPosition(pos);
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, scintilla.TextLength);
            value = scintilla.Lines.CharToBytePosition(value);
            scintilla.DirectMessage(SCI_SETSELECTIONNANCHOR, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the amount of anchor virtual space.
    /// </summary>
    /// <returns>The amount of virtual space past the end of the line offsetting the selection anchor.</returns>
    public virtual int AnchorVirtualSpace
    {
        get
        {
            return scintilla.DirectMessage(SCI_GETSELECTIONNANCHORVIRTUALSPACE, new IntPtr(Index)).ToInt32();
        }
        set
        {
            value = HelpersGeneral.ClampMin(value, 0);
            scintilla.DirectMessage(SCI_SETSELECTIONNANCHORVIRTUALSPACE, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the caret position of the selection.
    /// </summary>
    /// <returns>The zero-based document position of the selection caret.</returns>
    public virtual int Caret
    {
        get
        {
            var pos = scintilla.DirectMessage(SCI_GETSELECTIONNCARET, new IntPtr(Index)).ToInt32();
            if (pos <= 0)
                return pos;

            return scintilla.Lines.ByteToCharPosition(pos);
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, scintilla.TextLength);
            value = scintilla.Lines.CharToBytePosition(value);
            scintilla.DirectMessage(SCI_SETSELECTIONNCARET, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the amount of caret virtual space.
    /// </summary>
    /// <returns>The amount of virtual space past the end of the line offsetting the selection caret.</returns>
    public virtual int CaretVirtualSpace
    {
        get
        {
            return scintilla.DirectMessage(SCI_GETSELECTIONNCARETVIRTUALSPACE, new IntPtr(Index)).ToInt32();
        }
        set
        {
            value = HelpersGeneral.ClampMin(value, 0);
            scintilla.DirectMessage(SCI_SETSELECTIONNCARETVIRTUALSPACE, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the end position of the selection.
    /// </summary>
    /// <returns>The zero-based document position where the selection ends.</returns>
    public virtual int End
    {
        get
        {
            var pos = scintilla.DirectMessage(SCI_GETSELECTIONNEND, new IntPtr(Index)).ToInt32();
            if (pos <= 0)
                return pos;

            return scintilla.Lines.ByteToCharPosition(pos);
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, scintilla.TextLength);
            value = scintilla.Lines.CharToBytePosition(value);
            scintilla.DirectMessage(SCI_SETSELECTIONNEND, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets the selection index.
    /// </summary>
    /// <returns>The zero-based selection index within the <see cref="SelectionCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> that created it.</returns>
    public int Index { get; private set; }

    /// <summary>
    /// Gets or sets the start position of the selection.
    /// </summary>
    /// <returns>The zero-based document position where the selection starts.</returns>
    public virtual int Start
    {
        get
        {
            var pos = scintilla.DirectMessage(SCI_GETSELECTIONNSTART, new IntPtr(Index)).ToInt32();
            if (pos <= 0)
                return pos;

            return scintilla.Lines.ByteToCharPosition(pos);
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, scintilla.TextLength);
            value = scintilla.Lines.CharToBytePosition(value);
            scintilla.DirectMessage(SCI_SETSELECTIONNSTART, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this selection.</param>
    /// <param name="index">The index of this selection within the <see cref="SelectionCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> that created it.</param>
    protected SelectionBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, int index)
    {
        this.scintilla = scintilla;
        Index = index;
    }
}