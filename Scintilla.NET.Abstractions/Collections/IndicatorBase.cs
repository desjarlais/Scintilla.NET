using System.Collections;
using Scintilla.NET.Abstractions.Enumerations;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// Represents an indicator in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class IndicatorBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> 
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
    #region Fields    
    /// <summary>
    /// A reference to the Scintilla control interface.
    /// </summary>
    protected readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;

    #endregion Fields

    #region Constants

    /// <summary>
    /// An OR mask to use with <see cref="Scintilla.IndicatorValue" /> and <see cref="IndicatorFlags.ValueFore" /> to indicate
    /// that the user-defined indicator value should be treated as a RGB color.
    /// </summary>
    public const int ValueBit = ScintillaConstants.SC_INDICVALUEBIT;

    /// <summary>
    /// An AND mask to use with <see cref="Indicator.ValueAt" /> to retrieve the user-defined value as a RGB color when being treated as such.
    /// </summary>
    public const int ValueMask = ScintillaConstants.SC_INDICVALUEMASK;

    #endregion Constants

    #region Methods

    /// <summary>
    /// Given a document position which is filled with this indicator, will return the document position
    /// where the use of this indicator ends.
    /// </summary>
    /// <param name="position">A zero-based document position using this indicator.</param>
    /// <returns>The zero-based document position where the use of this indicator ends.</returns>
    /// <remarks>
    /// Specifying a <paramref name="position" /> which is not filled with this indicator will cause this method
    /// to return the end position of the range where this indicator is not in use (the negative space). If this
    /// indicator is not in use anywhere within the document the return value will be 0.
    /// </remarks>
    public virtual int End(int position)
    {
        position = HelpersGeneral.Clamp(position, 0, scintilla.TextLength);
        position = scintilla.Lines.CharToBytePosition(position);
        position = scintilla.DirectMessage(ScintillaConstants.SCI_INDICATOREND, new IntPtr(Index), new IntPtr(position)).ToInt32();
        return scintilla.Lines.ByteToCharPosition(position);
    }

    /// <summary>
    /// Given a document position which is filled with this indicator, will return the document position
    /// where the use of this indicator starts.
    /// </summary>
    /// <param name="position">A zero-based document position using this indicator.</param>
    /// <returns>The zero-based document position where the use of this indicator starts.</returns>
    /// <remarks>
    /// Specifying a <paramref name="position" /> which is not filled with this indicator will cause this method
    /// to return the start position of the range where this indicator is not in use (the negative space). If this
    /// indicator is not in use anywhere within the document the return value will be 0.
    /// </remarks>
    public virtual int Start(int position)
    {
        position = HelpersGeneral.Clamp(position, 0, scintilla.TextLength);
        position = scintilla.Lines.CharToBytePosition(position);
        position = scintilla.DirectMessage(ScintillaConstants.SCI_INDICATORSTART, new IntPtr(Index), new IntPtr(position)).ToInt32();
        return scintilla.Lines.ByteToCharPosition(position);
    }

    /// <summary>
    /// Returns the user-defined value for the indicator at the specified position.
    /// </summary>
    /// <param name="position">The zero-based document position to get the indicator value for.</param>
    /// <returns>The user-defined value at the specified <paramref name="position" />.</returns>
    public virtual int ValueAt(int position)
    {
        position = HelpersGeneral.Clamp(position, 0, scintilla.TextLength);
        position = scintilla.Lines.CharToBytePosition(position);

        return scintilla.DirectMessage(SCI_INDICATORVALUEAT, new IntPtr(Index), new IntPtr(position)).ToInt32();
    }

    #endregion Methods

    #region Properties

    /// <summary>
    /// Gets or sets the alpha transparency of the indicator.
    /// </summary>
    /// <returns>
    /// The alpha transparency ranging from 0 (completely transparent)
    /// to 255 (no transparency). The default is 30.
    /// </returns>
    public virtual int Alpha
    {
        get
        {
            return scintilla.DirectMessage(SCI_INDICGETALPHA, new IntPtr(Index)).ToInt32();
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, 255);
            scintilla.DirectMessage(SCI_INDICSETALPHA, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the indicator flags.
    /// </summary>
    /// <returns>
    /// A bitwise combination of the <see cref="IndicatorFlags" /> enumeration.
    /// The default is <see cref="IndicatorFlags.None" />.
    /// </returns>
    public virtual IndicatorFlags Flags
    {
        get
        {
            return (IndicatorFlags)scintilla.DirectMessage(SCI_INDICGETFLAGS, new IntPtr(Index));
        }
        set
        {
            int flags = (int)value;
            scintilla.DirectMessage(SCI_INDICSETFLAGS, new IntPtr(Index), new IntPtr(flags));
        }
    }

    /// <summary>
    /// Gets or sets the color used to draw an indicator.
    /// </summary>
    /// <returns>The Color used to draw an indicator. The default varies.</returns>
    /// <remarks>Changing the <see cref="ForeColor" /> property will reset the <see cref="HoverForeColor" />.</remarks>
    /// <seealso cref="HoverForeColor" />
    public abstract TColor ForeColor { get; set; }

    /// <summary>
    /// Gets or sets the color used to draw an indicator when the mouse or caret is over an indicator.
    /// </summary>
    /// <returns>
    /// The Color used to draw an indicator.
    /// By default, the hover style is equal to the regular <see cref="ForeColor" />.
    /// </returns>
    /// <remarks>Changing the <see cref="ForeColor" /> property will reset the <see cref="HoverForeColor" />.</remarks>
    /// <seealso cref="ForeColor" />
    public abstract TColor HoverForeColor { get; set; }

    /// <summary>
    /// Gets or sets the indicator style used when the mouse or caret is over an indicator.
    /// </summary>
    /// <returns>
    /// One of the <see cref="IndicatorStyle" /> enumeration values.
    /// By default, the hover style is equal to the regular <see cref="Style" />.
    /// </returns>
    /// <remarks>Changing the <see cref="Style" /> property will reset the <see cref="HoverStyle" />.</remarks>
    /// <seealso cref="Style" />
    public virtual IndicatorStyle HoverStyle
    {
        get
        {
            return (IndicatorStyle)scintilla.DirectMessage(SCI_INDICGETHOVERSTYLE, new IntPtr(Index));
        }
        set
        {
            var style = (int)value;
            scintilla.DirectMessage(SCI_INDICSETHOVERSTYLE, new IntPtr(Index), new IntPtr(style));
        }
    }

    /// <summary>
    /// Gets the zero-based indicator index this object represents.
    /// </summary>
    /// <returns>The indicator definition index within the <see cref="IndicatorCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" />.</returns>
    protected int Index { get; private set; }

    /// <summary>
    /// Gets or sets the alpha transparency of the indicator outline.
    /// </summary>
    /// <returns>
    /// The alpha transparency ranging from 0 (completely transparent)
    /// to 255 (no transparency). The default is 50.
    /// </returns>
    public virtual int OutlineAlpha
    {
        get
        {
            return scintilla.DirectMessage(SCI_INDICGETOUTLINEALPHA, new IntPtr(Index)).ToInt32();
        }
        set
        {
            value = HelpersGeneral.Clamp(value, 0, 255);
            scintilla.DirectMessage(SCI_INDICSETOUTLINEALPHA, new IntPtr(Index), new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets or sets the indicator style.
    /// </summary>
    /// <returns>One of the <see cref="IndicatorStyle" /> enumeration values. The default varies.</returns>
    /// <remarks>Changing the <see cref="Style" /> property will reset the <see cref="HoverStyle" />.</remarks>
    /// <seealso cref="HoverStyle" />
    public virtual IndicatorStyle Style
    {
        get
        {
            return (IndicatorStyle)scintilla.DirectMessage(SCI_INDICGETSTYLE, new IntPtr(Index));
        }
        set
        {
            var style = (int)value;
            scintilla.DirectMessage(SCI_INDICSETSTYLE, new IntPtr(Index), new IntPtr(style));
        }
    }

    /// <summary>
    /// Gets or sets whether indicators are drawn under or over text.
    /// </summary>
    /// <returns>true to draw the indicator under text; otherwise, false. The default is false.</returns>
    /// <remarks>Drawing indicators under text requires <see cref="Phases.One" /> or <see cref="Phases.Multiple" /> drawing.</remarks>
    public virtual bool Under
    {
        get
        {
            return (scintilla.DirectMessage(SCI_INDICGETUNDER, new IntPtr(Index)) != IntPtr.Zero);
        }
        set
        {
            var under = (value ? new IntPtr(1) : IntPtr.Zero);
            scintilla.DirectMessage(SCI_INDICSETUNDER, new IntPtr(Index), under);
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IndicatorBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this indicator.</param>
    /// <param name="index">The index of this style within the <see cref="IndicatorCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> that created it.</param>
    protected IndicatorBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla, int index)
    {
        this.scintilla = scintilla;
        Index = index;
    }

    #endregion Constructors
}