using System;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Collections;

namespace ScintillaNET;

/// <summary>
/// A style definition in a <see cref="Scintilla" /> control.
/// </summary>
public class Style : StyleBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    #region Properties

    /// <summary>
    /// Gets or sets the background color of the style.
    /// </summary>
    /// <returns>A Color object representing the style background color. The default is White.</returns>
    /// <remarks>Alpha color values are ignored.</remarks>
    public override Color BackColor
    {
        get
        {
            var color = scintilla.DirectMessage(ScintillaConstants.SCI_STYLEGETBACK, new IntPtr(Index), IntPtr.Zero).ToInt32();
            return ColorTranslator.FromWin32(color);
        }
        set
        {
            if (value.IsEmpty)
                value = Color.White;

            var color = ColorTranslator.ToWin32(value);
            scintilla.DirectMessage(ScintillaConstants.SCI_STYLESETBACK, new IntPtr(Index), new IntPtr(color));
        }
    }

    /// <summary>
    /// Gets or sets the foreground color of the style.
    /// </summary>
    /// <returns>A Color object representing the style foreground color. The default is Black.</returns>
    /// <remarks>Alpha color values are ignored.</remarks>
    public override Color ForeColor
    {
        get
        {
            var color = scintilla.DirectMessage(ScintillaConstants.SCI_STYLEGETFORE, new IntPtr(Index), IntPtr.Zero).ToInt32();
            return ColorTranslator.FromWin32(color);
        }
        set
        {
            if (value.IsEmpty)
                value = Color.Black;

            var color = ColorTranslator.ToWin32(value);
            scintilla.DirectMessage(ScintillaConstants.SCI_STYLESETFORE, new IntPtr(Index), new IntPtr(color));
        }
    }
    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instances of the <see cref="Style" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this style.</param>
    /// <param name="index">The index of this style within the <see cref="StyleCollection" /> that created it.</param>
    public Style(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, int index) : base(scintilla, index)
    {
    }

    #endregion Constructors
}