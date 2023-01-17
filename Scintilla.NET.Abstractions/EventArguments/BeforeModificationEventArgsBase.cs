using System.Collections;
using Scintilla.NET.Abstractions.Collections;
using Scintilla.NET.Abstractions.Enumerations;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the <see cref="Scintilla.BeforeInsert" /> and <see cref="Scintilla.BeforeDelete" /> events.
/// </summary>
public abstract class BeforeModificationEventArgsBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> 
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

    /// <summary>
    /// Gets or sets the cached position.
    /// </summary>
    /// <value>The cached position.</value>
    public virtual int? CachedPosition { get; set; }

    /// <summary>
    /// Gets or sets the cached text.
    /// </summary>
    /// <value>The cached text.</value>
    public virtual string? CachedText { get; set; }

    /// <summary>
    /// Gets the zero-based document position where the modification will occur.
    /// </summary>
    /// <returns>The zero-based character position within the document where text will be inserted or deleted.</returns>
    public virtual int Position
    {
        get
        {
            CachedPosition ??= scintilla.Lines.ByteToCharPosition(bytePosition);

            return (int)CachedPosition;
        }
    }

    /// <summary>
    /// Gets the source of the modification.
    /// </summary>
    /// <returns>One of the <see cref="ModificationSource" /> enum values.</returns>
    public virtual ModificationSource Source { get; private set; }

    /// <summary>
    /// Gets the text being inserted or deleted.
    /// </summary>
    /// <returns>
    /// The text about to be inserted or deleted, or null when the the source of the modification is an undo/redo operation.
    /// </returns>
    /// <remarks>
    /// This property will return null when <see cref="Source" /> is <see cref="ModificationSource.Undo" /> or <see cref="ModificationSource.Redo" />.
    /// </remarks>
    public virtual unsafe string? Text
    {
        get
        {
            if (Source != ModificationSource.User)
                return null;

            if (CachedText == null)
            {
                // For some reason the Scintilla overlords don't provide text in
                // SC_MOD_BEFOREDELETE... but we can get it from the document.
                if (textPtr == IntPtr.Zero)
                {
                    var ptr = scintilla.DirectMessage(SCI_GETRANGEPOINTER, new IntPtr(bytePosition), new IntPtr(byteLength));
                    CachedText = new string((sbyte*)ptr, 0, byteLength, scintilla.Encoding);
                }
                else
                {
                    CachedText = HelpersGeneral.GetString(textPtr, byteLength, scintilla.Encoding);
                }
            }

            return CachedText;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BeforeModificationEventArgsBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="source">The source of the modification.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where text is being modified.</param>
    /// <param name="byteLength">The length in bytes of the text being modified.</param>
    /// <param name="text">A pointer to the text being inserted.</param>
    protected BeforeModificationEventArgsBase(
        IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle,
            TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, ModificationSource source,
        int bytePosition, int byteLength, IntPtr text) : base(scintilla)
    {
        this.bytePosition = bytePosition;
        this.byteLength = byteLength;
        textPtr = text;

        Source = source;
    }
}