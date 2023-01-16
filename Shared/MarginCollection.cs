using System.ComponentModel;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Collections;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace ScintillaNET;

/// <summary>
/// An immutable collection of margins in a <see cref="Scintilla" /> control.
/// </summary>
public class MarginCollection : MarginCollectionBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Gets or sets the number of margins in the <see cref="MarginCollection" />.
    /// </summary>
    /// <returns>The number of margins in the collection. The default is 5.</returns>
    [DefaultValue(SC_MAX_MARGIN + 1)]
    [Description("The maximum number of margins.")]
    public override int Capacity
    {
        get => base.Capacity;

        set => base.Capacity = value;
    }

    /// <summary>
    /// Gets or sets the width in pixels of the left margin padding.
    /// </summary>
    /// <returns>The left margin padding measured in pixels. The default is 1.</returns>
    [DefaultValue(1)]
    [Description("The left margin padding in pixels.")]
    public override int Left
    {
        get => base.Left;

        set => base.Left = value;
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
    public override int Right
    {
        get => base.Right;

        set => base.Right = value;
    }

    /// <summary>
    /// Gets a <see cref="Margin" /> object at the specified index.
    /// </summary>
    /// <param name="index">The margin index.</param>
    /// <returns>An object representing the margin at the specified <paramref name="index" />.</returns>
    /// <remarks>By convention margin 0 is used for line numbers and the two following for symbols.</remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Margin this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Margin(scintilla, index);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarginCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    public MarginCollection(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla) : base(scintilla)
    {
    }
}