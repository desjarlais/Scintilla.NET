using System.Collections;
using Scintilla.NET.Abstractions.Enumerations;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// Represents a margin marker in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class MarkerBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
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
    public readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;

    /// <summary>
    /// An unsigned 32-bit mask of all <see cref="MarginBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> indexes where each bit corresponds to a margin index.
    /// </summary>
    public const uint MaskAll = unchecked((uint)-1);

    /// <summary>
    /// An unsigned 32-bit mask of folder <see cref="MarginBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> indexes (25 through 31) where each bit corresponds to a margin index.
    /// </summary>
    /// <seealso cref="MarginBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}.Mask" />
    public const uint MaskFolders = SC_MASK_FOLDERS;

    /// <summary>
    /// Folder end marker index. This marker is typically configured to display the <see cref="MarkerSymbol.BoxPlusConnected" /> symbol.
    /// </summary>
    public const int FolderEnd = SC_MARKNUM_FOLDEREND;

    /// <summary>
    /// Folder open marker index. This marker is typically configured to display the <see cref="MarkerSymbol.BoxMinusConnected" /> symbol.
    /// </summary>
    public const int FolderOpenMid = SC_MARKNUM_FOLDEROPENMID;

    /// <summary>
    /// Folder mid tail marker index. This marker is typically configured to display the <see cref="MarkerSymbol.TCorner" /> symbol.
    /// </summary>
    public const int FolderMidTail = SC_MARKNUM_FOLDERMIDTAIL;

    /// <summary>
    /// Folder tail marker index. This marker is typically configured to display the <see cref="MarkerSymbol.LCorner" /> symbol.
    /// </summary>
    public const int FolderTail = SC_MARKNUM_FOLDERTAIL;

    /// <summary>
    /// Folder sub marker index. This marker is typically configured to display the <see cref="MarkerSymbol.VLine" /> symbol.
    /// </summary>
    public const int FolderSub = SC_MARKNUM_FOLDERSUB;

    /// <summary>
    /// Folder marker index. This marker is typically configured to display the <see cref="MarkerSymbol.BoxPlus" /> symbol.
    /// </summary>
    public const int Folder = SC_MARKNUM_FOLDER;

    /// <summary>
    /// Folder open marker index. This marker is typically configured to display the <see cref="MarkerSymbol.BoxMinus" /> symbol.
    /// </summary>
    public const int FolderOpen = SC_MARKNUM_FOLDEROPEN;

    /// <summary>
    /// Sets the marker symbol to a custom image.
    /// </summary>
    /// <param name="image">The Bitmap to use as a marker symbol.</param>
    /// <remarks>Calling this method will also update the <see cref="Symbol" /> property to <see cref="MarkerSymbol.RgbaImage" />.</remarks>
    public abstract void DefineRgbaImage(TBitmap image);

    /// <summary>
    /// Removes this marker from all lines.
    /// </summary>
    public void DeleteAll()
    {
        scintilla.MarkerDeleteAll(Index);
    }

    /// <summary>
    /// Sets the foreground alpha transparency for markers that are drawn in the content area.
    /// </summary>
    /// <param name="alpha">The alpha transparency ranging from 0 (completely transparent) to 255 (no transparency).</param>
    /// <remarks>See the remarks on the <see cref="SetBackColor" /> method for a full explanation of when a marker can be drawn in the content area.</remarks>
    /// <seealso cref="SetBackColor" />
    public virtual void SetAlpha(int alpha)
    {
        alpha = HelpersGeneral.Clamp(alpha, 0, 255);
        scintilla.DirectMessage(SCI_MARKERSETALPHA, new IntPtr(Index), new IntPtr(alpha));
    }

    /// <summary>
    /// Sets the background color of the marker.
    /// </summary>
    /// <param name="color">The <see cref="MarkerBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> background Color. The default is White.</param>
    /// <remarks>
    /// The background color of the whole line will be drawn in the <paramref name="color" /> specified when the marker is not visible
    /// because it is hidden by a <see cref="MarginBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}.Mask" /> or the <see cref="MarginBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}.Width" /> is zero.
    /// </remarks>
    /// <seealso cref="SetAlpha" />
    public abstract void SetBackColor(TColor color);

    /// <summary>
    /// Sets the foreground color of the marker.
    /// </summary>
    /// <param name="color">The <see cref="MarkerBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> foreground Color. The default is Black.</param>
    public abstract void SetForeColor(TColor color);

    /// <summary>
    /// Gets the zero-based marker index this object represents.
    /// </summary>
    /// <returns>The marker index within the <see cref="MarkerCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" />.</returns>
    public int Index { get; private set; }

    /// <summary>
    /// Gets or sets the marker symbol.
    /// </summary>
    /// <returns>
    /// One of the <see cref="MarkerSymbol" /> enumeration values.
    /// The default is <see cref="MarkerSymbol.Circle" />.
    /// </returns>
    public virtual MarkerSymbol Symbol
    {
        get
        {
            return (MarkerSymbol)scintilla.DirectMessage(SCI_MARKERSYMBOLDEFINED, new IntPtr(Index));
        }
        set
        {
            var markerSymbol = (int)value;
            scintilla.DirectMessage(SCI_MARKERDEFINE, new IntPtr(Index), new IntPtr(markerSymbol));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkerBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this marker.</param>
    /// <param name="index">The index of this style within the <see cref="MarkerCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> that created it.</param>
    public MarkerBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, int index)
    {
        this.scintilla = scintilla;
        Index = index;
    }
}