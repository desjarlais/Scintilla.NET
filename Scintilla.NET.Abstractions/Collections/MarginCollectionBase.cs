using System.Collections;
using System.ComponentModel;
using Scintilla.NET.Abstractions.Enumerations;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace Scintilla.NET.Abstractions.Collections;

/// <summary>
/// An immutable collection of margins in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class MarginCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> : IEnumerable<TMargin>
    where TMarkers : MarkerCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TStyles : StyleCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TIndicators :IndicatorCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TLines : LineCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TMargins : MarginCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TSelections : SelectionCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TEventArgs : System.EventArgs
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
    /// Removes all text displayed in every <see cref="MarginType.Text" /> and <see cref="MarginType.RightText" /> margins.
    /// </summary>
    public virtual void ClearAllText()
    {
        scintilla.DirectMessage(SCI_MARGINTEXTCLEARALL);
    }

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An object that contains all <see cref="Margin" /> objects within the <see cref="MarginCollection" />.</returns>
    public virtual IEnumerator<TMargin> GetEnumerator()
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
    /// Gets or sets the number of margins in the <see cref="MarginCollection" />.
    /// </summary>
    /// <returns>The number of margins in the collection. The default is 5.</returns>
    public virtual int Capacity
    {
        get
        {
            return scintilla.DirectMessage(SCI_GETMARGINS).ToInt32();
        }
        set
        {
            value = HelpersGeneral.ClampMin(value, 0);
            scintilla.DirectMessage(SCI_SETMARGINS, new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets the number of margins in the <see cref="MarginCollection" />.
    /// </summary>
    /// <returns>The number of margins in the collection.</returns>
    /// <remarks>This property is kept for convenience. The return value will always be equal to <see cref="Capacity" />.</remarks>
    /// <seealso cref="Capacity" />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Count
    {
        get
        {
            return Capacity;
        }
    }

    /// <summary>
    /// Gets or sets the width in pixels of the left margin padding.
    /// </summary>
    /// <returns>The left margin padding measured in pixels. The default is 1.</returns>
    [DefaultValue(1)]
    [Description("The left margin padding in pixels.")]
    public virtual int Left
    {
        get
        {
            return scintilla.DirectMessage(SCI_GETMARGINLEFT).ToInt32();
        }
        set
        {
            value = HelpersGeneral.ClampMin(value, 0);
            scintilla.DirectMessage(SCI_SETMARGINLEFT, IntPtr.Zero, new IntPtr(value));
        }
    }

    // TODO Why is this commented out?
    /*
    /// <summary>
    /// Gets or sets the margin options.
    /// </summary>
    /// <returns>
    /// A <see cref="ScintillaNET.MarginOptions" /> that represents the margin options.
    /// The default is <see cref="ScintillaNET.MarginOptions.None" />.
    /// </returns>
    [DefaultValue(MarginOptions.None)]
    [Description("Margin options flags.")]
    [TypeConverter(typeof(FlagsEnumTypeConverter.FlagsEnumConverter))]
    public MarginOptions Options
    {
        get
        {
            return (MarginOptions)scintilla.DirectMessage(NativeMethods.SCI_GETMARGINOPTIONS);
        }
        set
        {
            var options = (int)value;
            scintilla.DirectMessage(NativeMethods.SCI_SETMARGINOPTIONS, new IntPtr(options));
        }
    }
    */

    /// <summary>
    /// Gets or sets the width in pixels of the right margin padding.
    /// </summary>
    /// <returns>The right margin padding measured in pixels. The default is 1.</returns>
    [DefaultValue(1)]
    [Description("The right margin padding in pixels.")]
    public virtual int Right
    {
        get
        {
            return scintilla.DirectMessage(SCI_GETMARGINRIGHT).ToInt32();
        }
        set
        {
            value = HelpersGeneral.ClampMin(value, 0);
            scintilla.DirectMessage(SCI_SETMARGINRIGHT, IntPtr.Zero, new IntPtr(value));
        }
    }

    /// <summary>
    /// Gets a <see cref="Margin" /> object at the specified index.
    /// </summary>
    /// <param name="index">The margin index.</param>
    /// <returns>An object representing the margin at the specified <paramref name="index" />.</returns>
    /// <remarks>By convention margin 0 is used for line numbers and the two following for symbols.</remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public abstract TMargin this[int index] { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarginCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    protected MarginCollectionBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla)
    {
        this.scintilla = scintilla;
    }
}