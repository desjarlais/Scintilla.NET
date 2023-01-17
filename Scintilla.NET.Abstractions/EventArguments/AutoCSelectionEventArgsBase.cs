using System.Collections;
using Scintilla.NET.Abstractions.Collections;
using Scintilla.NET.Abstractions.Enumerations;

namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the Scintilla.AutoCSelection event.
/// </summary>
public abstract class AutoCSelectionEventArgsBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> 
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
    private readonly IntPtr textPtr;
    private readonly int bytePosition;
    private int? position;
    private string? text;

    /// <summary>
    /// Gets the fill-up character that caused the completion.
    /// </summary>
    /// <returns>The fill-up character used to cause the completion; otherwise, 0.</returns>
    /// <remarks>Only a <see cref="ListCompletionMethod" /> of <see cref="Scintilla.NET.Abstractions.Enumerations.ListCompletionMethod.FillUp" /> will return a non-zero character.</remarks>
    /// <seealso cref="Scintilla.AutoCSetFillUps" />
    public virtual int Char { get; private set; }

    /// <summary>
    /// Gets a value indicating how the completion occurred.
    /// </summary>
    /// <returns>One of the <see cref="Scintilla.NET.Abstractions.Enumerations.ListCompletionMethod" /> enumeration values.</returns>
    public virtual ListCompletionMethod ListCompletionMethod { get; private set; }

    /// <summary>
    /// Gets the start position of the word being completed.
    /// </summary>
    /// <returns>The zero-based document position of the word being completed.</returns>
    public virtual int Position
    {
        get
        {
            if (position == null)
                position = scintilla.Lines.ByteToCharPosition(bytePosition);

            return (int)position;
        }
    }

    /// <summary>
    /// Gets the text of the selected auto-completion item.
    /// </summary>
    /// <returns>The selected auto-completion item text.</returns>
    public virtual unsafe string Text
    {
        get
        {
            if (text == null)
            {
                var len = 0;
                while (((byte*)textPtr)[len] != 0)
                    len++;

                text = HelpersGeneral.GetString(textPtr, len, scintilla.Encoding);
            }

            return text;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoCSelectionEventArgsBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document of the word being completed.</param>
    /// <param name="text">A pointer to the selected auto-completion text.</param>
    /// <param name="ch">The character that caused the completion.</param>
    /// <param name="listCompletionMethod">A value indicating the way in which the completion occurred.</param>
    protected AutoCSelectionEventArgsBase(
        IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle,
            TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, int bytePosition, IntPtr text, int ch,
        ListCompletionMethod listCompletionMethod) : base(scintilla)
    {
        this.bytePosition = bytePosition;
        textPtr = text;
        Char = ch;
        ListCompletionMethod = listCompletionMethod;
    }
}