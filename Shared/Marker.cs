using System;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Collections;
using Scintilla.NET.Abstractions.Enumerations;
using static Scintilla.NET.Abstractions.ScintillaConstants;

namespace ScintillaNET;

/// <summary>
/// Represents a margin marker in a <see cref="Scintilla" /> control.
/// </summary>
public class Marker : MarkerBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Sets the marker symbol to a custom image.
    /// </summary>
    /// <param name="image">The Bitmap to use as a marker symbol.</param>
    /// <remarks>Calling this method will also update the <see cref="MarkerBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}.Symbol" /> property to <see cref="MarkerSymbol.RgbaImage" />.</remarks>
    public override unsafe void DefineRgbaImage(Bitmap image)
    {
        if (image == null)
            return;

        scintilla.DirectMessage(SCI_RGBAIMAGESETWIDTH, new IntPtr(image.Width));
        scintilla.DirectMessage(SCI_RGBAIMAGESETHEIGHT, new IntPtr(image.Height));

        var bytes = Helpers.BitmapToArgb(image);
        fixed (byte* bp = bytes)
            scintilla.DirectMessage(SCI_MARKERDEFINERGBAIMAGE, new IntPtr(Index), new IntPtr(bp));
    }

    /// <summary>
    /// Sets the background color of the marker.
    /// </summary>
    /// <param name="color">The <see cref="Marker" /> background Color. The default is White.</param>
    /// <remarks>
    /// The background color of the whole line will be drawn in the <paramref name="color" /> specified when the marker is not visible
    /// because it is hidden by a <see cref="Margin.Mask" /> or the <see cref="Margin.Width" /> is zero.
    /// </remarks>
    /// <seealso cref="MarkerBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}.SetAlpha" />
    public override void SetBackColor(Color color)
    {
        var colorNum = ColorTranslator.ToWin32(color);
        scintilla.DirectMessage(SCI_MARKERSETBACK, new IntPtr(Index), new IntPtr(colorNum));
    }

    /// <summary>
    /// Sets the foreground color of the marker.
    /// </summary>
    /// <param name="color">The <see cref="Marker" /> foreground Color. The default is Black.</param>
    public override void SetForeColor(Color color)
    {
        var colorNum = ColorTranslator.ToWin32(color);
        scintilla.DirectMessage(SCI_MARKERSETFORE, new IntPtr(Index), new IntPtr(colorNum));
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Marker" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this marker.</param>
    /// <param name="index">The index of this style within the <see cref="MarkerCollection" /> that created it.</param>
    public Marker(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, int index) : base(scintilla, index)
    {
    }
}